using UnityEngine;
using UnityEditor;
using UnityEditor.AI;

[CustomEditor(typeof(TilemapNavMeshSurface))]
public class TilemapNavMeshSurfaceEditor : Editor
{
    private SerializedProperty buildNavMeshOnStartProp;
    private SerializedProperty navMeshSurfaceProp;

    private void OnEnable()
    {
        buildNavMeshOnStartProp = serializedObject.FindProperty("buildNavMeshOnStart");
        navMeshSurfaceProp = serializedObject.FindProperty("navMeshSurface");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tilemap NavMesh Surface", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This component uses the generated grass mesh as a NavMesh surface.", MessageType.Info);
        EditorGUILayout.Space();

        // NavMesh Settings section
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("NavMesh Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(buildNavMeshOnStartProp);
        EditorGUILayout.PropertyField(navMeshSurfaceProp);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // Navigation status
        TilemapNavMeshSurface navMeshSurface = (TilemapNavMeshSurface)target;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Navigation Status", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Has NavMeshSurface:", navMeshSurface.navMeshSurface != null ? "Yes" : "No");
        
        bool hasNavMeshData = false;
        if (navMeshSurface.navMeshSurface != null)
        {
            hasNavMeshData = navMeshSurface.navMeshSurface.navMeshData != null;
        }
        EditorGUILayout.LabelField("Has NavMesh Data:", hasNavMeshData ? "Yes" : "No");
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // Action buttons
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Update Mesh", GUILayout.Width(120)))
        {
            // Apply any property changes before updating
            serializedObject.ApplyModifiedProperties();
            
            // Try to initialize components
            if (navMeshSurface.EnsureInitialized())
            {
                TilemapMeshGenerator meshGenerator = navMeshSurface.GetComponent<TilemapMeshGenerator>();
                if (meshGenerator != null && meshGenerator.EnsureInitialized())
                {
                    meshGenerator.GenerateMeshFromTilemap();
                    SceneView.RepaintAll();
                    EditorUtility.SetDirty(target);
                }
                else
                {
                    EditorUtility.DisplayDialog("Mesh Update Failed",
                        "Failed to initialize the TilemapMeshGenerator component.", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Initialization Failed",
                    "Failed to initialize required components for NavMesh generation.", "OK");
            }
        }
        
        if (GUILayout.Button("Build NavMesh", GUILayout.Width(120)))
        {
            // Apply any property changes before building
            serializedObject.ApplyModifiedProperties();
            
            // This will handle component initialization internally
            navMeshSurface.BuildNavMesh();
            SceneView.RepaintAll();
            EditorUtility.SetDirty(target);
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        // Show a button to open the Navigation window
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Open Navigation Window", GUILayout.Width(180)))
        {
            NavMeshEditorHelpers.OpenAgentSettings(0);
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
} 