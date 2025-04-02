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

		[Header("Stats")]
		public ArenaTeam Team;
		public float Health;


		public string Id;
		public CardData CardOrigin;
		public UnitStats UnitStats;
		public CombatStats CombatStats;
		public UnitRoot UnitRoot;
		public MovementController MovementController;

		[Header("Target")]
		public Unit Target;
		public bool TargetIsInCombatRange;




		// IDamageable interface implementation
		public float MaxHealth => UnitStats.HitPoints;
		public float CurrentHealth => Health;

		public float HealthRatio => Health / MaxHealth;

		public bool IsPlayerTeam => Team == ArenaTeam.Player;
		public bool IsForestTeam => Team == ArenaTeam.Forest;
		public bool HasTarget => Target != null;

		public Vector3 Position => UnitRoot.Position;

		public Unit(CardData _cardOrigin, UnitRoot root, UnitStats unitStats, CombatStats combatStats)
		{
			CardOrigin = _cardOrigin;
			UnitRoot = root;
			MovementController = root.MovementController;
			Id = $"{CardOrigin.CardName}_{++s_unitCount}";
			UnitStats = unitStats;
			CombatStats = combatStats;
			Health = UnitStats.HitPoints;
		}
	}
}

