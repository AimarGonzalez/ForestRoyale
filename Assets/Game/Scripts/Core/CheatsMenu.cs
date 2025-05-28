using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using UnityEngine;


namespace ForestRoyale.Core
{
	[ExecuteInEditMode]
	public class CheatsMenu : MonoBehaviour
	{
		private const string PANEL_SETTINGS = "Panel settings";

		[BoxGroup(PANEL_SETTINGS), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[SerializeField] private int _fontSize = 12;
		[BoxGroup(PANEL_SETTINGS), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[SerializeField] private int _labelWidth = 100;
		[BoxGroup(PANEL_SETTINGS), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[SerializeField] private int _slinderHeigh = 10;

		[BoxGroup(PANEL_SETTINGS), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[SerializeField]
		[Range(0.01f, 1f)]
		private float _width = 0.5f;

		[BoxGroup(PANEL_SETTINGS), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[SerializeField]
		[Range(0.01f, 1f)]
		private float _height = 0.3f;

		private void OnGUI()
		{
			GUIUtils.PushFontSize(_fontSize);
			GUILayout.BeginArea(new Rect(10, 10, _width * Screen.width, _height * Screen.height), GUI.skin.box);
			DrawCheatsGUI();
			GUILayout.EndArea();
			GUIUtils.PopFontSize();
		}

		private void DrawCheatsGUI()
		{
			GUILayoutUtils.LabelWidth = _labelWidth;
			GUILayoutUtils.LabelHeight = GUI.skin.label.CalcHeight(new GUIContent("X"), 100);
			GUILayout.BeginVertical();
			{
				GUILayoutUtils.TextField("Font Size", _fontSize.ToString());
				_fontSize = GUILayoutUtils.Slider("Font Size", _fontSize, 12, 100, _slinderHeigh);
				GUILayoutUtils.TextField("Font Size", _fontSize.ToString());
				//_fontSize = GUILayoutUtils.Slider("Font Size", _fontSize, 12, 100, _slinderHeigh);
				GUILayoutUtils.TextField("Font Size", _fontSize.ToString());
			}
			GUILayout.EndVertical();
		}
	}
}
