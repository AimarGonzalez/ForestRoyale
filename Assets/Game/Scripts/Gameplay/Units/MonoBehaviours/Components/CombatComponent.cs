using ForestLib.Utils;
using ForestRoyale.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class CombatComponent : UnitComponent
	{
		public enum AttackState
		{
			None,
			PlayingAttackAnimation,
			Cooldown
		}
		
		[ShowInInspector, ReadOnly]
		private AttackState _state;

		[ShowInInspector, ReadOnly]
		private Unit _target;

		private Timer _timer;

		private AttackRangeMonitor _attackRangeMonitor;

		public AttackState State
		{
			get => _state;
			set => _state = value;
		}

		public Unit Target
		{
			get => _target;
			set
			{
				_target = value;
				_attackRangeMonitor.Target = _target;
			}
		}
		
		public bool HasTarget => _target != null;

		public float Cooldown => _timer?.TimeLeft ?? 0f;

		[Inject]
		private ArenaEvents _arenaEvents;

		public bool IsPlayingAnimation => _state == AttackState.PlayingAttackAnimation;

		public bool IsTargetInCombatRange => _attackRangeMonitor.IsTargetInCombatRange;
		

		protected override void Awake()
		{
			base.Awake();
			_timer = new Timer();
			_timer.OnFinished += OnCooldownFinished;

			_attackRangeMonitor = GetComponent<AttackRangeMonitor>();
		}

		private void Update()
		{
			_timer.Elapsed(Time.deltaTime);
		}

		public void Attack()
		{
			switch (_state)
			{
				case AttackState.None:
					_state = AttackState.PlayingAttackAnimation;
					StartCooldown();


					//Play animation
					//Listen for animation event

					OnAnimationHitEvent();
					_state = AttackState.Cooldown;

					break;

				case AttackState.PlayingAttackAnimation:
				case AttackState.Cooldown:
					// do nothing
					break;

				default:
					Debug.LogError($"Unknown attack state: {_state}");
					break;
			}
		}

		private void OnAnimationHitEvent()
		{
			_arenaEvents.TriggerUnitAttacked(Unit, _target);
		}

		private void StartCooldown()
		{
			_timer.Start(Unit.CombatStats.Cooldown);
		}

		private void OnCooldownFinished()
		{
			_state = AttackState.None;
		}
	}
}

