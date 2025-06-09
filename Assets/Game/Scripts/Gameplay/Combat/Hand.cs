using ForestRoyale.Gameplay.Cards;
using System;
using System.Collections.Generic;

namespace ForestRoyale.Gameplay.Combat
{
	public class Hand
	{
		private const int MAX_CARDS = 4;
		private readonly List<CardData> _cards = new();

		public List<CardData> Cards => _cards;
		public bool CanDrawMore => _cards.Count < MAX_CARDS;
		public bool IsFull => _cards.Count >= MAX_CARDS;


		public bool AddCard(CardData card)
		{
			if (IsFull)
			{
				return false;
			}

			_cards.Add(card);
			return true;
		}

		public bool RemoveCard(CardData card)
		{
			if (!_cards.Remove(card))
			{
				return false;
			}

			return true;
		}

		public void Clear()
		{
			_cards.Clear();
		}
	}
}