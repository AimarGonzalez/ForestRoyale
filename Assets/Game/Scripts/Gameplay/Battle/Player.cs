using ForestRoyale.Gameplay.Cards;
using UnityEngine;

namespace ForestRoyale.Gameplay.Battle
{
	public class Player
	{
		private float _maxElixir = 10f;
		private float _elixirRegenRate = 1f;
		private float _initialElixir = 5f;
		private float _currentElixir;
		private Hand _hand;
		private Deck _deck;

		public float CurrentElixir => _currentElixir;
		public float MaxElixir => _maxElixir;
		public bool IsElixirFull => _currentElixir >= _maxElixir;
		public Hand Hand => _hand;
		public Deck Deck => _deck;

		public Player(float maxElixir = 10f, float elixirRegenRate = 1f, float initialElixir = 5f)
		{
			_maxElixir = maxElixir;
			_elixirRegenRate = elixirRegenRate;
			_initialElixir = initialElixir;
			_hand = new Hand();
			_deck = new Deck();
			_currentElixir = _initialElixir;
		}

		public void Update(float deltaTime)
		{
			if (!IsElixirFull)
			{
				_currentElixir = Mathf.Min(_currentElixir + (_elixirRegenRate * deltaTime), _maxElixir);
			}
		}

		public bool CanPlayCard(CardData card)
		{
			return _currentElixir >= card.ElixirCost;
		}

		public void SpendElixir(float amount)
		{
			_currentElixir = Mathf.Max(0, _currentElixir - amount);
		}

		public void PopulateInitialHand()
		{
			while (_hand.CanDrawMore)
			{
				DrawCard();
			}
		}

		public void DrawCard()
		{
			if (_deck.CanDraw && _hand.CanDrawMore)
			{
				var card = _deck.DrawCard();
				_hand.AddCard(card);
			}
		}

		public void ReturnCardToDeck(CardData card)
		{
			if (_hand.RemoveCard(card))
			{
				_deck.ReturnCard(card);
			}
		}

		public bool PlayCard(CardData card, Vector3 position)
		{
			if (!_hand.Cards.Contains(card))
			{
				return false;
			}

			if (!CanPlayCard(card))
			{
				return false;
			}

			SpendElixir(card.ElixirCost);

			_hand.RemoveCard(card);
			_deck.ReturnCard(card);

			return true;
		}
	}
}