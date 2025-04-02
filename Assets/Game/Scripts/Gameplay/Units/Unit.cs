using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.CardStats;
using ForestRoyale.Gameplay.Units.MonoBehaviors;
using System;
using UnityEngine;


namespace ForestRoyale.Gameplay.Units
{
	[Serializable]
	public class Unit : IDamageable
	{
		private static uint s_unitCount = 0;
		
		[SerializeField] private Team _team;
		[SerializeField] private float _health;

		[Header("Target")]
		[SerializeField] public Unit Target;
		[SerializeField] public bool TargetIsInCombatRange;

		public bool IsPlayerTeam => _team == Team.Player;
		public bool IsForestTeam => _team == Team.Forest;
		public bool HasTarget => Target != null;

		public CardData CardOrigin;
		public UnitStats UnitStats;
		public CombatStats CombatStats;
		public string Id;
		public UnitRoot UnitRoot;
		public MovementController MovementController;


		// IDamageable interface implementation
		public float MaxHealth => UnitStats.HitPoints;
		public float Health => _health;

		public float HealthRatio => Health / MaxHealth;
		
		public Vector3 Position => UnitRoot.Position;

		public Unit(CardData _cardOrigin, UnitRoot root, UnitStats unitStats, CombatStats combatStats)
		{
			CardOrigin = _cardOrigin;
			UnitRoot = root;
			MovementController = root.MovementController;
			Id = $"{CardOrigin.CardName}_{++s_unitCount}";
			UnitStats = unitStats;
			CombatStats = combatStats;
			_health = UnitStats.HitPoints;
		}
	}

	internal enum Team
	{
		Player,
		Forest
	}
}

