using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class MovementComponent : UnitComponent
	{
		[SerializeField]
		private bool _followChildren = true;

		[SerializeField]
		[Required]
		private Collider2D _body;

		[SerializeField]
		[Required]
		private NavMeshAgent _agent;

		[SerializeField]
		[Required]
		private NavMeshObstacle _obstacle;

		public Collider2D Body => _body;
		public NavMeshAgent Agent => _agent;

		protected override void Awake()
		{
			base.Awake();

			// look for component fallbacks
			_agent ??= GetComponentInChildren<NavMeshAgent>();
			_obstacle ??= GetComponentInChildren<NavMeshObstacle>();

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

		public async Awaitable MoveToTarget()
		{
			await Move();

			Assert.NotNull(Unit.Target);
			_agent.destination = Unit.Target.Position;
		}

		public async Awaitable Move()
		{
			if (_obstacle.enabled)
			{
				_obstacle.enabled = false;

				// BUGFIX: Wait next frame to let the navmesh remove the hole carved by the obstacle.
				// Otherwise the agent glitches out of the hole instantly and looks terrible.
				await Awaitable.NextFrameAsync();
			}

			_agent.enabled = true;
		}

		public void Stop()
		{
			_agent.enabled = false;

			_obstacle.enabled = true;
		}

		public void LateUpdate()
		{
			FollowComponent(_agent);
			FollowComponent(_body);
		}

		private void FollowComponent(Component component)
		{
			if (!_followChildren)
			{
				return;
			}

			// move to bodies position
			transform.position = component.transform.position;

			// clear body local position
			component.transform.localPosition = Vector3.zero;
		}
	}
}