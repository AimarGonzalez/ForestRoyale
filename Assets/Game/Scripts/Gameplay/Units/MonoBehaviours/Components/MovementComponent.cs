using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class MovementComponent : UnitComponent
	{
		[SerializeField]
		[Required]
		private NavMeshAgent _agent;
		
		[SerializeField]
		[Required]
		private NavMeshObstacle _obstacle;

		[SerializeField]
		[Required]
		private Collider _bodyCollider;


		protected override void Awake()
		{
			base.Awake();
			
			// look for component fallbacks
			_agent ??= GetComponent<NavMeshAgent>();
			_obstacle ??= GetComponent<NavMeshObstacle>();
			_bodyCollider ??= GetComponent<Collider>();

			Stop();
		}

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
			_agent.enabled = true;
			
			Assert.NotNull(Unit.Target);
			_agent.destination = Unit.Target.Position;

			Move();
		}

		public void Move()
		{
			_agent.enabled = true;
			
			_obstacle.enabled = false;
		}

		public void Stop()
		{
			_agent.enabled = false;
			
			_obstacle.enabled = true;
		}
	}
}