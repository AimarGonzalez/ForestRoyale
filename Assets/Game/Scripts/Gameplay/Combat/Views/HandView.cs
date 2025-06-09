using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Systems;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Combat.Views
{
	public class HandView : MonoBehaviour
	{
		[Inject]
		private ArenaEvents _arenaEvents;

		private Battle _battle;

		private CardSlot[] _cardSlots;

		public IEnumerable<CardSlot> CardSlots => _cardSlots;

		private void Awake()
		{
			_arenaEvents.OnBattleCreated += HandleBattleCreated;

			_cardSlots = GetComponentsInChildren<CardSlot>();
		}

		private void HandleBattleCreated(Battle battle)
		{
			Unsubscribe();
			_battle = battle;
			Subscribe();

			ClearSlots();
		}

		private void Subscribe()
		{
			if (_battle == null)
			{
				return;
			}
			
			_battle.Player.OnHandChanged += HandleHandChanged;
			_battle.Player.OnCardDrawn += HandleCardDrawn;
			_battle.Player.OnCardPlayed += HandleCardPlayed;
		}

		private void Unsubscribe()
		{
			if (_battle == null)
			{
				return;
			}
			
			_battle.Player.OnHandChanged -= HandleHandChanged;
			_battle.Player.OnCardDrawn -= HandleCardDrawn;
			_battle.Player.OnCardPlayed -= HandleCardPlayed;
		}

		private void ClearSlots()
		{
			foreach (var slot in _cardSlots)
			{
				slot.Clear();
			}
		}

		private void HandleHandChanged(List<CardData> cards)
		{
			for(int i = 0; i < cards.Count; i++)
			{
				_cardSlots[i].CardData = cards[i];
			}
		}

		private void HandleCardDrawn(CardData card)
		{
			//find first empty slot and assign the new card
			foreach (var slot in _cardSlots)
			{
				if (slot.CardData == null)
				{
					slot.CardData = card;
					return;
				}
			}
		}

		private void HandleCardPlayed(CardData card)
		{
			// Do nothing
		}
	}
}
