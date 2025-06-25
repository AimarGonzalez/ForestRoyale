using ForestRoyale.Gameplay.Combat;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Systems
{
	public class ArenaSystemsLoop : MonoBehaviour
	{
		[Inject]
		private readonly GameState _gameState;

		[Inject]
		private readonly MovementSystem _movementSystem;

		[Inject]
		private readonly TargetingSystem _targetingSystem;

		[Inject]
		private readonly UnitStateMachine _unitStateMachine;

		[Inject]
		private readonly CombatSystem _combatSystem;

		[Inject]
		private readonly ProjectilesSystem _projectilesSystem;

		private void HandleBattlePaused(Battle battle)
		{
			// Movement system has autonomous components we need to pause
			_movementSystem.Pause();
		}

		public void Update()
		{
			if (_gameState.HasActiveBattle)
			{
				_targetingSystem.UpdateTargets();
				_unitStateMachine.UpdateState();

				_movementSystem.UpdateMovement();
				_unitStateMachine.UpdateState();


				_projectilesSystem.UpdateProjectiles();
				_combatSystem.UpdateCombat();
				_unitStateMachine.UpdateState();
			}
			else
			{
				// Movement system has autonomous components we need to pause
				_movementSystem.Pause();
			}
		}
	}
}