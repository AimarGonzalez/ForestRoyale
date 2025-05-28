using ForestRoyale.Core;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.CardStats;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units.MonoBehaviours;
using ForestRoyale.Gameplay.Units.MonoBehaviours.Components;
using Game.Scripts.Gameplay.Cards.CardStats;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units
{
	// TODO: Rename IDamageable to IHealthBarSource
	public class Unit : IDamageable
	{
		private static Dictionary<string, uint> s_unitCount = new Dictionary<string, uint>();

		[Header("Stats")]
		[ShowInInspector, DisableInEditorMode] private ArenaTeam _team;
		[ShowInInspector, DisableInEditorMode] private float _health;

		[ShowInInspector, ReadOnly] private string _id;
		[ShowInInspector, ReadOnly] private CardData _cardOrigin;
		[ShowInInspector, ReadOnly] private UnitStats _unitStats;
		[ShowInInspector, ReadOnly] private CombatStats _combatStats;

		private UnitRoot _unitRoot;
		private UnitSO _unitSO;

		private MovementComponent _movementComponent;
		private CombatComponent _combatComponent;
		private IDeathComponent _deathComponent;

		[Header("State")]
		[ShowInInspector, ReadOnly]
		private UnitState _state;

		[Header("Target")]
		[ShowInInspector]
		public Unit Target
		{
			get => _combatComponent.Target;
			set => _combatComponent.Target = value;
		}

		[ShowInInspector]
		public bool IsTargetInCombatRange => Application.isPlaying ? _combatComponent.IsTargetInCombatRange : false;

		public string Id => _id;
		public bool CanMove => _movementComponent != null;
		public bool CanFight => _combatComponent != null;

		public bool IsAlive => _state != UnitState.Dying && _state != UnitState.Dead;

		public UnitType UnitType => _unitStats.UnitType;

		public UnitState State
		{
			get => _state;
			set => _state = value;
		}

		public ArenaTeam Team => _team;
		public CardData CardOrigin => _cardOrigin;
		public UnitStats UnitStats => _unitStats;
		public CombatStats CombatStats => _combatStats;
		public UnitRoot UnitRoot => _unitRoot;
		public MovementComponent MovementComponent => _movementComponent;
		public CombatComponent CombatComponent => _combatComponent;
		public IDeathComponent DeathComponent => _deathComponent;


		// IDamageable interface implementation
		public float MaxHealth => _unitStats.HitPoints;
		public float HealthRatio => _health / MaxHealth;
		public float CurrentHealth => _health;

		public bool IsPlayerTeam => _team == ArenaTeam.Player;
		public bool IsForestTeam => _team == ArenaTeam.Forest;

		public Vector3 Position => _unitRoot.Position;

		public bool HasTarget => _combatComponent.HasTarget;

		/*
		public bool TargetIsInCombatRange
		{
			get => _targetIsInCombatRange;
			set => _targetIsInCombatRange = value;
		}
		*/

		public Unit(CardData cardOrigin, UnitRoot root, ArenaTeam team, UnitSO unitSO)
		{
			_cardOrigin = cardOrigin;
			_unitRoot = root;
			_team = team;
			_unitSO = unitSO;
			_movementComponent = root.MovementComponent;
			_combatComponent = root.CombatComponent;
			_deathComponent = root.DeathComponent;
			_unitStats = unitSO.UnitStats;
			_combatStats = unitSO.CombatStats;
			_health = _unitStats.HitPoints;

			_id = _unitRoot.name;
		}

		private string GenerateId()
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

		public void TakeDamage(CombatSystem.HitData hitData)
		{
			_health -= hitData.Damage;

			//TODO: play damage effects
		}

		public void Reset()
		{
			_health = _unitStats.HitPoints;
			State = UnitState.Idle;
			Target = null;

			foreach (IReseteable resetable in _unitRoot.GetComponentsInChildren<IReseteable>())
			{
				resetable.Reset();
			}
		}
	}
}

