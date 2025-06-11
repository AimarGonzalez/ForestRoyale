using UnityEngine;
using UnityEditor;
using Raven.Attributes;

namespace Raven.Edtr.Drawers
{
	[CustomPropertyDrawer(typeof(RequiredAttribute))]
	public class RequiredDrawer : PropertyDrawer
	{
		private RequiredAttribute RequiredAttribute => (RequiredAttribute)attribute;
		private bool _showError;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();

			// Draw the property field
			EditorGUI.PropertyField(position, property, label, true);

			if (EditorGUI.EndChangeCheck())
			{
				_showError = IsValueMissing(property);
			}

			// Show error box if needed
			if (_showError && RequiredAttribute.ShowError)
			{
				Color previousColor = GUI.color;
				GUI.color = new Color(1, 0, 0, 0.3f);

				Rect errorPosition = position;
				errorPosition.height = EditorGUIUtility.singleLineHeight;

				EditorGUI.HelpBox(errorPosition, RequiredAttribute.ErrorMessage, MessageType.Error);
				GUI.color = previousColor;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUI.GetPropertyHeight(property, label, true);
			if (_showError && RequiredAttribute.ShowError)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}
			return height;
		}

		private bool IsValueMissing(SerializedProperty property)
		{
			switch (property.propertyType)
			{
				case SerializedPropertyType.ObjectReference:
					return property.objectReferenceValue == null;
				case SerializedPropertyType.String:
					return string.IsNullOrEmpty(property.stringValue);
				default:
					return false;
			}
		}
	}
}