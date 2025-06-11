using UnityEditor;
using UnityEngine;

namespace ForestRoyale.Core.UI
{
	public static class HandleGUIUtils
	{
		public static void DrawDebugPanel(GUIUtils.Property[] properties, Transform transform, GUIUtils.PanelPlacement panelPlacement)
		{
			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			(Vector2 panelSize, float labelWidth, float valueWidth) = GUIUtils.CalcPanelSize(panelStyle, properties);
			Vector3 panelPosition = GUIUtils.CalcPanelPosition(transform, panelSize, panelPlacement);

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