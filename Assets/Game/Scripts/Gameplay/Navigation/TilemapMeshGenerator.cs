using ForestLib.ExtensionMethods;
using ForestLib.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

// TODO: Consider moving this class to an Editor namespace
namespace ForestRoyale.Gameplay.Navigation
{
	[RequireComponent(typeof(MeshFilter))]
	public class TilemapMeshGenerator : MonoBehaviour
	{
		[BoxGroup("Tilemap Data")]
		[Tooltip("Tilemaps to generate the mesh from")]
		[SerializeField] private Tilemap[] _targetTilemaps;

		[BoxGroup("Tilemap Data")]
		[Tooltip("Tiles to include in the mesh generation")]
		[SerializeField] private TileBase[] _grassTiles;

		[BoxGroup("Mesh Settings")]
		[Tooltip("Offset from the tilemap to prevent z-fighting")]
		[SerializeField] private float _yOffset = 0.01f;

		[BoxGroup("Mesh Settings")]
		[Tooltip("Whether to update the mesh when the tilemap changes")]
		[SerializeField] private bool _updateDynamically = true;

		[BoxGroup("Mesh Settings")]
		[Tooltip("Whether to generate a collider for the mesh")]
		[SerializeField] private bool _generateCollider = false;

		[BoxGroup("Mesh Settings")]
		[Tooltip("Path where the generated mesh will be saved as an asset (relative to Assets folder)")]
		[HorizontalGroup("Mesh Settings/Path")]
		[Sirenix.OdinInspector.FilePath(Extensions = ".asset")]
		[Required]
		[InlineButton(nameof(AutoFillMeshPath), SdfIconType.Magic, "")]
		[ValidateInput(nameof(IsValidateMeshPath), "Mesh path is invalid")]
		[SerializeField] private string _meshAssetPath = "";

		private void AutoFillMeshPath()
		{
			_meshAssetPath = GeneratePathFromCurrentScene();
		}

		private bool IsValidateMeshPath(string path)
		{
			return !string.IsNullOrEmpty(path) && path.EndsWith(".asset") && Regex.IsMatch(path, @"^Assets/.*/[\w\- ]+.asset$");
		}

		[BoxGroup("Debug")]
		[Tooltip("Show debug gizmos in the scene view")]
		[SerializeField] private bool _showDebugGizmos = false;

		private MeshFilter _meshFilter;
		private MeshCollider _meshCollider;
		private Mesh _generatedMesh;

		// Cache for optimization
		private bool[,] _grassGrid;
		private BoundsInt _lastBounds;
		private Quaternion[] _cachedRotations;

		private void Awake()
		{
			EnsureInitialized();
			GenerateMeshFromTilemap();
		}

		/// <summary>
		/// Ensures that the required components are initialized.
		/// Can be called from both Awake and from editor context.
		/// </summary>
		/// <returns>True if successfully initialized, false otherwise</returns>
		public bool EnsureInitialized()
		{
			// Get or add required components
			_meshFilter = GetComponent<MeshFilter>();
			if (_meshFilter == null)
			{
				_meshFilter = gameObject.AddComponent<MeshFilter>();
			}

			// Initialize collider if needed
			if (_generateCollider)
			{
				_meshCollider = GetComponent<MeshCollider>();
				if (_meshCollider == null)
				{
					_meshCollider = gameObject.AddComponent<MeshCollider>();
				}
			}

			// Try to find tilemaps if not specified
			if (_targetTilemaps == null || _targetTilemaps.Length == 0)
			{
				_targetTilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
				if (_targetTilemaps.Length == 0)
				{
					Debug.LogError("No Tilemap found in the scene. Please assign a Tilemap.");
					return false;
				}
			}

			return true;
		}

		private void OnEnable()
		{
			if (_updateDynamically && _targetTilemaps != null)
			{
				// Setup events to update when tiles change (requires custom implementation with events on your Tilemap manager)
			}
		}

		private void OnDisable()
		{
			if (_updateDynamically && _targetTilemaps != null)
			{
				// Remove event listeners
			}
		}

		private bool MeshAssetPathIsValid => IsValidateMeshPath(_meshAssetPath);

		private void CacheAndUnrotateTilemaps()
		{
			_cachedRotations = new Quaternion[_targetTilemaps.Length];
			for (int i = 0; i < _targetTilemaps.Length; i++)
			{
				if (_targetTilemaps[i] != null)
				{
					_cachedRotations[i] = _targetTilemaps[i].transform.rotation;
					_targetTilemaps[i].transform.rotation = Quaternion.identity;
				}
			}
		}

		private void RestoreTilemapRotations()
		{
			if (_cachedRotations == null)
			{
				return;
			}

			for (int i = 0; i < _targetTilemaps.Length; i++)
			{
				if (_targetTilemaps[i] != null)
				{
					_targetTilemaps[i].transform.rotation = _cachedRotations[i];
				}
			}
			_cachedRotations = null;
		}

