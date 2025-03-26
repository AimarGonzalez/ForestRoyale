using ForestRoyale.Gameplay.Cards.Components;
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
		public void InitializeBuildingCardData(UnitStats unitStats, CombatStats combatStats)
		{
			UnitStats = unitStats;
			CombatStats = combatStats;
		}
#endif
	}
}