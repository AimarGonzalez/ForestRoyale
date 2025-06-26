using ForestRoyale.Gameplay.Units.MonoBehaviours.Components;
using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Navigation
{
	/// <summary>
	/// Debug component that handles both path visualization and target setting for NavMeshAgents
	/// </summary>
	public class MovementDebugger : MonoBehaviour
	{
		[Header("Path Visualization")]
		[Tooltip("Color of the path line")]
		[SerializeField] private Color pathColor = Color.yellow;

		[Tooltip("Color of the destination marker")]
		[SerializeField] private Color destinationColor = Color.yellow;

		[Tooltip("Size of the destination marker")]
		[SerializeField] private float destinationMarkerSize = 0.5f;

		[Tooltip("Whether to show the complete calculated path")]
		[SerializeField] private bool showCompletePath = true;

		private NavMeshAgent _agent;

		private void Awake()
		{
			_agent = GetComponent<MovementComponent>().Agent;
		}

		private void OnDrawGizmos()
		{
			if (!Application.isPlaying || _agent == null)
			{
				return;
			}

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
			if (_agent != null && _agent.hasPath)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(_agent.destination, 0.5f);
				if (!Application.isPlaying)
				{
					Gizmos.DrawLine(transform.position, _agent.destination);
				}
			}
		}
	}
}