using UnityEngine;
using UnityEditor;
using System.IO;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CreateGiantCard : EditorWindow
	{
		[MenuItem("ForestRoyale/Create Giant Card")]
		public static void CreateGiant()
		{
			// Create a new instance of the TroopCardData ScriptableObject
			TroopCardData giantCard = ScriptableObject.CreateInstance<TroopCardData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset
			string assetPath = $"{directory}/Giant_Card.asset";
			AssetDatabase.CreateAsset(giantCard, assetPath);

			// Select the asset in the project window
			Selection.activeObject = giantCard;

			// Initialize card data
			giantCard.InitializeCardData(
				cardName: "Giant",
				description: "Slow but durable, the Giant is a powerful tank that only targets buildings. He leads the charge while other troops deal damage.",
				portrait: null, // Portrait would be set manually
				elixirCost: 5,
				rarity: CardRarity.Rare);

			// Create and initialize combat stats
			CombatStats combatStats = new CombatStats();
			combatStats.Initialize(
				damage: 188f,        // Damage per hit at level 9
				attackSpeed: 1.5f,    // Attacks every 1.5 seconds
				attackRange: 1.0f,    // Melee range
				areaDamageRadius: 0f  // Single target damage
			);

			// Create and initialize troop properties
			TroopStats troopProperties = new TroopStats();
			troopProperties.Initialize(
				hitPoints: 3344f,
				isAirUnit: false,
				movementSpeed: 1.0f   // Slow movement
			);

			// Initialize troop card data with the component instances
			giantCard.InitializeTroopCardData(
				unitCount: 1,
				troopProperties: troopProperties,
				combatStats: combatStats);

			// Save changes
			EditorUtility.SetDirty(giantCard);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Giant data created successfully at {assetPath}");
		}
	}
}