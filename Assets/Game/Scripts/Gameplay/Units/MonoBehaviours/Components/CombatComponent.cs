using ForestLib.Utils;
using ForestRoyale.Gameplay.Systems;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Game.Scripts.Gameplay.Units.MonoBehaviours.Components
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

		private Timer _timer;

		public AttackState State
		{
			get => _state;
			set => _state = value;
		}

		public float Cooldown => _timer.TimeLeft;

		[Inject]
		private ArenaEvents _arenaEvents;

		public bool IsPlayingAnimation => _state != AttackState.PlayingAttackAnimation;
		

		protected override void Awake()
		{
			base.Awake();
			_timer = new Timer();
			_timer.OnFinished += OnCooldownFinished;
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
			_arenaEvents.TriggerUnitAttacked(Unit, Unit.Target);
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

