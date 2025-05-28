using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Combat;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Core
{
	[ExecuteInEditMode]
	public class CheatsMenu : MonoBehaviour
	{
		// ------------------
		private const string PANEL_SETTINGS = "Panel settings";

		[SerializeField]
		private GUISkin _skin;

		[Serializable]
		private struct PanelSettings
		{
			[Serializable]
			private struct AspectRatio
			{
				public int Width;
				public int Height;

				public AspectRatio(int width, int height)
				{
					Width = width;
					Height = height;
				}

				public float Ratio => (float)Width / Height;
			}

			[SerializeField]
			private AspectRatio _aspectRatio;

			public float Ratio => _aspectRatio.Ratio;
			public int FontSize;
			public float LabelWidth;
			public float Width;
			public float Height;

			public PanelSettings(int fontSize, float labelWidth, float width, float height)
			{
				_aspectRatio = new AspectRatio(1, 1);
				FontSize = fontSize;
				LabelWidth = labelWidth;
				Width = width;
				Height = height;
			}
		}

		[SerializeField]
		[TableList]
		[BoxGroup(PANEL_SETTINGS), PropertyOrder(DebugUI.Order)]
		private List<PanelSettings> _panelSettings;

		// ------------------

		[Inject]
		private TimeController _timeController;

		[Inject]
		private Battle _battle;

		private void OnGUI()
		{
			PanelSettings panelSettings = CalcInterpolatedPanelSettings();


			GUIUtils.PushSkin(_skin);
			GUIUtils.PushFontSize(panelSettings.FontSize);

			GUILayoutUtils.LabelWidth = panelSettings.LabelWidth * panelSettings.Width * Screen.width;
			GUILayoutUtils.LabelHeight = GUI.skin.label.CalcHeight(new GUIContent("X"), 100);

			GUILayout.BeginArea(new Rect(10, 10, panelSettings.Width * Screen.width, panelSettings.Height * Screen.height), GUI.skin.box);
			DrawCheatsGUI();
			GUILayout.EndArea();

			GUIUtils.PopFontSize();
			GUIUtils.PopSkin();
		}

		private PanelSettings CalcInterpolatedPanelSettings()
		{
			if (_panelSettings.IsNullOrEmpty())
			{
				return new PanelSettings(12, 0.5f, 0.5f, 0.5f);
			}

			PanelSettings minSettings = _panelSettings[0];
			PanelSettings maxSettings = _panelSettings[_panelSettings.Count - 1];
			float screenRatio = (float)Screen.width / Screen.height;
			foreach (PanelSettings panelSettings in _panelSettings)
			{
				if (screenRatio < panelSettings.Ratio && panelSettings.Ratio > minSettings.Ratio)
				{
					minSettings = panelSettings;
				}

				if (screenRatio > panelSettings.Ratio && panelSettings.Ratio < maxSettings.Ratio)
				{
					maxSettings = panelSettings;
				}
			}

			float ratio = 1f;

			if (minSettings.Ratio != maxSettings.Ratio)
			{
				ratio = (screenRatio - minSettings.Ratio) / (maxSettings.Ratio - minSettings.Ratio);
			}

			PanelSettings interpolatedSettings = new PanelSettings(
				(int)Mathf.Lerp(minSettings.FontSize, maxSettings.FontSize, ratio),
				Mathf.Lerp(minSettings.LabelWidth, maxSettings.LabelWidth, ratio),
				Mathf.Lerp(minSettings.Width, maxSettings.Width, ratio),
				Mathf.Lerp(minSettings.Height, maxSettings.Height, ratio)
			);

			return interpolatedSettings;
		}

		private void DrawCheatsGUI()
		{
			GUILayout.BeginVertical();

			GUILayoutUtils.BeginVerticalBox(_timeController);
			GUILayoutUtils.BeginVerticalBox(_battle);

			GUILayout.EndVertical();
		}
	}
}
