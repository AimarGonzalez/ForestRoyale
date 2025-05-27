using Game.Scripts.Gameplay.Cards.CardStats;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace ForestRoyale.Gameplay.Cards.CardStats
{
	[System.Serializable]
	public class CombatStats
	{
		[Tooltip("Base damage per attack")]
		[SerializeField]
		private float _damage;

		[Tooltip("Time between attacks in seconds")]
		[SerializeField]
		private float _attackSpeed;

		[Tooltip("Range of attack")]
		[SerializeField]
		private float _attackRange;

		[Tooltip("Area damage radius (0 for single target)")]
		[SerializeField]
		private float _areaDamageRadius;

		[Tooltip("Range of sight")]
		[SerializeField]
		private float _sightRange;

		[Tooltip("Target preference")]
		[SerializeField]
		private List<UnitType> _targetPreference;

		// Public getters for properties
		public float Damage => _damage;
		public float AttackSpeed => _attackSpeed;
		public float AttackRange => _attackRange;
		public float AreaDamageRadius => _areaDamageRadius;
		public float SightRange => _sightRange;
		public List<UnitType> TargetPreference => _targetPreference;
		public float Cooldown => _attackSpeed;
		


#if UNITY_EDITOR
		public static CombatStats Build(
			float damage,
			float attackSpeed,
			float attackRange,
			float areaDamageRadius,
			float sightRange,
			List<UnitType> targetPreference)
		{
			CombatStats stats = new CombatStats
			{
				_damage = damage,
				_attackSpeed = attackSpeed,
				_attackRange = attackRange,
				_areaDamageRadius = areaDamageRadius,
				_sightRange = sightRange,
				_targetPreference = targetPreference
			};
			return stats;
		}
#endif
	}
}