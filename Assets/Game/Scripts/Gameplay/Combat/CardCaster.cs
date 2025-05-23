using ForestLib.Utils;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Systems;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Combat
{
	public class CardCaster : MonoBehaviour
	{
		[SerializeField, Range(0f, 1f)]
		private float _castingLinePosition = 0.3f;

		[SerializeField]
		private List<CardSlot> _cardSlots;

		[Inject]
		private ApplicationEvents _applicationEvents;

		private Hand _hand;
		private Deck _deck;
		private Player _player;

		private void Awake()
		{
			Debug.Log($"CardCaster - Awake ({_applicationEvents})");
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
				cardView.OnClick += OnCardClicked;
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

		private void OnCardClicked(CardSlot cardSlot, CardData cardData)
		{
			Debug.Log($"CardCaster - OnCardClicked ({cardSlot})");
			foreach (CardSlot otherCardView in _cardSlots)
			{
				otherCardView.SetSelected(otherCardView == cardSlot);
			}
		}

		// ------------------------------------------------

		private void OnDrawGizmos()
		{
			Camera camera = Camera.main;
			if (camera == null)
			{
				return;
			}

			GizmoUtils.DrawHorizontalLineOnScreen(_castingLinePosition, Color.red);
		}
	}
}
