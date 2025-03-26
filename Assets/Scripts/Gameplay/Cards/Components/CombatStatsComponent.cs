using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards.Components
{
	[System.Serializable]
	public class CombatStatsComponent
	{
		[BoxGroup("Combat Stats")] [Tooltip("Base health points")] [SerializeField]
		private float _hitPoints;

		[BoxGroup("Combat Stats")] [Tooltip("Base damage per attack")] [SerializeField]
		private float _damage;

		[BoxGroup("Combat Stats")] [Tooltip("Time between attacks in seconds")] [SerializeField]
		private float _attackSpeed;

		[BoxGroup("Combat Stats")] [Tooltip("Range of attack")] [SerializeField]
		private float _attackRange;

		[BoxGroup("Combat Stats")] [Tooltip("Area damage radius (0 for single target)")] [SerializeField]
		private float _areaDamageRadius;

		[BoxGroup("Combat Stats")] [Tooltip("Duration in seconds (for temporary effects)")] [SerializeField]
		private float _duration;

		// Public getters for properties
		public float HitPoints => _hitPoints;
		public float Damage => _damage;
		public float AttackSpeed => _attackSpeed;
		public float AttackRange => _attackRange;
		public float AreaDamageRadius => _areaDamageRadius;
		public float Duration => _duration;
	}
}