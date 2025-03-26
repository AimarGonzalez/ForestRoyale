using System;
using System.IO;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.Components;
using UnityEditor;
using UnityEngine;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CreateMinionsCard : EditorWindow
	{
		[MenuItem("ForestRoyale/Create Minions Card")]
		public static void CreateMinions()
		{
			// Create a new instance of the TroopCardData ScriptableObject
			TroopCardData minionsCard = ScriptableObject.CreateInstance<TroopCardData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset
			string assetPath = $"{directory}/Minions_Card.asset";
			AssetDatabase.CreateAsset(minionsCard, assetPath);

			// Select the asset in the project window
			Selection.activeObject = minionsCard;

			// Initialize card data
			minionsCard.InitializeCardData(
				cardName: "Minions",
				description: "Three fast, unarmored flying attackers. Weak to arrows, fireballs, dragons, and anything else that targets air units.",
				portrait: null, // Portrait would be set manually
				elixirCost: 3,
				rarity: CardRarity.Common);

			CombatStats combatStats = new CombatStats();
			combatStats.Initialize(
				damage: 103f,         // Damage per hit at level 9
				attackSpeed: 1.0f,     // Attacks every 1 second
				attackRange: 2.0f,     // Short range
				areaDamageRadius: 0f   // Single target damage
			);

			TroopStats troopProperties = new TroopStats();
			troopProperties.Initialize(
				hitPoints: 190f,
				isAirUnit: true,
				movementSpeed: 3.0f    // Fast movement
			);

			minionsCard.InitializeTroopCardData(
				unitCount: 3,
				troopProperties: troopProperties,
				combatStats: combatStats);

			// Save changes
			EditorUtility.SetDirty(minionsCard);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Minions data created successfully at {assetPath}");
		}
	}
}