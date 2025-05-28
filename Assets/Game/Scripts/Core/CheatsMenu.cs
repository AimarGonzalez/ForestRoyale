using ForestRoyale.Gameplay.Combat;
using UnityEngine;

namespace ForestRoyale
{
	public class BattleManager : MonoBehaviour
	{
		[SerializeField] private Battle _battle;

		private void OnGUI()
		{
			
			GUILayout.BeginArea(new Rect(10, 10, 300, 100));
			DrawCheatsGUI();
			GUILayout.EndArea();
		}

		private static void DrawCheatsGUI()
		{
			GUILayout.BeginVertical();


			GUILayout.EndVertical();
		}
	}
}
