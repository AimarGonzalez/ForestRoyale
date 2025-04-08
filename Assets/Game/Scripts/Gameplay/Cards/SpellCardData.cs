using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewSpell", menuName = "ForestRoyale/Spell Data", order = 2)]
	public class SpellCardData : CardData
	{
		[SerializeField, InlineEditor]
		private SpellSO _spellSO;
		public SpellSO SpellSO => _spellSO;

#if UNITY_EDITOR
		public static SpellCardData Build(
			string cardName,
			string description,
			Sprite portrait,
			int elixirCost,
			CardRarity rarity,
			SpellSO spellSO)
		{
			SpellCardData card = CreateInstance<SpellCardData>();
			card.SetCardData(cardName, description, portrait, elixirCost, rarity);

			card._spellSO = spellSO;

			return card;
		}
#endif
	}
}