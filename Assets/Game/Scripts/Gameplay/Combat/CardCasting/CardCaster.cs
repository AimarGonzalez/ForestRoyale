using ForestLib.ExtensionMethods;
using ForestLib.Utils;
using ForestRoyale.Gameplay.Cards;
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

		[SerializeField]
		private List<CardSlot> _cardSlots;

		[Inject]
		private ApplicationEvents _applicationEvents;

		private Hand _hand;
		private Deck _deck;
		private Player _player;

		private void Awake()
		{
		}

		private void Start()
		{
			Subscribe();

			foreach (CardSlot cardView in _cardSlots)
			{
				cardView.Init(_castingLinePosition);
			}
		}

		private void Subscribe()
		{
			_applicationEvents.OnBattleStarted += OnBattleStarted;
			foreach (CardSlot cardView in _cardSlots)
			{
				cardView.OnSelected += OnCardSelected;
				cardView.OnEndDragEvent += OnCardEndDrag;
			}
		}

		private void Unsubscribe()
		{
			_applicationEvents.OnBattleStarted -= OnBattleStarted;
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		private void OnBattleStarted(Battle battle)
		{
			Debug.Log($"CardCaster - OnBattleStarted");
			_player = battle.Player;
			_hand = battle.Player.Hand;
			_deck = battle.Player.Deck;

		}

		private void OnCardSelected(CardSlot cardSlot, CardData cardData)
		{
			Debug.Log($"CardCaster - OnCardSelected ({cardSlot})");
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

			return cardSlot.IsCastPreviewVisible;
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
			cardSlot.Cast(_charactersRoot);
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
