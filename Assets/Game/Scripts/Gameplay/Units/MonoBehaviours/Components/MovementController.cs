using ForestRoyale.Game.Scripts.Gameplay.Units.MonoBehaviours;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Units.MonoBehaviors
{
	public class MovementController : UnitComponent
	{
		[SerializeField]
		[Required]
		private NavMeshAgent _agent;

		[SerializeField]
		[Required]
		private Collider _bodyCollider;


		protected override void OnUnitChanged()
		{
			if (_agent == null)
			{
				Debug.LogError("Missing NavMeshAgent component.");
				return;
			}

			if (Unit != null)
			{
				// Update agent with unit stats
				_agent.speed = Unit.UnitStats.MovementSpeed;
			}
			else
			{
				// Disable movement
				_agent.speed = 0;
			}

		}

		public void MoveToTarget()
		{
			Assert.NotNull(Unit.Target);
			_agent.destination = Unit.Target.Position;

			Move();
		}

		public void Move()
		{
			_agent.isStopped = false;
		}

		public void Stop()
		{
			_agent.isStopped = true;
		}
	}
}