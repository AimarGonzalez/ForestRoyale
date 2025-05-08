using ForestRoyale.Gameplay.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Battle
{
	public class Deck
	{
		private const int MAX_CARDS = 8;
		private readonly List<CardData> _cards = new();
		private readonly Queue<CardData> _drawPile = new();

		public IReadOnlyList<CardData> Cards => _cards;
		public bool CanDraw => _drawPile.Count > 0;
		public bool IsFull => _cards.Count >= MAX_CARDS;

		public bool AddCard(CardData card)
		{
			if (IsFull)
			{
				return false;
			}

			_cards.Add(card);
			_drawPile.Enqueue(card);
			return true;
		}

		public void RemoveCard(CardData card)
		{
			_cards.Remove(card);
			// Note: We don't remove from draw pile as it's a queue
		}

		public CardData DrawCard()
		{
			if (!CanDraw)
			{
				return null;
			}

			return _drawPile.Dequeue();
		}

		public void ReturnCard(CardData card)
		{
			_drawPile.Enqueue(card);
		}

		public void Shuffle()
		{
			var cardArray = _drawPile.ToArray();
			// Fisher-Yates shuffle
			for (int i = cardArray.Length - 1; i > 0; i--)
			{
				int j = Random.Range(0, i + 1);
				var temp = cardArray[i];
				cardArray[i] = cardArray[j];
				cardArray[j] = temp;
			}

			_drawPile.Clear();
			foreach (var card in cardArray)
			{
				_drawPile.Enqueue(card);
			}
		}

		public void Initialize(IEnumerable<CardData> cards)
		{
			_cards.Clear();
			_drawPile.Clear();
			foreach (var card in cards)
			{
				AddCard(card);
			}

			Shuffle();
		}
	}
}