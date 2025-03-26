using Raven.Attributes;
using UnityEngine;
using UnityEditor;

namespace Raven.Drawers
{
	[CustomPropertyDrawer(typeof(LabelTextAttribute))]
	public class LabelTextDrawer : PropertyDrawer
	{
		private LabelTextAttribute _labelTextAttribute => (LabelTextAttribute)attribute;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Create a new label with the custom text
			GUIContent customLabel = new GUIContent(_labelTextAttribute.Label);

			// Apply styling using custom draw
			if (_labelTextAttribute.Bold || _labelTextAttribute.Italic || _labelTextAttribute.Color != Color.black)
			{
				// Use a modified position to place the label and the field
				Rect labelRect = position;
				labelRect.width = EditorGUIUtility.labelWidth;

				// Apply text style
				GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
				if (_labelTextAttribute.Bold) labelStyle.fontStyle = FontStyle.Bold;
				if (_labelTextAttribute.Italic) labelStyle.fontStyle |= FontStyle.Italic;
				if (_labelTextAttribute.Color != Color.black) labelStyle.normal.textColor = _labelTextAttribute.Color;

				// Draw the custom label
				EditorGUI.LabelField(labelRect, customLabel, labelStyle);

				// Draw the property field without a label
				Rect fieldRect = position;
				fieldRect.x += EditorGUIUtility.labelWidth;
				fieldRect.width -= EditorGUIUtility.labelWidth;

				EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
			}
			else
			{
				// If no special styling, just use the standard property field with custom label
				EditorGUI.PropertyField(position, property, customLabel, true);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
	}
}