using Sirenix.Utilities;
using UnityEngine;

namespace ForestRoyale.Gui
{
	public static class GUILayoutUtils
	{
		public static void BeginVerticalBox()
		{
			GUILayout.BeginVertical(GUI.skin.box);
		}

		public static void BeginHorizontalBox()
		{
			GUILayout.BeginVertical(GUI.skin.box);
		}

		public static void BeginVertical(GUIStyle style, Color backgroundColor)
		{
			GUIUtils.PushColor(backgroundColor);
			GUILayout.BeginVertical(style);
			GUIUtils.PopColor();
		}

		public static void BeginHorizontal(GUIStyle style, Color backgroundColor)
		{
			GUIUtils.PushColor(backgroundColor);
			GUILayout.BeginHorizontal(style);
			GUIUtils.PopColor();
		}

		public static void EndVerticalBox() => GUILayout.EndVertical();
		public static void EndHorizontalBox() => GUILayout.EndVertical();
		public static void EndVertical() => GUILayout.EndVertical();
		public static void EndHorizontal() => GUILayout.EndHorizontal();

		public static void TextField(string label, string text, GUIStyle labelStyle, GUIStyle textFieldStyle, float labelWidth)
		{
			GUILayout.BeginHorizontal();
			GUILayout.BeginHorizontal(GUILayoutOptions.Width(labelWidth));
			GUILayout.Label(label, labelStyle);
			GUILayout.EndHorizontal();
			GUILayout.TextField(text, textFieldStyle);
			GUILayout.EndHorizontal();
		}

	}
}
