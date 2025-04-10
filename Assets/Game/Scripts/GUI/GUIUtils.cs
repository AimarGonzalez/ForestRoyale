using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ForestRoyale.Gui
{
	public static class GUIUtils
	{
		public enum PanelPosition
		{
			Bottom,
			Top,
			Left,
			Right
		}

		public struct Property
		{
			public string label;
			public string value;

			public GUIStyle labelStyle;
			public GUIStyle valueStyle;

			public Vector2 labelSize;
			public Vector2 valueSize;

			public Property(string label)
				: this(label, "", GUI.skin.label, GUI.skin.label)
			{
			}

			public Property(string label, string value)
				: this(label, value, GUI.skin.label, GUI.skin.textField)
			{
			}

			public Property(string label, string value, GUIStyle valueStyle)
				: this(label, value, GUI.skin.label, valueStyle)
			{
			}

			public Property(string label, string value, GUIStyle labelStyle, GUIStyle valueStyle)
			{
				this.label = label;
				this.value = value;
				this.labelStyle = labelStyle;
				this.valueStyle = valueStyle;
				this.labelSize = labelStyle.CalcSize(new GUIContent(label));
				this.valueSize = valueStyle.CalcSize(new GUIContent(value));
			}
		}

		private static Stack<Color> _colorStack = new Stack<Color>();
		private static Stack<Color> _bgColorStack = new Stack<Color>();
		private static Stack<Color> _contentColorStack = new Stack<Color>();

		public static void PushColor(Color col)
		{
			_colorStack.Push(GUI.color);
			GUI.color = col;
		}

		public static void PopColor()
		{
			GUI.color = _colorStack.Pop();
		}

		public static void PushBackgroundColor(Color col)
		{
			_bgColorStack.Push(GUI.color);
			GUI.backgroundColor = col;
		}

		public static void PopBackgroundColor()
		{
			GUI.backgroundColor = _bgColorStack.Pop();
		}

		public static void PushContentColor(Color col)
		{
			_contentColorStack.Push(GUI.contentColor);
			GUI.contentColor = col;
		}

		public static void PopContentColor()
		{
			GUI.contentColor = _contentColorStack.Pop();
		}
	
		public static void DrawTextField(int index,
			Property property,
			Rect rect,
			float labelWidth,
			GUIStyle panelStyle)
		{
			Vector2 labelSize = property.labelSize;
			Vector2 valueSize = property.valueSize;
			float rowHeigh = Mathf.Max(labelSize.y, valueSize.y);

			GUI.Label(
				new Rect(
					panelStyle.padding.left + rect.xMin,
					panelStyle.padding.top + rect.yMin + index * rowHeigh,
					labelWidth,
					rowHeigh),
				property.label,
				property.labelStyle);

			GUI.Label(
				new Rect(
					panelStyle.padding.left + labelWidth + rect.xMin,
					panelStyle.padding.top + rect.yMin + index * rowHeigh,
					valueSize.x,
					rowHeigh),
				property.value,
				property.valueStyle);
		}

		public static (Vector2 size, float labelWidth) CalcPanelSize(GUIStyle panelStyle, GUIUtils.Property[] properties)
		{
			float maxRowHeight = 0f;
			float maxLabelWidth = 0f;
			float maxValueWidth = 0f;

			foreach (var property in properties)
			{
				maxRowHeight = Mathf.Max(maxRowHeight, Mathf.Max(property.labelSize.y, property.valueSize.y));

				maxLabelWidth = Mathf.Max(maxLabelWidth, property.labelSize.x);
				maxValueWidth = Mathf.Max(maxValueWidth, property.valueSize.x);
			}

			Vector2 size = new Vector2(maxLabelWidth + maxValueWidth + panelStyle.padding.horizontal, properties.Length * maxRowHeight + panelStyle.padding.vertical);
			return (size, maxLabelWidth);
		}

		public static Vector3 CalcPanelPosition(Transform transform, Vector2 size, PanelPosition panelPosition)
		{
			float baseWorldOffset = 1f; //1 meter - TODO: Calculate based on the unit bounding box.
			Vector2 screenOffset = Vector2.zero;
			Vector3 worldOffset = Vector3.zero;

			switch (panelPosition)
			{
				case PanelPosition.Top:
					screenOffset = Vector2.down * (size.y * 0.5f);
					worldOffset = Vector3.forward * baseWorldOffset;
					break;
				case PanelPosition.Bottom:
					screenOffset = Vector2.up * (size.y * 0.5f);
					worldOffset = Vector3.back * baseWorldOffset;
					break;
				case PanelPosition.Left:
					screenOffset = Vector2.left * (size.x * 0.5f);
					worldOffset = Vector3.left * baseWorldOffset;
					break;
				case PanelPosition.Right:
					screenOffset = Vector2.right * (size.x * 0.5f);
					worldOffset = Vector3.right * baseWorldOffset;
					break;
			}

			Vector3 worldPosition = transform.position + worldOffset;
			Vector2 screenPoint = HandleUtility.WorldToGUIPoint(worldPosition);
			Vector3 labelPosition = screenPoint + screenOffset;
			return labelPosition;
		}
	}
}