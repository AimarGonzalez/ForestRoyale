using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapMeshGenerator))]
public class TilemapMeshGeneratorEditor : Editor
{
    private SerializedProperty targetTilemapsProp;
    private SerializedProperty grassTilesProp;
    private SerializedProperty yOffsetProp;
    private SerializedProperty updateDynamicallyProp;
    private SerializedProperty optimizeMeshProp;
    private SerializedProperty showDebugGizmosProp;
    private SerializedProperty generateColliderProp;

    private void OnEnable()
    {
        // Ensure serializedObject is valid before using it
        if (serializedObject != null)
        {
            targetTilemapsProp = serializedObject.FindProperty("_targetTilemaps");
            grassTilesProp = serializedObject.FindProperty("_grassTiles");
            yOffsetProp = serializedObject.FindProperty("_yOffset");
            updateDynamicallyProp = serializedObject.FindProperty("_updateDynamically");
            optimizeMeshProp = serializedObject.FindProperty("_optimizeMesh");
            showDebugGizmosProp = serializedObject.FindProperty("_showDebugGizmos");
            generateColliderProp = serializedObject.FindProperty("_generateCollider");
        }
    }

    public override void OnInspectorGUI()
    {
        // Ensure serializedObject is valid before using it
        if (serializedObject == null)
            return;
            
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tilemap Mesh Generator", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This component generates a mesh based on grass tiles in the tilemaps.", MessageType.Info);
        EditorGUILayout.Space();

        // Ensure all properties are valid before using them
        if (targetTilemapsProp != null && grassTilesProp != null && 
            yOffsetProp != null && updateDynamicallyProp != null && 
            optimizeMeshProp != null && showDebugGizmosProp != null &&
            generateColliderProp != null)
        {
            // Tilemap Reference section
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Tilemap Reference", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(targetTilemapsProp, true);
            EditorGUI.indentLevel--;
            EditorGUILayout.HelpBox("Drag and drop tilemaps here to include them in the mesh generation. Leave empty to use all tilemaps in the scene.", MessageType.Info);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Tile Filtering section
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Tile Filtering", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(grassTilesProp, true);
            EditorGUI.indentLevel--;
            EditorGUILayout.HelpBox("Drag and drop tile assets here to include them in the mesh generation. Leave empty to include all tiles.", MessageType.Info);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Mesh Settings section
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Mesh Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(yOffsetProp);
            EditorGUILayout.PropertyField(updateDynamicallyProp);
            EditorGUILayout.PropertyField(optimizeMeshProp);
            EditorGUILayout.PropertyField(generateColliderProp);
            EditorGUI.indentLevel--;
            EditorGUILayout.HelpBox("Enable Generate Collider to create a MeshCollider for physics interactions.", MessageType.Info);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Debug section
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(showDebugGizmosProp);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Action buttons
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Generate Mesh", GUILayout.Width(120)))
            {
                TilemapMeshGenerator generator = (TilemapMeshGenerator)target;
                
                // Apply any property changes before generating
                serializedObject.ApplyModifiedProperties();
                
                // Make sure components are initialized before generating the mesh
                if (generator.EnsureInitialized())
                {
                    generator.GenerateMeshFromTilemap();
                    SceneView.RepaintAll();
                }
                else
                {
                    // If initialization failed, show an error message
                    EditorUtility.DisplayDialog("Mesh Generation Failed", 
                        "Failed to initialize required components. Make sure a Tilemap is assigned.", "OK");
                }
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.HelpBox("Some properties could not be found. The component may not be initialized correctly.", MessageType.Error);
        }

        serializedObject.ApplyModifiedProperties();
    }
} 