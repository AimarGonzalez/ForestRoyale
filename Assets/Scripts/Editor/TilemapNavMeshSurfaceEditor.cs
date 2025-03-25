using UnityEngine;
using UnityEditor;
using UnityEditor.AI;
using Raven.Gameplay.Navigation;

namespace Raven.Editor
{
    [CustomEditor(typeof(TilemapNavMeshSurface))]
    public class TilemapNavMeshSurfaceEditor : UnityEditor.Editor
    {
        private SerializedProperty buildNavMeshOnStartProp;
        private SerializedProperty navMeshSurfaceProp;

        private void OnEnable()
        {
            buildNavMeshOnStartProp = serializedObject.FindProperty("rebuildNavMeshOnStart");
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
            
            if (GUILayout.Button("Update NavMesh", GUILayout.Width(120)))
            {
                serializedObject.ApplyModifiedProperties();
                
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
}