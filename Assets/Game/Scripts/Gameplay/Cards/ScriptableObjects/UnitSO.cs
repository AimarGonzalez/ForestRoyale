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

		[SerializeField]
		private UnitPrefabs _prefabs;

		public UnitStats UnitStats => _unitStats;
		public CombatStats CombatStats => _combatStats;
		public UnitPrefabs Prefabs => _prefabs;
#if UNITY_EDITOR
		public static UnitSO Build(UnitStats unitStats, CombatStats combatStats, UnitPrefabs unitPrefabs)
		{
			UnitSO unitSO = CreateInstance<UnitSO>();
			unitSO._unitStats = unitStats;
			unitSO._combatStats = combatStats;
			unitSO._prefabs = unitPrefabs;

			return unitSO;
		}
#endif
	}
}
