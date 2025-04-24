using ForestLib.Utils;
using ForestRoyale.Gameplay.Systems;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Game.Scripts.Gameplay.Units.MonoBehaviours.Components
{
	public class AttackComponent : UnitComponent
	{
		[SerializeField]
		private AttackState _state;

		private Timer _timer;

		public AttackState State
		{
			get => _state;
			set => _state = value;
		}

		[Inject]
		private ArenaEvents _arenaEvents;

		public bool IsPlayingAnimation => _state != AttackState.PlayingAttackAnimation;
		

		protected override void Awake()
		{
			base.Awake();
			_timer = new Timer();
			_timer.OnFinished += OnCooldownFinished;
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

	public enum AttackState
	{
		None,
		PlayingAttackAnimation,
		Cooldown
	}
}

