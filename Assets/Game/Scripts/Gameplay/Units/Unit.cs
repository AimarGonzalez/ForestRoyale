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
		[SerializeField] private ArenaTeam _team;
		[SerializeField] private float _health;

		[SerializeField] private string _id;
		[SerializeField] private CardData _cardOrigin;
		[SerializeField] private UnitStats _unitStats;
		[SerializeField] private CombatStats _combatStats;
		[SerializeField] private UnitRoot _unitRoot;
		[SerializeField] private MovementController _movementController;

		[Header("Target")]
		[SerializeField] private Unit _target;
		[SerializeField] private bool _targetIsInCombatRange;

		public string Id => _id;
		public ArenaTeam Team
		{
			get => _team;
			set => _team = value;
		}

		public CardData CardOrigin => _cardOrigin;
		public UnitStats UnitStats => _unitStats;
		public CombatStats CombatStats => _combatStats;
		public UnitRoot UnitRoot => _unitRoot;
		public MovementController MovementController => _movementController;


		// IDamageable interface implementation
		public float MaxHealth => _unitStats.HitPoints;
		public float HealthRatio => _health / MaxHealth;
		public float CurrentHealth
		{
			get => _health;
			set => _health = value;
		}

		public bool IsPlayerTeam => _team == ArenaTeam.Player;
		public bool IsForestTeam => _team == ArenaTeam.Forest;

		public Vector3 Position => _unitRoot.Position;

		public Unit Target
		{
			get => _target;
			set
			{
				_target = value;
			
				// invalidate old target info
				_targetIsInCombatRange = false;
			}
		}
		public bool HasTarget => _target != null;

		public bool TargetIsInCombatRange
		{
			get => _targetIsInCombatRange;
			set => _targetIsInCombatRange = value;
		}

		public Unit(CardData cardOrigin, UnitRoot root, UnitStats unitStats, CombatStats combatStats)
		{
			_cardOrigin = cardOrigin;
			_unitRoot = root;
			_movementController = root.MovementController;
			_id = $"{cardOrigin.CardName}_{++s_unitCount}";
			_unitStats = unitStats;
			_combatStats = combatStats;
			_health = unitStats.HitPoints;
		}
	}
}

