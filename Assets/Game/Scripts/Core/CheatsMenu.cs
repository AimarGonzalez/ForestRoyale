using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Combat;
using Sirenix.OdinInspector;
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

		[BoxGroup(PANEL_SETTINGS), PropertyOrder(DebugUI.Order)]
		[SerializeField] private int _fontSize = 12;
		[BoxGroup(PANEL_SETTINGS), PropertyOrder(DebugUI.Order)]
		[SerializeField] private float _labelWidth = 0.5f;
		[BoxGroup(PANEL_SETTINGS), PropertyOrder(DebugUI.Order)]
		[SerializeField] private int _slinderHeigh = 10;

		[BoxGroup(PANEL_SETTINGS), PropertyOrder(DebugUI.Order)]
		[SerializeField]
		[Range(0.01f, 1f)]
		private float _width = 0.5f;

		[BoxGroup(PANEL_SETTINGS), PropertyOrder(DebugUI.Order)]
		[SerializeField]
		[Range(0.01f, 1f)]
		private float _height = 0.3f;

		// ------------------

		[Inject]
		private TimeController _timeController;

		[Inject]
		private Battle _battle;

		private void OnGUI()
		{
			GUIUtils.PushSkin(_skin);
			GUIUtils.PushFontSize(_fontSize);

			GUILayoutUtils.LabelWidth = _labelWidth * _width * Screen.width;
			GUILayoutUtils.LabelHeight = GUI.skin.label.CalcHeight(new GUIContent("X"), 100);

			GUILayout.BeginArea(new Rect(10, 10, _width * Screen.width, _height * Screen.height), GUI.skin.box);
			DrawCheatsGUI();
			GUILayout.EndArea();

			GUIUtils.PopFontSize();
			GUIUtils.PopSkin();
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
