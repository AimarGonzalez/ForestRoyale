using UnityEngine;
using UnityEditor;

namespace raven
{
    [CustomPropertyDrawer(typeof(PropertyOrderAttribute))]
    public class PropertyOrderDrawer : PropertyDrawer
    {
        private PropertyOrderAttribute PropertyOrderAttribute => (PropertyOrderAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
} 