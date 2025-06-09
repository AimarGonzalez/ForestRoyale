using ForestRoyale.Gameplay.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	public class Deck
	{
		private readonly List<CardData> _cards = new();
		private readonly Queue<CardData> _drawPile = new();

		public IReadOnlyList<CardData> Cards => _cards;
		public bool CanDraw => _drawPile.Count > 0;

		public void Initialize(IEnumerable<CardData> cards)
		{
			_cards.Clear();
			_drawPile.Clear();
			foreach (var card in cards)
			{
				_cards.Add(card);
			}
			
			Shuffle();
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
	}
}