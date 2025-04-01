using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards.CardStats
{
	[System.Serializable]
	public class CombatStats
	{
		[BoxGroup("Combat Stats")]
		[Tooltip("Base damage per attack")]
		[SerializeField]
		private float _damage;

		[BoxGroup("Combat Stats")]
		[Tooltip("Time between attacks in seconds")]
		[SerializeField]
		private float _attackSpeed;

		[BoxGroup("Combat Stats")]
		[Tooltip("Range of attack")]
		[SerializeField]
		private float _attackRange;

		[BoxGroup("Combat Stats")]
		[Tooltip("Area damage radius (0 for single target)")]
		[SerializeField]
		private float _areaDamageRadius;

		// Public getters for properties
		public float Damage => _damage;
		public float AttackSpeed => _attackSpeed;
		public float AttackRange => _attackRange;
		public float AreaDamageRadius => _areaDamageRadius;

#if UNITY_EDITOR
		public static CombatStats Build(float damage, float attackSpeed, float attackRange, float areaDamageRadius = 0f)
		{
			CombatStats stats = new CombatStats
			{
				_damage = damage,
				_attackSpeed = attackSpeed,
				_attackRange = attackRange,
				_areaDamageRadius = areaDamageRadius
			};
			return stats;
		}
#endif
	}

	public enum TargetPreference
	{
		Any, // Target any unit in range
		Buildings, // Only target buildings
		Ground, // Prefer ground targets
		Air, // Prefer air targets
		Troops // Only target troops (not buildings)
	}
}