		[Button("Update Mesh")]
		[EnableIf(nameof(MeshAssetPathIsValid))]
		public void GenerateMeshFromTilemap()
		{
			if (!EnsureInitialized())
			{
				return;
			}

			try
			{
				CacheAndUnrotateTilemaps();
				_lastBounds = GetCombinedTilemapBounds();
				_grassGrid = PopulateGrassGrid(_lastBounds);
				GenerateSimpleMesh(_grassGrid, _lastBounds);
			}
			finally
			{
				RestoreTilemapRotations();
			}
		}

		/// <summary>
		/// Calculates the combined bounds of all target tilemaps.
		/// </summary>
		/// <returns>The combined bounds of all tilemaps</returns>
		private BoundsInt GetCombinedTilemapBounds()
		{
			BoundsInt combinedBounds = new BoundsInt();
			bool isFirst = true;

			foreach (Tilemap tilemap in _targetTilemaps)
			{
				if (tilemap == null)
				{
					continue;
				}

				BoundsInt bounds = tilemap.cellBounds;
				if (isFirst)
				{
					combinedBounds = bounds;
					isFirst = false;
				}
				else
				{
					combinedBounds.xMin = Mathf.Min(combinedBounds.xMin, bounds.xMin);
					combinedBounds.yMin = Mathf.Min(combinedBounds.yMin, bounds.yMin);
					combinedBounds.zMin = Mathf.Min(combinedBounds.zMin, bounds.zMin);
					combinedBounds.xMax = Mathf.Max(combinedBounds.xMax, bounds.xMax);
					combinedBounds.yMax = Mathf.Max(combinedBounds.yMax, bounds.yMax);
					combinedBounds.zMax = Mathf.Max(combinedBounds.zMax, bounds.zMax);
				}
			}

			return combinedBounds;
		}

		private bool IsTileGrass(TileBase tile)
		{
			if (tile == null)
			{
				return false;
			}

			if (_grassTiles == null || _grassTiles.Length == 0)
			{
				// If no tiles are specified, include all tiles
				return true;
			}

			foreach (TileBase grassTile in _grassTiles)
			{
				if (tile == grassTile)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Creates and populates a grid indicating which cells contain grass tiles.
		/// </summary>
		/// <param name="bounds">The bounds of the tilemap area to check</param>
		/// <returns>A 2D boolean array where true indicates a grass tile</returns>
		private bool[,] PopulateGrassGrid(BoundsInt bounds)
		{
			// Create a grid to store grass tiles
			int width = bounds.size.x;
			int height = bounds.size.y;
			bool[,] grassGrid = new bool[width, height];

			// Check each tilemap for grass tiles
			foreach (Tilemap tilemap in _targetTilemaps)
			{
				if (tilemap == null)
				{
					continue;
				}

				for (int x = bounds.xMin; x < bounds.xMax; x++)
				{
					for (int y = bounds.yMin; y < bounds.yMax; y++)
					{
						Vector3Int pos = new Vector3Int(x, y, 0);
						TileBase tile = tilemap.GetTile(pos);

						if (IsTileGrass(tile))
						{
							// Convert to local grid coordinates
							int gridX = x - bounds.xMin;
							int gridY = y - bounds.yMin;
							grassGrid[gridX, gridY] = true;
						}
					}
				}
			}

			return grassGrid;
		}

		private void GenerateSimpleMesh(bool[,] grassGrid, BoundsInt bounds)
		{

#if UNITY_EDITOR
			_generatedMesh = null;
			
			// Load asset from path
			if(IsValidateMeshPath(_meshAssetPath))
			{
				_generatedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(_meshAssetPath);
			}
#endif

			// Create or clear the mesh
			if (_generatedMesh == null)
			{
				_generatedMesh = new Mesh();
				_generatedMesh.name = "GrassMesh";
			}
			else
			{
				_generatedMesh.Clear();
			}

			int width = grassGrid.GetLength(0);
			int height = grassGrid.GetLength(1);

			// Used to create vertices and triangles for the mesh
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			List<Vector2> uvs = new List<Vector2>();

			// Create quads for each grass cell
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (!grassGrid[x, y])
					{
						continue;
					}

					// Convert from grid coordinates to world coordinates and project onto XZ plane
					Vector3 worldBottomLeft = _targetTilemaps[0].CellToWorld(new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0));
					Vector3 worldBottomRight = _targetTilemaps[0].CellToWorld(new Vector3Int(x + 1 + bounds.xMin, y + bounds.yMin, 0));
					Vector3 worldTopLeft = _targetTilemaps[0].CellToWorld(new Vector3Int(x + bounds.xMin, y + 1 + bounds.yMin, 0));
					Vector3 worldTopRight = _targetTilemaps[0].CellToWorld(new Vector3Int(x + 1 + bounds.xMin, y + 1 + bounds.yMin, 0));

					// Project vertices onto XZ plane (tilemaps are already unrotated)
					worldBottomLeft = new Vector3(worldBottomLeft.x, _yOffset, worldBottomLeft.z);
					worldBottomRight = new Vector3(worldBottomRight.x, _yOffset, worldBottomRight.z);
					worldTopLeft = new Vector3(worldTopLeft.x, _yOffset, worldTopLeft.z);
					worldTopRight = new Vector3(worldTopRight.x, _yOffset, worldTopRight.z);

					// For vertical tilemaps, we need to use the Y coordinate as Z
					if (Mathf.Approximately(Mathf.Abs(_targetTilemaps[0].transform.up.y), 0f))
					{
						worldBottomLeft = new Vector3(worldBottomLeft.x, _yOffset, worldBottomLeft.y);
						worldBottomRight = new Vector3(worldBottomRight.x, _yOffset, worldBottomRight.y);
						worldTopLeft = new Vector3(worldTopLeft.x, _yOffset, worldTopLeft.y);
						worldTopRight = new Vector3(worldTopRight.x, _yOffset, worldTopRight.y);
					}

					// Add vertices for this cell
					int baseIndex = vertices.Count;

					vertices.Add(worldBottomLeft);
					vertices.Add(worldBottomRight);
					vertices.Add(worldTopRight);
					vertices.Add(worldTopLeft);

					// Add UVs
					uvs.Add(new Vector2(0, 0));
					uvs.Add(new Vector2(1, 0));
					uvs.Add(new Vector2(1, 1));
					uvs.Add(new Vector2(0, 1));

					// Add triangles (counter-clockwise winding order for upward-facing mesh)
					triangles.Add(baseIndex);
					triangles.Add(baseIndex + 3);
					triangles.Add(baseIndex + 2);

					triangles.Add(baseIndex);
					triangles.Add(baseIndex + 2);
					triangles.Add(baseIndex + 1);
				}
			}

