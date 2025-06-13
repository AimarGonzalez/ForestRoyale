using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Core
{
	[ExecuteInEditMode]
	public class CheatsStyleProvider : MonoBehaviour
	{
		// ------------------
		private const string PANEL_SETTINGS = "Panel settings";
		private static float s_fontDPIRatio = 1.0f;

		[SerializeField]
		private GUISkin _skin;

		[SerializeField]
		private int _panelFontSize;

		[SerializeField]
		private int _buttonFontSize;

		[SerializeField]
		[TableList]
		[Tooltip("List of panel settings for different aspect ratios.\n" +
				 "The settings are interpolated to get the best fit for the current aspect ratio.")]
		[BoxGroup(PANEL_SETTINGS), PropertyOrder(DebugUI.Order)]
		private List<CheatPanelSettings> _cheatPanelSettings;

		// ------------------

		public int PanelFontSize => (int) (_panelFontSize * s_fontDPIRatio);
		public int ButtonFontSize => (int) (_buttonFontSize * s_fontDPIRatio);
		
		// -------------------
		private void Awake()
		{
			s_fontDPIRatio = UnityEngine.Device.Screen.dpi / 430f;
		}

		public CheatPanelSettings PushPanelStyle()
		{
			CheatPanelSettings cheatPanelSettings = CalcInterpolatedPanelSettings();

			GUIUtils.PushSkin(_skin);
			GUIUtils.PushFontSize(PanelFontSize);

			return cheatPanelSettings;
		}

		public void PopPanelStyle()
		{
			GUIUtils.PopFontSize();
			GUIUtils.PopSkin();
		}

		public void PushButtonStyle()
		{
			GUIUtils.PushSkin(_skin);
			GUIUtils.PushFontSize(ButtonFontSize);
		}

		public void PopButtonStyle()
		{
			GUIUtils.PopFontSize();
			GUIUtils.PopSkin();
		}


		private CheatPanelSettings CalcInterpolatedPanelSettings()
		{
			if (_cheatPanelSettings.IsNullOrEmpty())
			{
				return new CheatPanelSettings(0.5f, 0.5f, 0.5f);
			}

			CheatPanelSettings minSettings = _cheatPanelSettings[0];
			CheatPanelSettings maxSettings = _cheatPanelSettings[_cheatPanelSettings.Count - 1];
			float screenRatio = (float)Screen.width / Screen.height;
			foreach (CheatPanelSettings cheatPanelSettings in _cheatPanelSettings)
			{
				if (screenRatio < cheatPanelSettings.Ratio && cheatPanelSettings.Ratio > minSettings.Ratio)
				{
					minSettings = cheatPanelSettings;
				}

				if (screenRatio > cheatPanelSettings.Ratio && cheatPanelSettings.Ratio < maxSettings.Ratio)
				{
					maxSettings = cheatPanelSettings;
				}
			}

			float ratio = 1f;

			if (minSettings.Ratio != maxSettings.Ratio)
			{
				ratio = (screenRatio - minSettings.Ratio) / (maxSettings.Ratio - minSettings.Ratio);
			}

			CheatPanelSettings interpolatedSettings = new CheatPanelSettings(
				Mathf.Lerp(minSettings.LabelWidth, maxSettings.LabelWidth, ratio),
				Mathf.Lerp(minSettings.Width, maxSettings.Width, ratio),
				Mathf.Lerp(minSettings.Height, maxSettings.Height, ratio)
			);

			return interpolatedSettings;
		}
	}
}
