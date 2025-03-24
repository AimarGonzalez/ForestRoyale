# Tilemap Mesh Generator and NavMesh System

This set of components allows you to generate a navigable mesh from tilemap tiles (specifically grass tiles) to use for Unity's NavMesh pathfinding system.

## Components Overview

### TilemapMeshGenerator

This component analyzes a tilemap to find grass tiles, then generates a mesh that exactly matches the shape of the area covered by those tiles.

Features:
- Flexible tile filtering based on tile name patterns
- Optimization to combine adjacent tiles into larger rectangles
- Debug visualization to see the generated mesh
- Support for dynamic updates when the tilemap changes

### TilemapNavMeshSurface

This component works with the TilemapMeshGenerator to set up a NavMesh surface that agents can navigate on.

Features:
- Automatically builds a NavMesh from the generated mesh
- Configurable NavMesh settings through Unity's built-in NavMeshSurface
- Methods to update the NavMesh when the tilemap changes

## How to Use

### Basic Setup

1. Add both components to your Tilemap GameObject:
   - Add the `TilemapMeshGenerator` component
   - Add the `TilemapNavMeshSurface` component
   - Add a `MeshFilter` and `MeshRenderer` component (automatically added by the TilemapMeshGenerator)

2. Configure the TilemapMeshGenerator:
   - Set the `Target Tilemap` reference (if not on the same GameObject)
   - Set the `Grass Tile Names` to filter specific tiles (e.g., "terrain-grass-dry")
   - Adjust the `Mesh Height` to match your game's scale
   - Optionally enable `Optimize Mesh` for better performance

3. Configure the TilemapNavMeshSurface:
   - Set `Build NavMesh On Start` if you want the NavMesh to be built automatically
   - The NavMeshSurface reference should be automatically set during initialization

4. Click the "Generate Mesh" button in the inspector to create the mesh
5. Click the "Build NavMesh" button to generate the NavMesh

### Using with NavMesh Agents

1. Make sure you have NavMesh Agents set up in your scene
2. The agents should be able to navigate on the generated NavMesh surface
3. You can use Unity's standard NavMesh API to control agent movement:
   ```csharp
   NavMeshAgent agent = GetComponent<NavMeshAgent>();
   agent.SetDestination(targetPosition);
   ```

### Updating the NavMesh at Runtime

If your tilemap changes during gameplay, you can update the NavMesh:

```csharp
// Get the TilemapNavMeshSurface component
TilemapNavMeshSurface navMeshSurface = GetComponent<TilemapNavMeshSurface>();

// Update the NavMesh
navMeshSurface.UpdateNavMesh();
```

## Tips

- Use the `Show Debug Gizmos` option to visualize the generated mesh
- If you have performance issues, try enabling `Optimize Mesh` to reduce vertex count
- Make sure your NavMesh Agent's radius and height are appropriate for your game scale
- You can adjust the NavMesh bake settings through Unity's Navigation window

## Troubleshooting

- If no mesh is generated, check that your tile names match the `Grass Tile Names` filter
- If agents can't navigate, ensure the NavMesh was built successfully and agents have the correct NavMesh Agent component
- For better control of the NavMesh, use Unity's Navigation window (Window > AI > Navigation)

## Requirements

- Unity 2019.4 or newer
- NavMeshComponents package (for the NavMeshSurface component) 