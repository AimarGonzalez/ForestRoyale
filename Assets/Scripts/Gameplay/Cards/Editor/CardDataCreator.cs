using UnityEngine;
using UnityEditor;
using System.IO;
using ForestRoyale.Gameplay.Cards;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CardDataCreator : EditorWindow
	{
		[MenuItem("ForestRoyale/Card Creator")]
		public static void ShowWindow()
		{
			GetWindow<CardDataCreator>("Card Creator");
		}

		private void OnGUI()
		{
			GUILayout.Label("Forest Royale Card Creator", EditorStyles.boldLabel);
			EditorGUILayout.Space(10);

			GUILayout.Label("Create predefined cards from Clash Royale:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Minions", GUILayout.Height(30)))
			{
				CreateMinionsData.CreateMinions();
			}

			if (GUILayout.Button("Giant", GUILayout.Height(30)))
			{
				CreateGiantData.CreateGiant();
			}

			if (GUILayout.Button("Arrows", GUILayout.Height(30)))
			{
				CreateArrowsData.CreateArrows();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(20);
			GUILayout.Label("Create empty card template:", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("New Troop", GUILayout.Height(30)))
			{
				CreateEmptyTroop();
			}

			if (GUILayout.Button("New Building", GUILayout.Height(30)))
			{
				CreateEmptyBuilding();
			}

			if (GUILayout.Button("New Spell", GUILayout.Height(30)))
			{
				CreateEmptySpell();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void CreateEmptyTroop()
		{
			// Create a new instance of the TroopData ScriptableObject
			TroopData troopData = ScriptableObject.CreateInstance<TroopData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset with a timestamp to ensure uniqueness
			string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string assetPath = $"{directory}/NewTroop_{timestamp}.asset";
			AssetDatabase.CreateAsset(troopData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = troopData;

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log("Empty troop template created at " + assetPath);
		}

		private void CreateEmptyBuilding()
		{
			// Create a new instance of the BuildingData ScriptableObject
			BuildingData buildingData = ScriptableObject.CreateInstance<BuildingData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset with a timestamp to ensure uniqueness
			string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string assetPath = $"{directory}/NewBuilding_{timestamp}.asset";
			AssetDatabase.CreateAsset(buildingData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = buildingData;

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log("Empty building template created at " + assetPath);
		}

		private void CreateEmptySpell()
		{
			// Create a new instance of the SpellData ScriptableObject
			SpellData spellData = ScriptableObject.CreateInstance<SpellData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset with a timestamp to ensure uniqueness
			string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string assetPath = $"{directory}/NewSpell_{timestamp}.asset";
			AssetDatabase.CreateAsset(spellData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = spellData;

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log("Empty spell template created at " + assetPath);
		}
	}
}