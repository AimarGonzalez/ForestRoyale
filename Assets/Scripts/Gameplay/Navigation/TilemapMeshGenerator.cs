using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Raven.Attributes;

namespace Raven.Gameplay.Navigation
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

        [BoxGroup("Debug")] 
        [Tooltip("Show debug gizmos in the scene view")]
        [SerializeField] private bool _showDebugGizmos = false;
        
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private Mesh _generatedMesh;

        // Cache for optimization
        private bool[,] _grassGrid;
        private BoundsInt _lastBounds;
        
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
        
        [Button("Regenerate Mesh")]
        public void GenerateMeshFromTilemap()
        {
            if (!EnsureInitialized())
            {
                return;
            }

            _lastBounds = GetCombinedTilemapBounds();

            _grassGrid = PopulateGrassGrid(_lastBounds);

            GenerateSimpleMesh(_grassGrid, _lastBounds);
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
                if (tilemap == null) continue;

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
            if(tile == null)
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
                if (tilemap == null) continue;

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
                        continue;
                    
                    // Convert from grid coordinates to world coordinates
                    // Note: We're using X and Z for the horizontal plane, Y for height
                    Vector3 worldBottomLeft = _targetTilemaps[0].CellToWorld(new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0));
                    Vector3 worldBottomRight = _targetTilemaps[0].CellToWorld(new Vector3Int(x + 1 + bounds.xMin, y + bounds.yMin, 0));
                    Vector3 worldTopLeft = _targetTilemaps[0].CellToWorld(new Vector3Int(x + bounds.xMin, y + 1 + bounds.yMin, 0));
                    Vector3 worldTopRight = _targetTilemaps[0].CellToWorld(new Vector3Int(x + 1 + bounds.xMin, y + 1 + bounds.yMin, 0));
                    
                    // Apply Y offset for z-fighting prevention
                    worldBottomLeft.y = _yOffset;
                    worldBottomRight.y = _yOffset;
                    worldTopLeft.y = _yOffset;
                    worldTopRight.y = _yOffset;
                    
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
        
        // Optional: Update the mesh at runtime if the tilemap changes
        public void UpdateMesh()
        {
            if (_targetTilemaps != null)
            {
                GenerateMeshFromTilemap();
            }
        }
        
        private void OnDrawGizmos()
        {
            if (!_showDebugGizmos || _targetTilemaps == null || _generatedMesh == null)
                return;
            
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
        }
    } 
}