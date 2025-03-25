using Raven.Attributes;
using UnityEngine;
using UnityEditor;

namespace Raven.Drawers
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleDrawer : PropertyDrawer
    {
        private TitleAttribute TitleAttribute => (TitleAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the title
            Rect titleRect = position;
            titleRect.height = EditorGUIUtility.singleLineHeight * 1.5f;

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.fontSize = 14;
            titleStyle.alignment = TextAnchor.MiddleLeft;
            titleStyle.padding = new RectOffset(0, 0, 5, 5);

            if (TitleAttribute.Bold) titleStyle.fontStyle = FontStyle.Bold;
            if (TitleAttribute.Italic) titleStyle.fontStyle |= FontStyle.Italic;
            
            // Use rich text for underline and strikethrough since FontStyle doesn't support them
            string titleText = TitleAttribute.Title;
            if (TitleAttribute.Underline) titleText = $"<u>{titleText}</u>";
            if (TitleAttribute.Strikethrough) titleText = $"<s>{titleText}</s>";
            
            // Enable rich text in the style
            titleStyle.richText = TitleAttribute.Underline || TitleAttribute.Strikethrough;
            
            if (TitleAttribute.TitleColor) titleStyle.normal.textColor = TitleAttribute.Color;

            EditorGUI.LabelField(titleRect, titleText, titleStyle);

            // Draw the subtitle if it exists
            if (!string.IsNullOrEmpty(TitleAttribute.Subtitle))
            {
                Rect subtitleRect = titleRect;
                subtitleRect.y += titleRect.height;
                subtitleRect.height = EditorGUIUtility.singleLineHeight;
                
                GUIStyle subtitleStyle = new GUIStyle(EditorStyles.miniLabel);
                subtitleStyle.padding = new RectOffset(0, 0, 5, 5);
                
                EditorGUI.LabelField(subtitleRect, TitleAttribute.Subtitle, subtitleStyle);
            }

            // Draw the property
            Rect propertyRect = position;
            propertyRect.y += titleRect.height + (string.IsNullOrEmpty(TitleAttribute.Subtitle) ? 0 : EditorGUIUtility.singleLineHeight);
            propertyRect.height = EditorGUI.GetPropertyHeight(property, label, true);
            
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float titleHeight = EditorGUIUtility.singleLineHeight * 1.5f;
            float subtitleHeight = string.IsNullOrEmpty(TitleAttribute.Subtitle) ? 0 : EditorGUIUtility.singleLineHeight;
            float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            
            return titleHeight + subtitleHeight + propertyHeight;
        }
    }
} 