using Raven.Attributes;
using UnityEditor;
using UnityEngine;

namespace Raven.Drawers
{
	[CustomPropertyDrawer(typeof(LabelTextAttribute))]
	public class LabelTextDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			LabelTextAttribute labelTextAttribute = (LabelTextAttribute)attribute;
			
			// Create a new label with the custom text
			GUIContent customLabel = new GUIContent(labelTextAttribute.Label);

			// Apply styling using custom draw
			if (labelTextAttribute.Bold || labelTextAttribute.Italic || labelTextAttribute.Color != Color.black)
			{
				// Use a modified position to place the label and the field
				Rect labelRect = position;
				labelRect.width = EditorGUIUtility.labelWidth;

				// Apply text style
				GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
				if (labelTextAttribute.Bold)
				{
					labelStyle.fontStyle = FontStyle.Bold;
				}

				if (labelTextAttribute.Italic)
				{
					labelStyle.fontStyle |= FontStyle.Italic;
				}

				if (labelTextAttribute.Color != Color.black)
				{
					labelStyle.normal.textColor = labelTextAttribute.Color;
				}


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