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

		public void Update()
		{
			_targetingSystem.UpdateTargets();
			_movementSystem.UpdateMovement();
		}
	}
}

