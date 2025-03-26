using UnityEngine;
using ForestRoyale.Gameplay.Cards.CardStats;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewSpell", menuName = "ForestRoyale/Spell Data", order = 2)]
	public class SpellCardData : CardData
	{
		[SerializeField]
		private SpellStats _spellEffects = new SpellStats();
		public SpellStats SpellEffects => _spellEffects;

#if UNITY_EDITOR
		public static SpellCardData Build(
			string cardName,
			string description,
			Sprite portrait,
			int elixirCost,
			CardRarity rarity,
			SpellStats spellEffects)
		{
			SpellCardData card = CreateInstance<SpellCardData>();
			card.SetCardData(cardName, description, portrait, elixirCost, rarity);

			card._spellEffects = spellEffects;

			return card;
		}
#endif
	}
}