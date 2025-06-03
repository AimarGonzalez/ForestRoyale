using ForestLib.ExtensionMethods;
using ForestLib.Utils;
using ForestRoyale.Core.UI;
using NUnit.Framework;
using Sirenix.OdinInspector;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class MovementComponent : UnitComponent, IUnitChangeListener
	{
		[SerializeField]
		[InfoBox("Error: NavMeshAgent not found in root. Consider enabling 'followChildren''", InfoMessageType.Error, nameof(HasNoFollowChildrenNorMovementComponentInTheRoot))]
		[InfoBox("FollowChildren will update the character position to follow the corresponding child components (Ex: NavMeshAgent, Collider or Rigidbody) ")]
		private bool _followChildren = true;

		[SerializeField]
		private bool _isObstacleWhenStatic = false;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[HideInEditorMode, ShowInInspector, ReadOnly]
		[NonSerialized]
		private Rigidbody2D _body;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[HideInEditorMode, ShowInInspector, ReadOnly]
		[NonSerialized]
		private NavMeshAgent _agent;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[HideInEditorMode, ShowInInspector, ReadOnly]
		[NonSerialized]
		private NavMeshObstacle _obstacle;

		public Rigidbody2D Body => _body ??= GetComponentInChildren<Rigidbody2D>(includeInactive: true);
		public NavMeshAgent Agent => _agent ??= GetComponentInChildren<NavMeshAgent>(includeInactive: true);

#if UNITY_EDITOR
		private bool HasAnyMovementComponentInTheRoot => this.HasComponent<NavMeshAgent>() || this.HasComponent<Collider2D>() || this.HasComponent<Rigidbody2D>();
		private bool HasNoFollowChildrenNorMovementComponentInTheRoot => !_followChildren && !HasAnyMovementComponentInTheRoot;
#endif

		private bool _isMoving = false;
		private bool IsMoving
		{
			get => _isMoving;
			set => _isMoving = value;
		}

		private bool IsStopped
		{
			get => !_isMoving;
			set => _isMoving = !value;
		}

		private CancellationTokenSource _disableObstacleTokenSource = new();

		protected override void Awake()
		{
			base.Awake();

			// look for component fallbacks
			_body ??= GetComponentInChildren<Rigidbody2D>(includeInactive: true);
			_agent ??= GetComponentInChildren<NavMeshAgent>(includeInactive: true);
			if (_isObstacleWhenStatic)
			{
				_obstacle ??= GetComponentInChildren<NavMeshObstacle>();
			}

			Subscribe();

			Stop();
		}

		private void Subscribe()
		{
		}

		private void Unsubscribe()
		{
		}

		protected override void OnDestroy()
		{
			Unsubscribe();
		}

		void IUnitChangeListener.OnUnitChanged(Unit oldUnit, Unit newUnit)
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

		public void UpdateMoveDestination()
		{
			if (!_agent.enabled)
			{
				// DEFERRED:The target update is just deferred. It will be updated on next call to Move().
				// This protects a warning that triggers if you set a destination to a disabled agent.
				return;
			}

			_agent.destination = Unit.Target.Position;
		}

		public async void Move()
		{
			if (IsMoving)
			{
				UpdateMoveDestination();
			}
			else
			{
				IsMoving = true;

				// BUGFIX: Wait for the navmesh to remove the hole carved by the obstacle.
				// Otherwise the agent glitches out of the hole instantly and looks terrible.
				if (!await DisableObstacleAndWaitNavmeshToHeal())
				{
					// Abort the Move(). Continuing would cause warning: 
					// "NavMeshAgent and NavMeshObstacle components are active at the same time. This can lead to erroneous behavior."
					return;
				}

				_agent.enabled = true;

				UpdateMoveDestination();
			}
		}

		public void Stop()
		{
			if (IsStopped)
			{
				return;
			}

			IsStopped = true;

			_agent.enabled = false;

			if (_isObstacleWhenStatic && _obstacle)
			{
				_obstacle.enabled = true;

				// Cancel the ongoing call to Move()
				_disableObstacleTokenSource.Cancel();
				_disableObstacleTokenSource = new CancellationTokenSource();
			}
		}

		private async Awaitable<bool> DisableObstacleAndWaitNavmeshToHeal()
		{
			if (!_isObstacleWhenStatic || !_obstacle)
			{
				return true;
			}

			_obstacle.enabled = false;

			try
			{
				// Wait next frame to let the navmesh remove the hole carved by the obstacle.
				await Awaitable.NextFrameAsync(_disableObstacleTokenSource.Token);
			}
			catch (OperationCanceledException)
			{
				// Failed to disable obstacle: we got a call to Stop() while waiting for the navmesh to heal.
				return false;
			}

			return true;
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