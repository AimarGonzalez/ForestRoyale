using ForestRoyale.Gameplay.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	public class Player
	{

		public event Action<List<CardData>> OnHandChanged;
		public event Action<CardData> OnCardDrawn;
		public event Action<CardData> OnCardPlayed;

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

		public bool CanPayElixirCost(CardData card)
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
			OnHandChanged?.Invoke(_hand.Cards);
		}

		public void DrawCard()
		{
			if (_deck.CanDraw && _hand.CanDrawMore)
			{
				var card = _deck.DrawCard();
				_hand.AddCard(card);
				OnCardDrawn?.Invoke(card);
			}
		}

		public void ReturnCardToDeck(CardData card)
		{
			if (_hand.RemoveCard(card))
			{
				_deck.ReturnCard(card);
			}
			OnCardPlayed?.Invoke(card);
		}

		public bool PlayCard(CardData card)
		{
			if (!_hand.Cards.Contains(card))
			{
				return false;
			}

			/*
			//check if player has enough elixir to play the card
			if (!CanPayElixirCost(card))
			{
				return false;
			}
			*/

			SpendElixir(card.ElixirCost);

			_hand.RemoveCard(card);
			_deck.ReturnCard(card);

			return true;
		}
	}
}