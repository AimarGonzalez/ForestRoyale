using UnityEngine;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewTroop", menuName = "ForestRoyale/Troop Data", order = 1)]
	public class TroopCard : UnitCard
	{
		[Tooltip("Number of units in this card")]
		[SerializeField]
		private int _unitCount = 1;

		[SerializeField]
		private CombatStats _combatStats;

		[SerializeField]
		private TroopStats _troopProperties;

		public CombatStats CombatStats => _combatStats;
		public TroopStats TroopProperties => _troopProperties;
		public int UnitCount => _unitCount;
	}
}