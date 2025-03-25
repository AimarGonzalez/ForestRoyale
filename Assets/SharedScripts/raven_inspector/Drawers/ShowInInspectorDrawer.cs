using Raven.editor;
using UnityEngine;
using UnityEditor;

namespace Raven.Drawers
{
    [CustomPropertyDrawer(typeof(ShowInInspectorAttribute))]
    public class ShowInInspectorDrawer : PropertyDrawer
    {
        private ShowInInspectorAttribute ShowInInspectorAttribute => (ShowInInspectorAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if we should show the property based on the condition
            if (ShowInInspectorAttribute.ShowIf && !string.IsNullOrEmpty(ShowInInspectorAttribute.Condition))
            {
                if (!RavenInspectorUtility.EvaluateCondition(property, ShowInInspectorAttribute.Condition))
                {
                    return;
                }
            }

            // Draw the property
            EditorGUI.BeginDisabledGroup(ShowInInspectorAttribute.ReadOnly);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // If the property is hidden by condition, return 0 height
            if (ShowInInspectorAttribute.ShowIf && !string.IsNullOrEmpty(ShowInInspectorAttribute.Condition))
            {
                if (!RavenInspectorUtility.EvaluateCondition(property, ShowInInspectorAttribute.Condition))
                {
                    return 0f;
                }
            }
            
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
} 