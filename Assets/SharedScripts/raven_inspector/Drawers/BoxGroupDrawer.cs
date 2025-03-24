using UnityEngine;
using UnityEditor;

namespace raven
{
    [CustomPropertyDrawer(typeof(BoxGroupAttribute))]
    public class BoxGroupDrawer : PropertyDrawer
    {
        private BoxGroupAttribute BoxGroupAttribute => (BoxGroupAttribute)attribute;
        private static readonly Color DefaultBoxColor = new Color(0.8f, 0.8f, 0.8f, 0.2f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the box
            Rect boxRect = position;
            boxRect.height = GetPropertyHeight(property, label);
            
            // Create a style for the box
            GUIStyle boxStyle = new GUIStyle(EditorStyles.helpBox);
            boxStyle.padding = new RectOffset(10, 10, 10, 10);
            
            // Draw the box with the specified color
            Color originalColor = GUI.color;
            
            // Only use the custom color if specified, otherwise use default
            GUI.color = BoxGroupAttribute.HasCustomColor ? BoxGroupAttribute.Color : DefaultBoxColor;
            GUI.Box(boxRect, GUIContent.none, boxStyle);
            GUI.color = originalColor;

            // Draw the title if enabled
            if (BoxGroupAttribute.ShowTitle)
            {
                Rect titleRect = boxRect;
                titleRect.height = EditorGUIUtility.singleLineHeight;
                titleRect.y += 5;
                titleRect.x += 5;
                titleRect.width -= 10;

                GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
                titleStyle.alignment = BoxGroupAttribute.CenterTitle ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
                titleStyle.padding = new RectOffset(0, 0, 5, 5);

                EditorGUI.LabelField(titleRect, BoxGroupAttribute.GroupName, titleStyle);
            }

            // Draw the property
            Rect propertyRect = boxRect;
            propertyRect.x += BoxGroupAttribute.Padding;
            propertyRect.y += BoxGroupAttribute.ShowTitle ? EditorGUIUtility.singleLineHeight + 10 : BoxGroupAttribute.Padding;
            propertyRect.width -= BoxGroupAttribute.Padding * 2;
            propertyRect.height = EditorGUI.GetPropertyHeight(property, label, true);

            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            float titleHeight = BoxGroupAttribute.ShowTitle ? EditorGUIUtility.singleLineHeight + 10 : 0;
            float padding = BoxGroupAttribute.Padding * 2;
            
            return propertyHeight + titleHeight + padding;
        }
    }
} 