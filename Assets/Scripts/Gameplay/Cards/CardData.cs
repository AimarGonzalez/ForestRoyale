using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards
{
	public abstract class CardData : ScriptableObject
	{
		[BoxGroup("Card")]
		[Tooltip("The name of the card")]
		[SerializeField] protected string _cardName;

		[BoxGroup("Card")]
		[Tooltip("Description of the card")]
		[SerializeField] [TextArea(3, 6)] protected string _description;

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
	}

	public enum CardRarity
	{
		Common,
		Rare,
		Epic,
		Legendary,
	}
}