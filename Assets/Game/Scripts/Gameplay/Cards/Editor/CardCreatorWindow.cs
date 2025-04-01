using UnityEngine;
using UnityEditor;
using System.IO;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.CardStats;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CardCreatorWindow : EditorWindow
	{
		[MenuItem("ForestRoyale/Open Card Creator Window")]
		public static void ShowWindow()
		{
			GetWindow<CardCreatorWindow>("Card Creator");
		}

		private void OnGUI()
		{
			GUILayout.Label("Forest Royale Card Creator", EditorStyles.boldLabel);
			EditorGUILayout.Space(10);

			GUILayout.Label("Create predefined cards from Clash Royale:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Minions", GUILayout.Height(30)))
			{
				CreateMinions();
			}

			if (GUILayout.Button("Giant", GUILayout.Height(30)))
			{
				CreateGiant();
			}

			if (GUILayout.Button("Arrows", GUILayout.Height(30)))
			{
				CreateArrows();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(10);
			if (GUILayout.Button("Create 10 Iconic Cards", GUILayout.Height(40)))
			{
				CreateIconicCards();
			}

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

		private void CreateMinions()
		{
			// Create troop stats for minions
			TroopStats troopStats = TroopStats.Build(
				hitPoints: 190f,
				isAirUnit: true,
				movementSpeed: 3.0f
			);

			// Create combat stats for minions
			CombatStats combatStats = CombatStats.Build(
				damage: 103f,
				attackSpeed: 1.0f,
				attackRange: 2.0f,
				areaDamageRadius: 0f // Single target
			);

			// Create the minions card
			TroopCardData minionsCard = TroopCardData.Build(
				cardName: "Minions",
				description: "Three fast, unarmored flying attackers. Weak to arrows, fireballs, dragons, and anything else that targets air units.",
				portrait: null, // This would need to be loaded from Resources or AssetDatabase
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitCount: 3,
				troopStats: troopStats,
				combatStats: combatStats
			);

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Save the asset
			string assetPath = $"{directory}/Minions_Card.asset";
			AssetDatabase.CreateAsset(minionsCard, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Minions card created at {assetPath}");

			// Select the asset in the project window
			Selection.activeObject = minionsCard;
		}

		private void CreateGiant()
		{
			// Create troop stats for giant
			TroopStats troopStats = TroopStats.Build(
				hitPoints: 3344f,
				isAirUnit: false,
				movementSpeed: 1.0f
			);

			// Create combat stats for giant
			CombatStats combatStats = CombatStats.Build(
				damage: 188f,
				attackSpeed: 1.5f,
				attackRange: 1.0f,
				areaDamageRadius: 0f // Single target
			);

			// Create the giant card
			TroopCardData giantCard = TroopCardData.Build(
				cardName: "Giant",
				description: "Slow but durable, the Giant is a powerful tank that only targets buildings. He leads the charge while other troops deal damage.",
				portrait: null, // This would need to be loaded from Resources or AssetDatabase
				elixirCost: 5,
				rarity: CardRarity.Rare,
				unitCount: 1,
				troopStats: troopStats,
				combatStats: combatStats
			);

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Save the asset
			string assetPath = $"{directory}/Giant_Card.asset";
			AssetDatabase.CreateAsset(giantCard, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Giant card created at {assetPath}");

			// Select the asset in the project window
			Selection.activeObject = giantCard;
		}

		private void CreateArrows()
		{
			// Create spell stats for arrows
			SpellStats spellStats = SpellStats.Build(
				affectsAir: true,
				affectsGround: true,
				affectsBuildings: true,
				attributes: SpellAttributes.Damage | SpellAttributes.AreaEffect
			);

			// Create the arrows card
			SpellCardData arrowsCard = SpellCardData.Build(
				cardName: "Arrows",
				description: "Arrows shower a large area, dealing moderate area damage to both air and ground units. Effective against swarms of weak enemies.",
				portrait: null, // This would need to be loaded from Resources or AssetDatabase
				elixirCost: 3,
				rarity: CardRarity.Common,
				spellEffects: spellStats
			);

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Save the asset
			string assetPath = $"{directory}/Arrows_Card.asset";
			AssetDatabase.CreateAsset(arrowsCard, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Arrows card created at {assetPath}");

			// Select the asset in the project window
			Selection.activeObject = arrowsCard;
		}

		private void CreateIconicCards()
		{
			// Directory checks
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Create each iconic card, checking if it already exists
			int createdCount = 0;

			// 1. Minions (already have a creation method)
			if (!File.Exists($"{directory}/Minions_Card.asset"))
			{
				CreateMinions();
				createdCount++;
			}

			// 2. Giant (already have a creation method)
			if (!File.Exists($"{directory}/Giant_Card.asset"))
			{
				CreateGiant();
				createdCount++;
			}

			// 3. Arrows (already have a creation method)
			if (!File.Exists($"{directory}/Arrows_Card.asset"))
			{
				CreateArrows();
				createdCount++;
			}

			// 4. Goblin
			if (!File.Exists($"{directory}/Goblin_Card.asset"))
			{
				CreateGoblinCard();
				createdCount++;
			}

			// 5. Fireball
			if (!File.Exists($"{directory}/Fireball_Card.asset"))
			{
				CreateFireballCard();
				createdCount++;
			}

			// 6. Knight
			if (!File.Exists($"{directory}/Knight_Card.asset"))
			{
				CreateKnightCard();
				createdCount++;
			}

			// 7. Musketeer
			if (!File.Exists($"{directory}/Musketeer_Card.asset"))
			{
				CreateMusketeerCard();
				createdCount++;
			}

			// 8. Skeleton
			if (!File.Exists($"{directory}/Skeleton_Card.asset"))
			{
				CreateSkeletonCard();
				createdCount++;
			}

			// 9. Cannon
			if (!File.Exists($"{directory}/Cannon_Card.asset"))
			{
				CreateCannonCard();
				createdCount++;
			}

			// 10. Hog Rider
			if (!File.Exists($"{directory}/HogRider_Card.asset"))
			{
				CreateHogRiderCard();
				createdCount++;
			}

			Debug.Log($"Created {createdCount} new iconic cards. Skipped {10 - createdCount} existing cards.");
		}

		private void CreateGoblinCard()
		{
			TroopStats troopStats = TroopStats.Build(
				hitPoints: 80f,
				isAirUnit: false,
				movementSpeed: 3.0f
			);

			CombatStats combatStats = CombatStats.Build(
				damage: 50f,
				attackSpeed: 1.1f,
				attackRange: 0.5f,
				areaDamageRadius: 0f
			);

			TroopCardData card = TroopCardData.Build(
				cardName: "Goblin",
				description: "Fast, cheap and weak. Deploy in a swarm to overwhelm enemies.",
				portrait: null,
				elixirCost: 2,
				rarity: CardRarity.Common,
				unitCount: 3,
				troopStats: troopStats,
				combatStats: combatStats
			);

			string assetPath = "Assets/Resources/Cards/Goblin_Card.asset";
			AssetDatabase.CreateAsset(card, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateFireballCard()
		{
			SpellStats spellStats = SpellStats.Build(
				affectsAir: true,
				affectsGround: true,
				affectsBuildings: true,
				attributes: SpellAttributes.Damage | SpellAttributes.AreaEffect
			);

			SpellCardData card = SpellCardData.Build(
				cardName: "Fireball",
				description: "Deals high damage in a medium radius. Effective against groups of enemies.",
				portrait: null,
				elixirCost: 4,
				rarity: CardRarity.Rare,
				spellEffects: spellStats
			);

			string assetPath = "Assets/Resources/Cards/Fireball_Card.asset";
			AssetDatabase.CreateAsset(card, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateKnightCard()
		{
			TroopStats troopStats = TroopStats.Build(
				hitPoints: 1450f,
				isAirUnit: false,
				movementSpeed: 1.2f
			);

			CombatStats combatStats = CombatStats.Build(
				damage: 159f,
				attackSpeed: 1.2f,
				attackRange: 1.0f,
				areaDamageRadius: 0f
			);

			TroopCardData card = TroopCardData.Build(
				cardName: "Knight",
				description: "A tough melee fighter. The Mustache says it all.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitCount: 1,
				troopStats: troopStats,
				combatStats: combatStats
			);

			string assetPath = "Assets/Resources/Cards/Knight_Card.asset";
			AssetDatabase.CreateAsset(card, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateMusketeerCard()
		{
			TroopStats troopStats = TroopStats.Build(
				hitPoints: 720f,
				isAirUnit: false,
				movementSpeed: 1.5f
			);

			CombatStats combatStats = CombatStats.Build(
				damage: 176f,
				attackSpeed: 1.1f,
				attackRange: 6.0f,
				areaDamageRadius: 0f
			);

			TroopCardData card = TroopCardData.Build(
				cardName: "Musketeer",
				description: "Don't be fooled by her delicate appearance. She can take out a tower from a mile away.",
				portrait: null,
				elixirCost: 4,
				rarity: CardRarity.Rare,
				unitCount: 1,
				troopStats: troopStats,
				combatStats: combatStats
			);

			string assetPath = "Assets/Resources/Cards/Musketeer_Card.asset";
			AssetDatabase.CreateAsset(card, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateSkeletonCard()
		{
			TroopStats troopStats = TroopStats.Build(
				hitPoints: 32f,
				isAirUnit: false,
				movementSpeed: 2.5f
			);

			CombatStats combatStats = CombatStats.Build(
				damage: 32f,
				attackSpeed: 1.0f,
				attackRange: 0.5f,
				areaDamageRadius: 0f
			);

			TroopCardData card = TroopCardData.Build(
				cardName: "Skeleton",
				description: "Four fast, very weak melee fighters. Generate positive elixir trades.",
				portrait: null,
				elixirCost: 1,
				rarity: CardRarity.Common,
				unitCount: 4,
				troopStats: troopStats,
				combatStats: combatStats
			);

			string assetPath = "Assets/Resources/Cards/Skeleton_Card.asset";
			AssetDatabase.CreateAsset(card, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateCannonCard()
		{
			UnitStats unitStats = UnitStats.Build(
				hitPoints: 824f
			);

			CombatStats combatStats = CombatStats.Build(
				damage: 127f,
				attackSpeed: 0.8f,
				attackRange: 5.5f,
				areaDamageRadius: 0f
			);

			BuildingCardData card = BuildingCardData.Build(
				cardName: "Cannon",
				description: "Defensive building that attacks ground units. Cannot attack flying enemies.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitStats: unitStats,
				combatStats: combatStats
			);

			string assetPath = "Assets/Resources/Cards/Cannon_Card.asset";
			AssetDatabase.CreateAsset(card, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateHogRiderCard()
		{
			TroopStats troopStats = TroopStats.Build(
				hitPoints: 1696f,
				isAirUnit: false,
				movementSpeed: 3.5f
			);

			CombatStats combatStats = CombatStats.Build(
				damage: 264f,
				attackSpeed: 1.6f,
				attackRange: 0.8f,
				areaDamageRadius: 0f
			);

			TroopCardData card = TroopCardData.Build(
				cardName: "Hog Rider",
				description: "Fast unit that targets buildings. He jumps over rivers and can push other units aside.",
				portrait: null,
				elixirCost: 4,
				rarity: CardRarity.Rare,
				unitCount: 1,
				troopStats: troopStats,
				combatStats: combatStats
			);

			string assetPath = "Assets/Resources/Cards/HogRider_Card.asset";
			AssetDatabase.CreateAsset(card, assetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateEmptyTroop()
		{
			// Create a new instance of the TroopData ScriptableObject
			TroopCardData troopCard = ScriptableObject.CreateInstance<TroopCardData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate a unique asset name with incremental suffix
			string assetPath = GetUniqueAssetPath(directory, "NewTroop_Card");

			AssetDatabase.CreateAsset(troopCard, assetPath);

			// Select the asset in the project window
			Selection.activeObject = troopCard;

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Empty troop template created at {assetPath}");
		}

		private void CreateEmptyBuilding()
		{
			// Create a new instance of the BuildingData ScriptableObject
			BuildingCardData buildingCard = ScriptableObject.CreateInstance<BuildingCardData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate a unique asset name with incremental suffix
			string assetPath = GetUniqueAssetPath(directory, "NewBuilding_Card");

			AssetDatabase.CreateAsset(buildingCard, assetPath);

			// Select the asset in the project window
			Selection.activeObject = buildingCard;

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Empty building template created at {assetPath}");
		}

		private void CreateEmptySpell()
		{
			// Create a new instance of the SpellData ScriptableObject
			SpellCardData spellData = ScriptableObject.CreateInstance<SpellCardData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate a unique asset name with incremental suffix
			string assetPath = GetUniqueAssetPath(directory, "NewSpell_Card");

			AssetDatabase.CreateAsset(spellData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = spellData;

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Empty spell template created at {assetPath}");
		}

		private string GetUniqueAssetPath(string directory, string baseName)
		{
			string assetPath = $"{directory}/{baseName}.asset";
			int suffix = 1;

			// If the file already exists, add incremental suffix
			while (File.Exists(assetPath))
			{
				assetPath = $"{directory}/{baseName}_{suffix}.asset";
				suffix++;
			}

			return assetPath;
		}
	}
}