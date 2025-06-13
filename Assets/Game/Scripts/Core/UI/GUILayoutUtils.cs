using Sirenix.Utilities;
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

		public static void BeginVerticalBox(IGUIDrawer drawer)
		{
			if (drawer == null)
			{
				return;
			}

			GUILayout.BeginVertical(GUI.skin.box);
			drawer.DrawGUI();
			GUILayout.EndVertical();
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

		public static void FieldLabel(string label)
		{
			GUILayout.BeginHorizontal(GUILayoutOptions.Width(LabelWidth));
			GUILayout.Label(label, GUILayoutOptions.Height(LabelHeight));
			GUILayout.EndHorizontal();
		}

		public static void Label(string label)
		{
			GUILayout.Label(label, GUILayoutOptions.Height(LabelHeight));
		}

		public static string TextField(string label, string text)
		{
			GUILayout.BeginHorizontal();
			FieldLabel(label);
			text = GUILayout.TextField(text);
			GUILayout.EndHorizontal();
			return text;
		}

		public static int IntField(string label, int value)
		{
			GUILayout.BeginHorizontal();
			FieldLabel(label);
			value = ToInt(GUILayout.TextField(value.ToString()));
			GUILayout.EndHorizontal();
			return value;
		}

		public static float FloatField(string label, float value)
		{
			GUILayout.BeginHorizontal();
			FieldLabel(label);
			value = ToFloat(GUILayout.TextField(value.ToString()));
			GUILayout.EndHorizontal();
			return value;
		}

		public static float Slider(string label, float value, float min, float max)
		{
			GUILayout.BeginHorizontal();
			FieldLabel(label);
			value = ToFloat(GUILayout.TextField(value.ToString("F1"), GUILayoutOptions.MaxWidth(50)));
			//GUILayout.Label(min.ToString("F1"));
			value = GUILayout.HorizontalSlider(value, min, max);
			//GUILayout.Label(max.ToString("F1"));
			GUILayout.EndHorizontal();
			return value;
		}

		public static float LogSlider(string label, float value, float min, float max)
		{
			float logMin = Mathf.Log10(min);
			float logMax = Mathf.Log10(max);
			float logCurrent = Mathf.Log10(value);

			GUILayout.BeginHorizontal();
			FieldLabel(label);
			value = ToFloat(GUILayout.TextField(value.ToString("F1"), GUILayoutOptions.MaxWidth(50)));
			float newLogValue = GUILayout.HorizontalSlider(logCurrent, logMin, logMax);
			value = Mathf.Pow(10f, newLogValue);
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
