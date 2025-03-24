using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace raven
{
    [CustomPropertyDrawer(typeof(FoldoutGroupAttribute))]
    public class FoldoutGroupDrawer : PropertyDrawer
    {
        private static readonly Dictionary<string, bool> FoldoutStates = new Dictionary<string, bool>();

        private FoldoutGroupAttribute FoldoutGroupAttribute => (FoldoutGroupAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string groupKey = $"{property.serializedObject.targetObject.GetInstanceID()}_{FoldoutGroupAttribute.GroupName}";
            
            if (!FoldoutStates.ContainsKey(groupKey))
            {
                FoldoutStates[groupKey] = FoldoutGroupAttribute.Expanded;
            }

            // Draw the foldout header
            Rect headerRect = position;
            headerRect.height = EditorGUIUtility.singleLineHeight;

            GUIStyle headerStyle = new GUIStyle(EditorStyles.foldoutHeader);
            headerStyle.fontStyle = FoldoutGroupAttribute.ShowTitle ? FontStyle.Bold : FontStyle.Normal;
            headerStyle.alignment = FoldoutGroupAttribute.CenterTitle ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;

            FoldoutStates[groupKey] = EditorGUI.Foldout(headerRect, FoldoutStates[groupKey], FoldoutGroupAttribute.ShowTitle ? FoldoutGroupAttribute.GroupName : "", true, headerStyle);

            if (FoldoutStates[groupKey])
            {
                // Draw the property
                Rect propertyRect = position;
                propertyRect.y += EditorGUIUtility.singleLineHeight + 5;
                propertyRect.height = EditorGUI.GetPropertyHeight(property, label, true);

                EditorGUI.PropertyField(propertyRect, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string groupKey = $"{property.serializedObject.targetObject.GetInstanceID()}_{FoldoutGroupAttribute.GroupName}";
            if (!FoldoutStates.ContainsKey(groupKey) || !FoldoutStates[groupKey])
            {
                return EditorGUIUtility.singleLineHeight;
            }

            return EditorGUIUtility.singleLineHeight + 5 + EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
} 