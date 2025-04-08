using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.CardStats;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using ForestRoyale.Libs.ForestLib.Utils;
using Game.Scripts.Gameplay.Cards.CardStats;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CardCreatorWindow : EditorWindow
	{
		private const string CARDS_DIRECTORY = "Assets/Resources/Cards";
		private const string UNITS_DIRECTORY = "Assets/Resources/Troops";
		private const string SPELLS_DIRECTORY = "Assets/Resources/Spells";
		private const string BUILDINGS_DIRECTORY = "Assets/Resources/Buildings";
		private const string TOWERS_DIRECTORY = "Assets/Resources/Towers";

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
				CreateMinionsCard();
			}

			if (GUILayout.Button("Giant", GUILayout.Height(30)))
			{
				CreateGiantCard();
			}

			if (GUILayout.Button("Arrows", GUILayout.Height(30)))
			{
				CreateArrowsCard();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(10);
			if (GUILayout.Button("Create 10 Iconic Cards", GUILayout.Height(40)))
			{
				CreateIconicCards();
			}

			EditorGUILayout.Space(10);
			GUILayout.Label("Bulk Creation:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Create All Troops", GUILayout.Height(30)))
			{
				CreateAllTroops();
			}

			if (GUILayout.Button("Create All Spells", GUILayout.Height(30)))
			{
				CreateAllSpells();
			}

			if (GUILayout.Button("Create All Buildings", GUILayout.Height(30)))
			{
				CreateAllBuildings();
			}

			if (GUILayout.Button("Create All Towers", GUILayout.Height(30)))
			{
				CreateAllTowers();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void CreateMinionsCard()
		{
			AssetUtils.CreateDirectory(CARDS_DIRECTORY);

			var minionsSO = CreateMinionsUnit(UNITS_DIRECTORY);
			TroopCardData minionsCard = TroopCardData.Build(
				cardName: "Minions",
				description: "Three fast, unarmored flying attackers. Weak to arrows, fireballs, dragons, and anything else that targets air units.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitCount: 3,
				unitSO: minionsSO
			);

			string cardAssetPath = $"{CARDS_DIRECTORY}/Minions_Card.asset";
			AssetDatabase.CreateAsset(minionsCard, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Minions card created at {cardAssetPath}");
			Selection.activeObject = minionsCard;
		}

		private void CreateGiantCard()
		{
			var giantSO = CreateGiantUnit(UNITS_DIRECTORY);
			TroopCardData giantCard = TroopCardData.Build(
				cardName: "Giant",
				description: "Slow but durable, the Giant is a powerful tank that only targets buildings. He leads the charge while other troops deal damage.",
				portrait: null,
				elixirCost: 5,
				rarity: CardRarity.Rare,
				unitCount: 1,
				unitSO: giantSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Giant_Card.asset";
			AssetDatabase.CreateAsset(giantCard, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Giant card created at {cardAssetPath}");
			Selection.activeObject = giantCard;
		}

		private void CreateArrowsCard()
		{
			var spellSO = CreateArrowsSpell(SPELLS_DIRECTORY);
			SpellCardData arrowsCard = SpellCardData.Build(
				cardName: "Arrows",
				description: "Arrows shower a large area, dealing moderate area damage to both air and ground units. Effective against swarms of weak enemies.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				spellSO: spellSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Arrows_Card.asset";
			AssetDatabase.CreateAsset(arrowsCard, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log($"Arrows card created at {cardAssetPath}");
			Selection.activeObject = arrowsCard;
		}

		private void CreateFireballCard()
		{
			var spellSO = CreateFireballSpell(SPELLS_DIRECTORY);
			SpellCardData card = SpellCardData.Build(
				cardName: "Fireball",
				description: "Deals high damage in a medium radius. Effective against groups of enemies.",
				portrait: null,
				elixirCost: 4,
				rarity: CardRarity.Rare,
				spellSO: spellSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Fireball_Card.asset";
			AssetDatabase.CreateAsset(card, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}


		private void CreateIconicCards()
		{
			AssetUtils.CreateDirectory(CARDS_DIRECTORY);

			int createdCount = 0;

			// Create each iconic card, checking if it already exists
			if (!File.Exists($"{CARDS_DIRECTORY}/Minions_Card.asset"))
			{
				CreateMinionsCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Giant_Card.asset"))
			{
				CreateGiantCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Arrows_Card.asset"))
			{
				CreateArrowsCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Goblin_Card.asset"))
			{
				CreateGoblinCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Fireball_Card.asset"))
			{
				CreateFireballCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Knight_Card.asset"))
			{
				CreateKnightCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Musketeer_Card.asset"))
			{
				CreateMusketeerCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Skeleton_Card.asset"))
			{
				CreateSkeletonCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/Cannon_Card.asset"))
			{
				CreateCannonCard();
				createdCount++;
			}

			if (!File.Exists($"{CARDS_DIRECTORY}/HogRider_Card.asset"))
			{
				CreateHogRiderCard();
				createdCount++;
			}

			Debug.Log($"Created {createdCount} new iconic cards. Skipped {10 - createdCount} existing cards.");
		}

		private void CreateGoblinCard()
		{

			var goblinSO = CreateGoblinUnit(UNITS_DIRECTORY);
			TroopCardData card = TroopCardData.Build(
				cardName: "Goblin",
				description: "Fast, cheap and weak. Deploy in a swarm to overwhelm enemies.",
				portrait: null,
				elixirCost: 2,
				rarity: CardRarity.Common,
				unitCount: 3,
				unitSO: goblinSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Goblin_Card.asset";
			AssetDatabase.CreateAsset(card, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateKnightCard()
		{
			var knightSO = CreateKnightUnit(UNITS_DIRECTORY);
			TroopCardData card = TroopCardData.Build(
				cardName: "Knight",
				description: "A tough melee fighter. The Mustache says it all.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitCount: 1,
				unitSO: knightSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Knight_Card.asset";
			AssetDatabase.CreateAsset(card, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateMusketeerCard()
		{
			var musketeerSO = CreateMusketerUnit(UNITS_DIRECTORY);
			TroopCardData card = TroopCardData.Build(
				cardName: "Musketeer",
				description: "Don't be fooled by her delicate appearance. She can take out a tower from a mile away.",
				portrait: null,
				elixirCost: 4,
				rarity: CardRarity.Rare,
				unitCount: 1,
				unitSO: musketeerSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Musketeer_Card.asset";
			AssetDatabase.CreateAsset(card, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateSkeletonCard()
		{
			var skeletonSO = CreateSkeletonUnit(UNITS_DIRECTORY);
			TroopCardData card = TroopCardData.Build(
				cardName: "Skeleton",
				description: "Four fast, very weak melee fighters. Generate positive elixir trades.",
				portrait: null,
				elixirCost: 1,
				rarity: CardRarity.Common,
				unitCount: 4,
				unitSO: skeletonSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Skeleton_Card.asset";
			AssetDatabase.CreateAsset(card, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateCannonCard()
		{
			var cannonSO = CreateCannonUnit(UNITS_DIRECTORY);
			BuildingCardData card = BuildingCardData.Build(
				cardName: "Cannon",
				description: "Defensive building that attacks ground units. Cannot attack flying enemies.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitSO: cannonSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Cannon_Card.asset";
			AssetDatabase.CreateAsset(card, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CreateHogRiderCard()
		{
			var hogRiderSO = CreateHogRiderUnit(UNITS_DIRECTORY);
			TroopCardData card = TroopCardData.Build(
				cardName: "Hog Rider",
				description: "Fast unit that targets buildings. He jumps over rivers and can push other units aside.",
				portrait: null,
				elixirCost: 4,
				rarity: CardRarity.Rare,
				unitCount: 1,
				unitSO: hogRiderSO
			);

			string cardAssetPath = $"{CARDS_DIRECTORY}/HogRider_Card.asset";
			AssetDatabase.CreateAsset(card, cardAssetPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}


		#region Unit Creation Methods
		private UnitSO CreateMinionsUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Minions_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Troop,
				hitPoints: 190f,
				transport: TransportType.Air,
				movementSpeed: 3.0f
			);
			var combatStats = CombatStats.Build(
				damage: 103f,
				attackSpeed: 1.0f,
				attackRange: 2.0f,
				areaDamageRadius: 0f,
				sightRange: 2.5f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateGiantUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Giant_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Troop,
				hitPoints: 3344f,
				transport: TransportType.Ground,
				movementSpeed: 1.0f
			);
			var combatStats = CombatStats.Build(
				damage: 188f,
				attackSpeed: 1.5f,
				attackRange: 1.0f,
				areaDamageRadius: 0f,
				sightRange: 1.5f,
				targetPreference: new List<TroopType> { TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateGoblinUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Goblin_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Troop,
				hitPoints: 80f,
				transport: TransportType.Ground,
				movementSpeed: 3.0f
			);
			var combatStats = CombatStats.Build(
				damage: 50f,
				attackSpeed: 1.1f,
				attackRange: 0.5f,
				areaDamageRadius: 0f,
				sightRange: 5.5f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateKnightUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Knight_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Troop,
				hitPoints: 1450f,
				transport: TransportType.Ground,
				movementSpeed: 1.2f
			);
			var combatStats = CombatStats.Build(
				damage: 159f,
				attackSpeed: 1.2f,
				attackRange: 1.0f,
				areaDamageRadius: 0f,
				sightRange: 5.5f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateMusketerUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Musketeer_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Troop,
				hitPoints: 720f,
				transport: TransportType.Ground,
				movementSpeed: 1.5f
			);
			var combatStats = CombatStats.Build(
				damage: 176f,
				attackSpeed: 1.1f,
				attackRange: 6.0f,
				areaDamageRadius: 0f,
				sightRange: 7.5f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateHogRiderUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/HogRider_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Troop,
				hitPoints: 1696f,
				transport: TransportType.Ground,
				movementSpeed: 3.5f
			);
			var combatStats = CombatStats.Build(
				damage: 264f,
				attackSpeed: 1.6f,
				attackRange: 0.8f,
				areaDamageRadius: 0f,
				sightRange: 6.0f,
				targetPreference: new List<TroopType> { TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateCannonUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Cannon_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Building,
				hitPoints: 824f,
				transport: TransportType.Ground,
				movementSpeed: 0f
			);
			var combatStats = CombatStats.Build(
				damage: 127f,
				attackSpeed: 0.8f,
				attackRange: 5.5f,
				areaDamageRadius: 0f,
				sightRange: 7.0f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateSkeletonUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Skeleton_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.Troop,
				hitPoints: 100f,
				transport: TransportType.Ground,
				movementSpeed: 2.5f
			);
			var combatStats = CombatStats.Build(
				damage: 100f,
				attackSpeed: 1.0f,
				attackRange: 1.0f,
				areaDamageRadius: 0f,
				sightRange: 5.5f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building, TroopType.ArenaTower }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}
		#endregion // Unit Creation Methods

		#region Spell Creation Methods
		private SpellSO CreateArrowsSpell(string directory)
		{
			AssetUtils.CreateDirectory(directory);

			string assetPath = $"{directory}/Arrows_Spell.asset";
			var spellStats = SpellStats.Build(
				affectsAir: true,
				affectsGround: true,
				affectsBuildings: true,
				attributes: SpellAttributes.Damage | SpellAttributes.AreaEffect
			);

			return CreateOrLoadSpell(assetPath, spellStats);
		}

		private SpellSO CreateFireballSpell(string directory)
		{
			AssetUtils.CreateDirectory(directory);

			string assetPath = $"{directory}/Fireball_Spell.asset";
			var spellStats = SpellStats.Build(
				affectsAir: true,
				affectsGround: true,
				affectsBuildings: true,
				attributes: SpellAttributes.Damage | SpellAttributes.AreaEffect
			);

			return CreateOrLoadSpell(assetPath, spellStats);
		}
		#endregion // Spell Creation Methods

		#region Bulk Creation Methods
		private void CreateAllTroops()
		{
			var createdUnits = new List<UnitSO>
			{
				CreateMinionsUnit(UNITS_DIRECTORY),
				CreateGiantUnit(UNITS_DIRECTORY),
				CreateKnightUnit(UNITS_DIRECTORY),
				CreateMusketerUnit(UNITS_DIRECTORY),
				CreateHogRiderUnit(UNITS_DIRECTORY),
				CreateGoblinUnit(UNITS_DIRECTORY),
				CreateSkeletonUnit(UNITS_DIRECTORY),
			};

			int createdCount = createdUnits.Where(unit => unit != null).Count();

			if (createdCount > 0)
			{
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			Debug.Log($"Created {createdCount} unit ScriptableObjects");
		}

		private void CreateAllSpells()
		{
			var createdSpells = new List<SpellSO>
			{
				CreateArrowsSpell(SPELLS_DIRECTORY),
				CreateFireballSpell(SPELLS_DIRECTORY)
			};

			int createdCount = createdSpells.Where(spell => spell != null).Count();

			if (createdCount > 0)
			{
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			Debug.Log($"Created {createdCount} spell ScriptableObjects");
		}

		private void CreateAllBuildings()
		{
			AssetUtils.CreateDirectory(BUILDINGS_DIRECTORY);

			var createdBuildings = new List<UnitSO>
			{
				CreateCannonUnit(BUILDINGS_DIRECTORY)
			};

			int createdCount = createdBuildings.Where(building => building != null).Count();

			if (createdCount > 0)
			{
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			Debug.Log($"Created {createdCount} building ScriptableObjects");
		}

		private void CreateAllTowers()
		{
			AssetUtils.CreateDirectory(TOWERS_DIRECTORY);

			var createdTowers = new List<UnitSO>
			{
				CreatePrincessTower(TOWERS_DIRECTORY),
				CreateKingTower(TOWERS_DIRECTORY)
			};

			int createdCount = createdTowers.Where(tower => tower != null).Count();

			if (createdCount > 0)
			{
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			Debug.Log($"Created {createdCount} tower ScriptableObjects");
		}

		private UnitSO CreatePrincessTower(string directory)
		{
			string assetPath = $"{directory}/PrincessTower_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.ArenaTower,
				hitPoints: 2534f,
				transport: TransportType.Ground,
				movementSpeed: 0f
			);
			var combatStats = CombatStats.Build(
				damage: 90f,
				attackSpeed: 0.8f,
				attackRange: 7.5f,
				areaDamageRadius: 0f,
				sightRange: 8.5f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}

		private UnitSO CreateKingTower(string directory)
		{
			string assetPath = $"{directory}/KingTower_Unit.asset";
			var unitStats = UnitStats.Build(
				type: TroopType.ArenaTower,
				hitPoints: 4008f,
				transport: TransportType.Ground,
				movementSpeed: 0f
			);
			var combatStats = CombatStats.Build(
				damage: 120f,
				attackSpeed: 1.0f,
				attackRange: 7.0f,
				areaDamageRadius: 0f,
				sightRange: 8.0f,
				targetPreference: new List<TroopType> { TroopType.Troop, TroopType.Building }
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats);
		}
		#endregion

		#region Helper Methods
		private UnitSO CreateOrLoadUnit(string assetPath, UnitStats unitStats, CombatStats combatStats)
		{
			if (File.Exists(assetPath))
			{
				return AssetDatabase.LoadAssetAtPath<UnitSO>(assetPath);
			}

			var unitSO = UnitSO.Build(unitStats: unitStats, combatStats: combatStats);
			AssetDatabase.CreateAsset(unitSO, assetPath);
			return unitSO;
		}

		private SpellSO CreateOrLoadSpell(string assetPath, SpellStats spellStats)
		{
			if (File.Exists(assetPath))
			{
				return AssetDatabase.LoadAssetAtPath<SpellSO>(assetPath);
			}

			var spellSO = SpellSO.Build(spellStats: spellStats);
			AssetDatabase.CreateAsset(spellSO, assetPath);
			return spellSO;
		}
		#endregion // Helper Methods
	}
}