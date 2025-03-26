using UnityEngine;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewTroop", menuName = "ForestRoyale/Troop Data", order = 1)]
	public class TroopCardData : CardData
	{
		[Tooltip("Number of units in this card")]
		[SerializeField]
		private int _unitCount = 1;

		[SerializeField]
		private TroopStats _troopProperties;

		[SerializeField]
		private CombatStats _combatStats;

		public CombatStats CombatStats => _combatStats;
		public TroopStats TroopProperties => _troopProperties;
		public int UnitCount => _unitCount;

#if UNITY_EDITOR
		public void InitializeTroopCardData(
			int unitCount,
			TroopStats troopProperties,
			CombatStats combatStats
			)
		{
			_unitCount = unitCount;
			_combatStats = combatStats;
			_troopProperties = troopProperties;
		}
#endif
	}
}