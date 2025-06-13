using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Combat;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Core
{
	[ExecuteInEditMode]
	public class CheatsMenu : MonoBehaviour
	{
		// ------------------
		
		[SerializeField]
		private bool _showDebugPanel = false;

		// ------------------

		[Inject]
		private GameState _battleCheats;

		private CheatsStyleProvider CheatsStyleProvider => GetComponent<CheatsStyleProvider>();

		private void OnGUI()
		{
			if (_showDebugPanel)
			{
				DrawCheatPanel();
			}
			else
			{
				DrawOpenButton();
			}
		}

		private void DrawCheatPanel()
		{
			CheatPanelSettings panelSettings = CheatsStyleProvider.PushPanelStyle();
			
			GUILayoutUtils.LabelWidth = panelSettings.LabelWidth * panelSettings.Width * Screen.width;
			GUILayoutUtils.LabelHeight = GUI.skin.label.CalcHeight(new GUIContent("X"), 100);

			GUILayout.BeginArea(new Rect(10, 10, panelSettings.Width * Screen.width, panelSettings.Height * Screen.height), GUI.skin.box);
			GUILayout.BeginVertical();

			DrawSubPanels();

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("CLOSE"))
			{
				_showDebugPanel = false;
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		
			CheatsStyleProvider.PopPanelStyle();
		}

		private void DrawOpenButton()
		{
			CheatsStyleProvider.PushButtonStyle();
			
			Vector2 buttonSize = new Vector2(Screen.width * 0.45f, Screen.height * 0.1f);
			float margin = 10;
			GUILayout.BeginArea(new Rect(margin, margin, buttonSize.x - margin, buttonSize.y - margin), GUI.skin.box);
			GUILayout.BeginVertical();

			if (GUILayout.Button("CHEATS", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
			{
				_showDebugPanel = true;
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
			
			CheatsStyleProvider.PopButtonStyle();
		}

		private void DrawSubPanels()
		{
			GUILayoutUtils.BeginVerticalBox(_battleCheats);
		}
	}
}
