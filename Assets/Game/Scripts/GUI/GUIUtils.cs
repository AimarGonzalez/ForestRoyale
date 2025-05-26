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

		public static Vector3 CalcPanelPosition(Transform transform, Vector2 size, PanelPlacement panelPosition, float margin = 0f)
		{
			const float CAMERA_ANGLE = 55f;
			float TAN_CAMERA_ANGLE = (float)Math.Tan((90f - CAMERA_ANGLE) * MathConst.Deg2Rad);
			var characterBounds = MeshUtils.GetBoundingBox(transform);

			float objectDepth = characterBounds.size.y;
			float objectHeight = characterBounds.size.z;
			float objectWidth = characterBounds.size.x;
			Vector2 screenOffset = Vector2.zero;
			Vector3 worldOffset = Vector3.zero;

			switch (panelPosition)
			{
				case PanelPlacement.Top:
					screenOffset = Vector2.down * (size.y * 0.5f + margin);
					worldOffset = Vector3.up * (objectDepth * 0.5f + objectHeight * TAN_CAMERA_ANGLE);
					break;
				case PanelPlacement.Bottom:
					screenOffset = Vector2.up * (size.y * 0.5f + margin);
					worldOffset = Vector3.down * objectDepth * 0.5f;
					break;
				case PanelPlacement.Left:
					screenOffset = Vector2.left * (size.x * 0.5f + margin);
					worldOffset = Vector3.left * objectWidth * 0.5f;
					break;
				case PanelPlacement.Right:
					screenOffset = Vector2.right * (size.x * 0.5f + margin);
					worldOffset = Vector3.right * objectWidth * 0.5f;
					break;
			}

			Vector3 worldPosition = transform.position + worldOffset;
			Vector2 guiPoint = WorldToGUIPoint(Camera.main, worldPosition);
			Vector3 labelPosition = guiPoint + screenOffset;
			return labelPosition;
		}

		public static Vector3 CalcPanelPositionOnUI(RectTransform target, Vector2 panelSize, PanelPlacement panelPlacement, float margin = 0f)
		{
			Vector2 panelOffset = Vector2.zero;
			Vector2 targetOffset = Vector2.zero;

			Rect rect = target.rect;
			float targetHeight = rect.height * 0.5f;
			float targetWidth = rect.width * 0.5f;

			switch (panelPlacement)
			{
				case PanelPlacement.Top:
					panelOffset = Vector2.up * (panelSize.y * 0.5f + margin);
					targetOffset = Vector2.up * targetHeight;
					break;
				case PanelPlacement.Bottom:
					panelOffset = Vector2.down * (panelSize.y * 0.5f + margin);
					targetOffset = Vector2.down * targetHeight;
					break;
				case PanelPlacement.Left:
					panelOffset = Vector2.left * (panelSize.x * 0.5f + margin);
					targetOffset = Vector2.left * targetWidth;
					break;
				case PanelPlacement.Right:
					panelOffset = Vector2.right * (panelSize.x * 0.5f + margin);
					targetOffset = Vector2.right * targetWidth;
					break;
			}

			Vector2 targetPosition = target.position;
			Vector2 screenPosition = targetPosition + targetOffset + panelOffset;
			Vector3 guiPoint = ScreenToGUIPoint(Camera.main, screenPosition);
			return guiPoint;
		}

		public static void DrawDebugPanel(Property[] properties, Transform target, PanelPlacement panelPlacement, float margin = 0f, Action onClose = null)
		{
			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			(Vector2 panelSize, float labelWidth, float valueWidth) = CalcPanelSize(panelStyle, properties);
			Vector3 panelPosition = CalcPanelPosition(target, panelSize, panelPlacement, margin);

			DrawDebugPanel(properties, panelPosition, panelSize, panelStyle, labelWidth, valueWidth, onClose);
		}

		public static void DrawDebugPanel(Property[] properties, RectTransform target, PanelPlacement panelPlacement, float margin = 0f, Action onClose = null)
		{
			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			(Vector2 panelSize, float labelWidth, float valueWidth) = CalcPanelSize(panelStyle, properties);
			Vector3 panelPosition = CalcPanelPositionOnUI(target, panelSize, panelPlacement, margin);

			DrawDebugPanel(properties, panelPosition, panelSize, panelStyle, labelWidth, valueWidth, onClose);
		}

		private static void DrawDebugPanel(Property[] properties, Vector3 panelPosition, Vector2 panelSize, GUIStyle style, float labelWidth, float valueWidth, Action onClose)
		{
			// Create rect centered on the panel's position
			Rect rect = new Rect(panelPosition.x - panelSize.x * 0.5f, panelPosition.y - panelSize.y * 0.5f, panelSize.x, panelSize.y);

			GUI.Box(rect, GUIContent.none, style);

			for (int i = 0; i < properties.Length; i++)
			{
				DrawTextField(i, properties[i], rect, style, labelWidth, valueWidth);
			}

			if (onClose != null)
			{
				// Get size of the button
				GUIContent closeButtonContent = new GUIContent("Close");
				Vector2 buttonSize = GUI.skin.button.CalcSize(closeButtonContent);
				bool pressed = GUI.Button(new Rect(rect.x + rect.width + 2, rect.y, buttonSize.x, buttonSize.y), closeButtonContent);
				if (pressed)
				{
					onClose?.Invoke();
				}
			}
		}

		public static Vector3 ScreenToGUIPoint(Camera camera, Vector3 screenPoint)
		{
			screenPoint.y = camera.pixelHeight - screenPoint.y;
			Vector2 points = EditorGUIUtility.PixelsToPoints((Vector2)screenPoint);
			return new Vector3(points.x, points.y, screenPoint.z);
		}

		public static Vector3 WorldToGUIPoint(Camera camera, Vector3 worldPoint)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldPoint);
			vector.y = (float)camera.pixelHeight - vector.y;
			Vector2 vector2 = EditorGUIUtility.PixelsToPoints(vector);
			return new Vector3(vector2.x, vector2.y, vector.z);
		}
	}
}