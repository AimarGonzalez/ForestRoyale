using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gui
{
	public static class GUIUtils
	{
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
			PushColor(backgroundColor);
			GUILayout.BeginVertical(style);
			PopColor();
		}

		public static void BeginHorizontal(GUIStyle style, Color backgroundColor)
		{
			PushColor(backgroundColor);
			GUILayout.BeginHorizontal(style);
			PopColor();
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