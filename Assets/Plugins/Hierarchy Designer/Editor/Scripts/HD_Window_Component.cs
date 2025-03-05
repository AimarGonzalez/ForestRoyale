#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Verpha.HierarchyDesigner
{
    internal class HD_Window_Component : EditorWindow
    {
        #region Properties
        private Vector2 scrollPosition;
        private Component targetComponent;
        private Editor componentEditor;
        private Texture2D componentIcon;
        #endregion

        #region Initialization
        private void OnEnable()
        {
            PropertyInfo style = GetType().GetProperty("wantsMouseMove", BindingFlags.Instance | BindingFlags.NonPublic);
            if (style != null) style.SetValue(this, true);
        }

        public void InitializeWindow(Component component, Vector2 mousePosition)
        {
            targetComponent = component;

            minSize = new(400, 200);
            maxSize = new(600, 800);

            float width = 400;
            float height = 400;
            float screenWidth = Screen.currentResolution.width;
            float screenHeight = Screen.currentResolution.height;

            float x = Mathf.Clamp(mousePosition.x, 0, screenWidth - width);
            float y = Mathf.Clamp(mousePosition.y, 0, screenHeight - height);

            position = new(x, y, width, height);

            if (targetComponent != null)
            {
                componentEditor = Editor.CreateEditor(targetComponent);
                componentIcon = EditorGUIUtility.ObjectContent(targetComponent, targetComponent.GetType()).image as Texture2D;
                titleContent = new(targetComponent.GetType().Name + " Properties", componentIcon);
            }
        }
        #endregion

        private void OnGUI()
        {
            if (targetComponent == null)
            {
                Close();
                return;
            }

            if (componentEditor == null && targetComponent != null)
            {
                componentEditor = Editor.CreateEditor(targetComponent);
            }

            using (new EditorGUILayout.VerticalScope())
            {
                DrawHeaderBar();

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                    if (componentEditor != null)
                    {
                        EditorGUI.BeginChangeCheck();
                        componentEditor.OnInspectorGUI();
                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(targetComponent);
                        }
                    }

                    EditorGUILayout.EndScrollView();
                }
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                Close();
            }
        }

        #region Methods
        private void DrawHeaderBar()
        {
            EditorGUILayout.Space(2);
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.HorizontalScope(GUILayout.Width(120)))
                {
                    bool isEnabled = false;
                    bool canBeDisabled = false;

                    if (targetComponent is Behaviour behaviour)
                    {
                        isEnabled = behaviour.enabled;
                        canBeDisabled = true;
                    }
                    else if (targetComponent is Renderer renderer)
                    {
                        isEnabled = renderer.enabled;
                        canBeDisabled = true;
                    }
                    else if (targetComponent is Collider collider)
                    {
                        isEnabled = collider.enabled;
                        canBeDisabled = true;
                    }

                    EditorGUI.BeginChangeCheck();
                    GUI.enabled = canBeDisabled;
                    bool newEnabled = EditorGUILayout.Toggle(isEnabled, GUILayout.Width(16));
                    GUI.enabled = true;

                    if (EditorGUI.EndChangeCheck() && canBeDisabled)
                    {
                        Undo.RecordObject(targetComponent, "Toggle Component");
                        if (targetComponent is Behaviour behaviour2)
                        {
                            behaviour2.enabled = newEnabled;
                        }
                        else if (targetComponent is Renderer renderer2)
                        {
                            renderer2.enabled = newEnabled;
                        }
                        else if (targetComponent is Collider collider2)
                        {
                            collider2.enabled = newEnabled;
                        }
                    }

                    using (new EditorGUI.DisabledGroupScope(!canBeDisabled))
                    {
                        GUILayout.Label("Enable", HD_Common_GUI.ComponentWindowTitleLabelStyle, GUILayout.Width(80));
                    }
                }
            }
            EditorGUILayout.Space(2);
        }
        #endregion

        private void OnDisable()
        {
            if (componentEditor != null)
            {
                DestroyImmediate(componentEditor);
                componentEditor = null;
            }
        }
    }
}
#endif