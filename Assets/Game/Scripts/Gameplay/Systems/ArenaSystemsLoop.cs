using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Systems
{
	public class ArenaSystemsLoop : MonoBehaviour
	{
		[Inject]
		private readonly MovementSystem _movementSystem;

		[Inject]
		private readonly TargetingSystem _targetingSystem;
		
		[Inject]
		private readonly UnitStateMachine _unitStateMachine;

		[Inject]
		private readonly CombatSystem _combatSystem;

		public void Update()
		{
			_targetingSystem.UpdateTargets();
			_movementSystem.UpdateMovement();
			_combatSystem.UpdateCombat();

			_unitStateMachine.UpdateState();
		}
	}
}

