using System.Collections.Generic;
using ForestLib.Utils;
using UnityEditor;
using UnityEngine;
using System;

namespace ForestRoyale.Gui
{
	public static class GUIUtils
	{
		public enum PanelPlacement
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

			public Property(string label, int value)
				: this(label, value.ToString())
			{
			}

			public Property(string label, float value)
				: this(label, value.ToString())
			{
			}

			public Property(string label, Vector3 value)
				: this(label, $"v({value.x}, {value.y}, {value.z})")
			{
			}

			public Property(string label, Enum value)
				: this(label, value.ToString())
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
			GUIStyle panelStyle,
			float labelWidth,
			float valueWidth = 0f
			)
		{

			float rowHeigh = Mathf.Max(property.labelSize.y, property.valueSize.y);
			valueWidth = valueWidth > 0f ? valueWidth : property.valueSize.x;

			GUI.Label(
				new Rect(
					panelStyle.padding.left + rect.xMin,
					panelStyle.padding.top + rect.yMin + index * rowHeigh,
					labelWidth,
					rowHeigh),
				property.label,
				property.labelStyle);

			PushBackgroundColor(new Color(0f, 0f, 0f, 0.8f));
			GUI.TextField(
				new Rect(
					panelStyle.padding.left + labelWidth + rect.xMin,
					panelStyle.padding.top + rect.yMin + index * rowHeigh,
					valueWidth,
					rowHeigh),
				property.value,
				property.valueStyle);

			PopBackgroundColor();
		}

		public static (Vector2 size, float labelWidth, float valueWidth) CalcPanelSize(GUIStyle panelStyle, GUIUtils.Property[] properties)
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
			return (size, maxLabelWidth, maxValueWidth);
		}

		public static Vector3 CalcPanelPosition(Transform transform, Vector2 size, PanelPlacement panelPosition)
		{
			var characterBounds = MeshUtils.GetBoundingBox(transform);

			float verticalOffset = characterBounds.size.z * 0.5f;
			float horizontalOffset = characterBounds.size.x * 0.5f;
			Vector2 screenOffset = Vector2.zero;
			Vector3 worldOffset = Vector3.zero;

			switch (panelPosition)
			{
				case PanelPlacement.Top:
					screenOffset = Vector2.down * (size.y * 0.5f);
					worldOffset = Vector3.forward * verticalOffset;
					break;
				case PanelPlacement.Bottom:
					screenOffset = Vector2.up * (size.y * 0.5f);
					worldOffset = Vector3.back * verticalOffset;
					break;
				case PanelPlacement.Left:
					screenOffset = Vector2.left * (size.x * 0.5f);
					worldOffset = Vector3.left * horizontalOffset;
					break;
				case PanelPlacement.Right:
					screenOffset = Vector2.right * (size.x * 0.5f);
					worldOffset = Vector3.right * horizontalOffset;
					break;
			}

			Vector3 worldPosition = transform.position + worldOffset;
			Vector2 screenPoint = HandleUtility.WorldToGUIPoint(worldPosition);
			Vector3 labelPosition = screenPoint + screenOffset;
			return labelPosition;
		}

		public static void DrawDebugPanel(Property[] properties, Transform transform, PanelPlacement panelPlacement)
		{
			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			(Vector2 panelSize, float labelWidth, float valueWidth) = CalcPanelSize(panelStyle, properties);
			Vector3 panelPosition = CalcPanelPosition(transform, panelSize, panelPlacement);

			Handles.BeginGUI();
			{
				// Create rect centered on the panel's position
				Rect rect = new Rect(panelPosition.x - panelSize.x * 0.5f, panelPosition.y - panelSize.y * 0.5f, panelSize.x, panelSize.y);

				GUI.Box(rect, GUIContent.none, panelStyle);

				for (int i = 0; i < properties.Length; i++)
				{
					GUIUtils.DrawTextField(i, properties[i], rect, panelStyle, labelWidth, valueWidth);
				}
			}
			Handles.EndGUI();
		}

	}
}