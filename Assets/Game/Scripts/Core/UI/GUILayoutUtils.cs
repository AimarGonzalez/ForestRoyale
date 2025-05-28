using Sirenix.Utilities;
using System.Reflection.Emit;
using UnityEngine;

namespace ForestRoyale.Core.UI
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

		public static float LabelWidth = 100f;
		public static float LabelHeight = 20f;

		public static void Label(string label)
		{
			GUILayout.BeginHorizontal(GUILayoutOptions.Width(LabelWidth));
			GUILayout.Label(label, GUILayoutOptions.Height(LabelHeight));
			GUILayout.EndHorizontal();
		}

		public static string TextField(string label, string text)
		{
			GUILayout.BeginHorizontal();
			Label(label);
			text = GUILayout.TextField(text);
			GUILayout.EndHorizontal();
			return text;
		}

		public static int IntField(string label, int value)
		{
			GUILayout.BeginHorizontal();
			Label(label);
			value = ToInt(GUILayout.TextField(value.ToString()));
			GUILayout.EndHorizontal();
			return value;
		}

		public static float FloatField(string label, float value)
		{
			GUILayout.BeginHorizontal();
			Label(label);
			value = ToFloat(GUILayout.TextField(value.ToString()));
			GUILayout.EndHorizontal();
			return value;
		}

		public static int Slider(string label, int value, int min, int max, int height)
		{
			GUILayout.BeginHorizontal();
			Label(label);
			value = (int)GUILayout.HorizontalSlider(value, min, max);
			GUILayout.EndHorizontal();
			return value;
		}

		private static int ToInt(string text)
		{
			if (int.TryParse(text, out int value))
			{
				return value;
			}
			return 0;
		}

		private static float ToFloat(string text)
		{
			if (float.TryParse(text, out float value))
			{
				return value;
			}
			return 0;
		}
	}
}
