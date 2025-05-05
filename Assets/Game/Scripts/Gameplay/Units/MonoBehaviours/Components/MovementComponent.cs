using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class MovementComponent : UnitComponent
	{
		[SerializeField]
		private bool _followBody;
		
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
		
		protected override void Awake()
		{
			base.Awake();
			
			// look for component fallbacks
			_agent ??= GetComponent<NavMeshAgent>();
			_obstacle ??= GetComponent<NavMeshObstacle>();

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
			Move();
			
			Assert.NotNull(Unit.Target);
			_agent.destination = Unit.Target.Position;
		}

		public void Move()
		{
			_obstacle.enabled = false;
			_agent.enabled = true;
		}

		public void Stop()
		{
			_agent.enabled = false;
			
			_obstacle.enabled = true;
		}

		public void Update()
		{
			FollowBody();
		}

		private void FollowBody()
		{
			if (!_followBody)
			{
				return;
			}
			// move to bodies position
			transform.position = _body.transform.position;
			
			// clear body local position
			_body.transform.localPosition = Vector3.zero;
		}
	}
}