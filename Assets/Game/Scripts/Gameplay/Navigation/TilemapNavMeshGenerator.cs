using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Navigation
{
	[RequireComponent(typeof(TilemapMeshGenerator))]
	public class TilemapNavMeshGenerator : MonoBehaviour
	{
		[Header("NavMesh Settings")]
		public bool rebuildNavMeshOnStart = true;

		public NavMeshSurface navMeshSurface;

		private TilemapMeshGenerator meshGenerator;

		private void Awake()
		{
			EnsureInitialized();
		}

		private void Start()
		{
			if (rebuildNavMeshOnStart)
			{
				BuildNavMesh();
			}
		}

		/// <summary>
		/// Ensures that all required components are initialized.
		/// Can be called from both runtime and editor contexts.
		/// </summary>
		/// <returns>True if successfully initialized, false otherwise</returns>
		public bool EnsureInitialized()
		{
			if (meshGenerator == null)
			{
				meshGenerator = GetComponent<TilemapMeshGenerator>();
			}

			if (meshGenerator == null)
			{
				Debug.LogError("TilemapMeshGenerator component is missing!");
				return false;
			}

			if (navMeshSurface == null)
			{
				navMeshSurface = GetComponent<NavMeshSurface>();
				if (navMeshSurface == null)
				{
					navMeshSurface = gameObject.AddComponent<NavMeshSurface>();

					ConfigureNavMeshSurfaceWithDefaultValues();
				}
			}

			return true;
		}

		public void ConfigureNavMeshSurfaceWithDefaultValues()
		{
			if (navMeshSurface != null)
			{
				// Configure the NavMeshSurface to use the mesh we generate
				navMeshSurface.collectObjects = CollectObjects.All;
				navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
			}
		}

		[ContextMenu("Rebuild NavMesh")]
		public void BuildNavMesh()
		{
			// Ensure initialization when called from editor
			if (!EnsureInitialized())
			{
				Debug.LogError("Failed to initialize required components for NavMesh generation.");
				return;
			}

			if (navMeshSurface == null)
			{
				Debug.LogError("NavMeshSurface component is not assigned!");
				return;
			}

			// Ensure mesh is up to date
			if (meshGenerator.EnsureInitialized())
			{
				meshGenerator.GenerateMeshFromTilemap();

				// Build the NavMesh
				navMeshSurface.BuildNavMesh();
			}
			else
			{
				Debug.LogError("Failed to initialize the TilemapMeshGenerator. Cannot build NavMesh.");
			}
		}

		// Optional: Update the NavMesh when changes are made to the tilemap
		public void UpdateNavMesh()
		{
			if (EnsureInitialized() && meshGenerator.EnsureInitialized())
			{
				meshGenerator.UpdateMesh();
				BuildNavMesh();
			}
		}
	}
}