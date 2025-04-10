using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.CardStats;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using ForestRoyale.Gameplay.Units.MonoBehaviors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units
{
	[Serializable]
	public class Unit : IDamageable
	{
		private static Dictionary<string, uint> s_unitCount = new Dictionary<string, uint>();

		[Header("Stats")]
		[SerializeField] private ArenaTeam _team;
		[SerializeField] private float _health;

		[SerializeField] private string _id;
		[SerializeField] private CardData _cardOrigin;
		[SerializeField] private UnitStats _unitStats;
		[SerializeField] private CombatStats _combatStats;
		[SerializeField] private UnitRoot _unitRoot;
		[SerializeField] private UnitSO _unitSO;
		[SerializeField] private MovementController _movementController;

		[Header("State")]
		[SerializeField] private UnitState _state;

		[Header("Target")]
		[SerializeField] private Unit _target;
		[SerializeField] private bool _targetIsInCombatRange;

		public string Id => _id;
		public bool CanMove => _movementController != null;

		public UnitState State
		{
			get => _state;
			set => _state = value;
		}

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
				if (_target != value)
				{
					_target = value;

					// invalidate old target info
					_targetIsInCombatRange = false;
				}
			}
		}
		public bool HasTarget => _target != null;

		public bool TargetIsInCombatRange
		{
			get => _targetIsInCombatRange;
			set => _targetIsInCombatRange = value;
		}

		public Unit(CardData cardOrigin, UnitRoot root, ArenaTeam team, UnitSO unitSO)
		{
			_cardOrigin = cardOrigin;
			_unitRoot = root;
			_team = team;
			_unitSO = unitSO;
			_movementController = root.MovementController;
			_unitStats = unitSO.UnitStats;
			_combatStats = unitSO.CombatStats;
			_health = _unitStats.HitPoints;

			_id = GenerateId();
		}

		public string GenerateId()
		{
			string name = _cardOrigin?.CardName ?? _unitSO.UnitStats.Name ?? _unitSO.name;
			if (!s_unitCount.ContainsKey(name))
			{
				s_unitCount[name] = 0;
			}

			return $"{name}_{++s_unitCount[name]}";
		}

		public string ToLogString()
		{
			return $"{_unitStats.Name} ({_id}/{_team}/{_state})";
		}
	}
}

