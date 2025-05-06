using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using UnityEditor.AI;
#if UNITY_EDITOR
using Unity.AI.Navigation.Editor;
#endif

// TODO: Consider moving this class to an Editor namespace
namespace ForestRoyale.Gameplay.Navigation
{
	[RequireComponent(typeof(TilemapMeshGenerator))]
	public class TilemapNavMeshGenerator : MonoBehaviour
	{
		[Required]
		[SerializeField]
		[InlineEditor]
		public NavMeshSurface navMeshSurface;

		[ShowInInspector]
		[ReadOnly]
		[BoxGroup("Navigation Status")]
		private bool HasNavMeshSurface => navMeshSurface != null;

		[ShowInInspector]
		[ReadOnly]
		[BoxGroup("Navigation Status")]
		private bool HasNavMeshData => navMeshSurface != null && navMeshSurface.navMeshData != null;

		private TilemapMeshGenerator meshGenerator;

		private void Awake()
		{
			EnsureInitialized();
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

#if UNITY_EDITOR
		[Button("Update NavMesh")]
		[HorizontalGroup("Actions")]
		[PropertyOrder(100)]
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
				Object[] surfaces = {navMeshSurface};
				NavMeshAssetManager.instance.StartBakingSurfaces(surfaces);
			}
			else
			{
				Debug.LogError("Failed to initialize the TilemapMeshGenerator. Cannot build NavMesh.");
			}
		}

		[Button("Open Navigation Window")]
		[HorizontalGroup("Actions")]
		[PropertyOrder(100)]
		private void OpenNavigationWindow()
		{
			NavMeshEditorHelpers.OpenAgentSettings(0);
		}
#endif
	}
}