			// Assign vertices, triangles, and UVs to the mesh
			_generatedMesh.vertices = vertices.ToArray();
			_generatedMesh.triangles = triangles.ToArray();
			_generatedMesh.uv = uvs.ToArray();

			// Recalculate normals and bounds
			_generatedMesh.RecalculateNormals();
			_generatedMesh.RecalculateBounds();

#if UNITY_EDITOR
			// create asset in new path
			if (IsValidateMeshPath(_meshAssetPath))
			{
				_generatedMesh = AssetUtils.CreateOrReplaceAsset(_generatedMesh, _meshAssetPath);
			}
			else{
				Debug.LogError("Mesh path is invalid - " + _meshAssetPath);
			}
#endif

			// Assign the mesh to the appropriate components
			_meshFilter.mesh = _generatedMesh;

			if (_generateCollider && _meshCollider != null)
			{
				_meshCollider.sharedMesh = _generatedMesh;
				_meshCollider.convex = false;
			}
		}

		// Optional: Get the generated mesh for external use (e.g., for NavMesh generation)
		public Mesh GetGeneratedMesh()
		{
			if (_generatedMesh == null)
			{
				GenerateMeshFromTilemap();
			}

			return _generatedMesh;
		}

		private void OnDrawGizmos()
		{
			if (!_showDebugGizmos || _targetTilemaps == null || _generatedMesh == null)
			{
				return;
			}

			Gizmos.color = Color.green;

			// Draw the outline of the mesh
			if (_meshFilter != null && _meshFilter.sharedMesh != null)
			{
				Mesh mesh = _meshFilter.sharedMesh;
				Vector3[] vertices = mesh.vertices;
				int[] triangles = mesh.triangles;

				for (int i = 0; i < triangles.Length; i += 3)
				{
					Vector3 v1 = transform.TransformPoint(vertices[triangles[i]]);
					Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 1]]);
					Vector3 v3 = transform.TransformPoint(vertices[triangles[i + 2]]);

					Gizmos.DrawLine(v1, v2);
					Gizmos.DrawLine(v2, v3);
					Gizmos.DrawLine(v3, v1);
				}
			}

			/*
			// Optional: draw a wireframe of the grass grid if available
			if (_grassGrid != null && _lastBounds != null)
			{
			    Gizmos.color = Color.yellow;
			    for (int x = 0; x < _grassGrid.GetLength(0); x++)
			    {
			        for (int y = 0; y < _grassGrid.GetLength(1); y++)
			        {
			            if (_grassGrid[x, y])
			            {
			                Vector3 worldPos = _targetTilemaps[0].CellToWorld(new Vector3Int(x + _lastBounds.xMin, y + _lastBounds.yMin, 0));
			                worldPos.y = _yOffset; // Position at the mesh height
			                Gizmos.DrawWireCube(worldPos + Vector3.one * 0.5f, Vector3.one * 0.9f);
			            }
			        }
			    }
			}
			*/
		}

		private string GeneratePathFromCurrentScene()
		{
			string sceneName = SceneManager.GetActiveScene().name;
			return $"Assets/Game/Scenes/{sceneName}/{sceneName}_Mesh.asset";
		}
	}
}