using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Navigation
{
	/// <summary>
	/// Debug component that handles both path visualization and target setting for NavMeshAgents
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class MovementDebugger : MonoBehaviour
	{
		[Header("Target Settings")]
		[Tooltip("The target GameObject to follow")]
		[SerializeField] private Transform target;

		[Header("Path Visualization")]
		[Tooltip("Color of the path line")]
		[SerializeField] private Color pathColor = Color.yellow;

		[Tooltip("Color of the destination marker")]
		[SerializeField] private Color destinationColor = Color.red;

		[Tooltip("Size of the destination marker")]
		[SerializeField] private float destinationMarkerSize = 0.5f;

		[Tooltip("Whether to show the complete calculated path")]
		[SerializeField] private bool showCompletePath = true;

		private NavMeshAgent _agent;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
		}

		private void FixedUpdate()
		{
			if (target == null) return;

			UpdatePath();
		}

		private void UpdatePath()
		{
			if (_agent.isOnNavMesh)
			{
				_agent.SetDestination(target.position);
			}
		}

		// Public method to set target at runtime
		public void SetTarget(Transform newTarget)
		{
			target = newTarget;
			if (target != null)
			{
				UpdatePath();
			}
		}

		private void OnDrawGizmos()
		{
			if (!Application.isPlaying || _agent == null) return;

			// Draw line to current destination
			if (_agent.hasPath)
			{
				// Set colors for drawing
				Gizmos.color = pathColor;

				if (showCompletePath)
				{
					// Draw the complete calculated path
					Vector3[] corners = _agent.path.corners;
					for (int i = 0; i < corners.Length - 1; i++)
					{
						Gizmos.DrawLine(corners[i], corners[i + 1]);
					}
				}
				else
				{
					// Draw direct line to destination
					Gizmos.DrawLine(transform.position, _agent.destination);
				}

				// Draw destination marker
				Gizmos.color = destinationColor;
				Gizmos.DrawWireSphere(_agent.destination, destinationMarkerSize);
			}
		}

		// Draw additional debug info when object is selected
		private void OnDrawGizmosSelected()
		{
			if (target != null)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(target.position, 0.5f);
				if (!Application.isPlaying)
				{
					Gizmos.DrawLine(transform.position, target.position);
				}
			}
		}
	}
}