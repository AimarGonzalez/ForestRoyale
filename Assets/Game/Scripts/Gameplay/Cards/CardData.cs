using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards
{
	[Serializable]
	public abstract class CardData : ScriptableObject
	{
		[BoxGroup("Card")]
		[Tooltip("The name of the card")]
		[SerializeField] protected string _cardName;

		[BoxGroup("Card")]
		[Tooltip("Description of the card")]
		[SerializeField][TextArea(3, 6)] protected string _description;

		[BoxGroup("Card")]
		[Tooltip("Card portrait image")]
		[SerializeField] protected Sprite _portrait;

		[BoxGroup("Card")]
		[Tooltip("Cost to deploy this card")]
		[SerializeField] protected int _elixirCost;

		[BoxGroup("Card")]
		[Tooltip("Card rarity")]
		[SerializeField] protected CardRarity _rarity;

		// Public getters for properties
		public string CardName => _cardName;
		public string Description => _description;
		public Sprite Portrait => _portrait;
		public int ElixirCost => _elixirCost;
		public CardRarity Rarity => _rarity;

#if UNITY_EDITOR
		// Protected method to populate card data fields - called by Build methods in derived classes
		protected void SetCardData(
			string cardName,
			string description,
			Sprite portrait,
			int elixirCost,
			CardRarity rarity)
		{
			_cardName = cardName;
			_description = description;
			_portrait = portrait;
			_elixirCost = elixirCost;
			_rarity = rarity;
		}
#endif
	}

	public enum CardRarity
	{
		Common,
		Rare,
		Epic,
		Legendary,
	}
}