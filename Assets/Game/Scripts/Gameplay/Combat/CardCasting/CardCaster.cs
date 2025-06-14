using ForestLib.ExtensionMethods;
using ForestLib.Utils;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Combat.Views;
using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units.MonoBehaviours;
using Game.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Combat
{
	public class CardCaster : MonoBehaviour
	{
		[SerializeField, Range(0f, 1f)]
		private float _castingLinePosition = 0.3f;

		[SerializeField, Required]
		private Transform _charactersRoot;


		private HandView _handView;
		private IEnumerable<CardSlot> _cardSlots;

		[Inject]
		private ApplicationEvents _applicationEvents;

		private Battle _battle;
		private Player _player;

		private void Awake()
		{
			_handView = FindAnyObjectByType<HandView>();
			_cardSlots = _handView.CardSlots;
			
			Subscribe();
		}

		private void Start()
		{
			foreach (CardSlot cardView in _cardSlots)
			{
				cardView.Init(_castingLinePosition);
			}
		}

		private void Subscribe()
		{
			_applicationEvents.OnBattleCreated += OnBattleCreated;
			foreach (CardSlot cardView in _cardSlots)
			{
				cardView.OnSelected += OnCardSelected;
				cardView.OnEndDragEvent += OnCardEndDrag;
			}
		}

		private void Unsubscribe()
		{
			_applicationEvents.OnBattleCreated -= OnBattleCreated;
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		private void OnBattleCreated(Battle battle)
		{
			Debug.Log($"CardCaster - OnBattleCreated");
			_battle = battle;
			_player = battle.Player;
		}

		private void OnCardSelected(CardSlot cardSlot, CardData cardData)
		{
			foreach (CardSlot otherCardView in _cardSlots)
			{
				if (otherCardView != cardSlot)
				{
					otherCardView.Deselect();
				}
			}
		}

		private void OnCardEndDrag(CardSlot cardSlot, CardData cardData)
		{
			Debug.Log($"CardCaster - OnCardEndDrag ({cardSlot})");

			// If card can be casted, cast it
			// If card can't be casted, move it back to the hand
			if (CanCastCard(cardSlot))
			{
				Cast(cardSlot);
			}
			else
			{
				cardSlot.StopDragging();
			}
		}

		private bool CanCastCard(CardSlot cardSlot)
		{
			//TODO: Implement
			// - check elixir
			bool isBattleActive = _battle != null && _battle.IsBattleActive;
			bool isCardSlotInPreviewState = cardSlot.IsCastPreviewVisible;

			return isBattleActive && isCardSlotInPreviewState;
		}

		private void Cast(CardSlot cardSlot)
		{
			switch (cardSlot.CardData)
			{
				case TroopCardData troopCardData:
					CastTroop(cardSlot);
					break;

				case SpellCardData spellCardData:
					//TODO: Implement
					break;

				default:
					Debug.LogError($"{nameof(CardCaster)} - Cast: Unknown card type {cardSlot.CardData.GetType().Name}");
					break;
			}
		}

		private void CastTroop(CardSlot cardSlot)
		{
			if (_player.PlayCard(cardSlot.CardData))
			{
				cardSlot.Cast(_charactersRoot);
				// TODO: await;
				_player.DrawCard();
			}
		}

		// -------- GIZMOS----------------------------------------

		private void OnDrawGizmos()
		{
			DrawCastingLineGizmo();
		}

		private void DrawCastingLineGizmo()
		{
			Camera camera = Camera.main;
			if (camera == null)
			{
				return;
			}

			GizmoUtils.DrawHorizontalLineOnScreen(_castingLinePosition * camera.pixelHeight, Color.red);
		}

		// ------------------------------------------------
	}
}
