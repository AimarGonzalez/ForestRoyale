using ForestRoyale.Gameplay.Cards.CardStats;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards.ScriptableObjects
{
	[CreateAssetMenu(fileName = "New Unit", menuName = "Forest Royale/Unit")]
	public class UnitSO : ScriptableObject
	{
		[SerializeField]
		private UnitStats _unitStats;

		[SerializeField]
		private CombatStats _combatStats;

		public UnitStats UnitStats => _unitStats;
		public CombatStats CombatStats => _combatStats;

#if UNITY_EDITOR
		public static UnitSO Build(UnitStats unitStats, CombatStats combatStats)
		{
			UnitSO unitSO = CreateInstance<UnitSO>();
			unitSO._unitStats = unitStats;
			unitSO._combatStats = combatStats;

			return unitSO;
		}
#endif
	}
}
