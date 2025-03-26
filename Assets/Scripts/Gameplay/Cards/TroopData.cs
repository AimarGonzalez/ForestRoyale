using UnityEngine;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewTroop", menuName = "ForestRoyale/Troop Data", order = 1)]
	public class TroopData : CardData
	{
		[Tooltip("Number of units in this card")]
		[SerializeField]
		private int _unitCount = 1;

		[SerializeField]
		private CombatStatsComponent _combatStats = new CombatStatsComponent();

		[SerializeField]
		private TroopPropertiesComponent _troopProperties = new TroopPropertiesComponent();

		public CombatStatsComponent CombatStats => _combatStats;
		public TroopPropertiesComponent TroopProperties => _troopProperties;
		public int UnitCount => _unitCount;
	}
}