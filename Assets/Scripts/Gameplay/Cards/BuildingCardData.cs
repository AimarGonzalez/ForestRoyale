using ForestRoyale.Gameplay.Cards.CardStats;
using UnityEngine;
using UnityEngine.Serialization;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewBuilding", menuName = "ForestRoyale/Building Data", order = 1)]
	public class BuildingCardData : CardData
	{
		public UnitStats UnitStats;
		public CombatStats CombatStats;

#if UNITY_EDITOR
		public static BuildingCardData Build(
			string cardName,
			string description,
			Sprite portrait,
			int elixirCost,
			CardRarity rarity,
			UnitStats unitStats,
			CombatStats combatStats)
		{
			BuildingCardData card = CreateInstance<BuildingCardData>();
			card.SetCardData(cardName, description, portrait, elixirCost, rarity);

			card.UnitStats = unitStats;
			card.CombatStats = combatStats;

			return card;
		}
#endif
	}
}