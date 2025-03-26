using UnityEngine;
using UnityEditor;
using System.IO;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CreateArrowsData : EditorWindow
	{
		[MenuItem("ForestRoyale/Create Arrows Data")]
		public static void CreateArrows()
		{
			// Create a new instance of the SpellCard ScriptableObject
			SpellCardData arrowsData = ScriptableObject.CreateInstance<SpellCardData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset
			string assetPath = $"{directory}/Arrows_Card.asset";
			AssetDatabase.CreateAsset(arrowsData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = arrowsData;

			// Initialize card data
			arrowsData.InitializeCardData(
				cardName: "Arrows",
				description: "Arrows shower a large area, dealing moderate area damage to both air and ground units. Effective against swarms of weak enemies.",
				portrait: null, // Portrait would be set manually
				elixirCost: 3,
				rarity: CardRarity.Common);

			// Initialize spell card data
			SpellStats spellEffects = new SpellStats();
			spellEffects.Initialize(
				affectsAir: true,
				affectsGround: true,
				affectsBuildings: true,
				attributes: SpellAttributes.Damage | SpellAttributes.AreaEffect);

			arrowsData.InitializeSpellCardData(spellEffects);

			// Save changes
			EditorUtility.SetDirty(arrowsData);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log("Arrows data created successfully at " + assetPath);
		}
	}
}