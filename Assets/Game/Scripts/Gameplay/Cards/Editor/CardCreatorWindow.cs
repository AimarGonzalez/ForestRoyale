using ForestLib.Utils;
using ForestRoyale.Core.Pool;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.CardStats;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using Game.Scripts.Gameplay.Cards.CardStats;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ForestRoyale.Gameplay.Editor
{
	public class CardCreatorWindow : EditorWindow
	{
		private const string PREFABS_DIRECTORY = "Assets/Game/Prefabs/Characters";
		private const string CARDS_DIRECTORY = "Assets/Game/Data/Cards";
		private const string UNITS_DIRECTORY = "Assets/Game/Data/Units/Troops";
		private const string SPELLS_DIRECTORY = "Assets/Game/Data/Spells";
		private const string TOWERS_DIRECTORY = "Assets/Game/Data/Units/Towers";

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
			if (GUILayout.Button("Create All Cards", GUILayout.Height(40)))
			{
				CreateAllCards();
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
			var minionsSO = CreateMinionsUnit(UNITS_DIRECTORY);
			TroopCardData minionsCard = TroopCardData.Build(
				cardName: "Minion Horde",
				description: "Six fast, unarmored flying attackers. Three's a crowd, six is a horde!",
				portrait: null,
				elixirCost: 5,
				rarity: CardRarity.Common,
				unitCount: 6,
				unitSO: minionsSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
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


		private void CreateAllCards()
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

			if (!File.Exists($"{CARDS_DIRECTORY}/SkeletonArmy_Card.asset"))
			{
				CreateSkeletonArmyCard();
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

			if (!File.Exists($"{CARDS_DIRECTORY}/Archers_Card.asset"))
			{
				CreateArchersCard();
				createdCount++;
			}

			Debug.Log($"Created {createdCount} new iconic cards. Skipped {11 - createdCount} existing cards.");
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

		private void CreateSkeletonArmyCard()
		{
			var skeletonSO = CreateSkeletonUnit(UNITS_DIRECTORY);
			TroopCardData card = TroopCardData.Build(
				cardName: "Skeleton Army",
				description: "Spawns an army of Skeletons. Meet Larry and his friends Harry, Terry, Gerry, Mary, etc.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Epic,
				unitCount: 15,
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
			TroopCardData card = TroopCardData.Build(
				cardName: "Cannon",
				description: "Defensive building that attacks ground units. Cannot attack flying enemies.",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitCount: 1,
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
				cardName: "HogRider",
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

		private void CreateArchersCard()
		{
			var archersSO = CreateArchersUnit(UNITS_DIRECTORY);
			TroopCardData card = TroopCardData.Build(
				cardName: "Archers",
				description: "A pair of unarmored ranged attackers. They'll help you take down ground and air units, but you're on your own with color-coordinating your outfits!",
				portrait: null,
				elixirCost: 3,
				rarity: CardRarity.Common,
				unitCount: 2,
				unitSO: archersSO
			);

			AssetUtils.CreateDirectory(CARDS_DIRECTORY);
			string cardAssetPath = $"{CARDS_DIRECTORY}/Archers_Card.asset";
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
				name: "Minions",
				type: UnitType.Troop,
				hitPoints: 190f,
				transport: TransportType.Air,
				movementSpeed: 3.0f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Melee,
				damage: 103f,
				attackSpeed: 1.0f,
				attackRange: 2.0f,
				areaDamageRadius: 0f,
				sightRange: 2.5f,
				projectileSpeed: 0f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building, UnitType.ArenaTower }
			);

			var unitPrefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Minions"),
				projectilePrefab: null
			);

			return CreateOrLoadUnit(assetPath, unitStats, combatStats, unitPrefabs);
		}

		private UnitSO CreateGiantUnit(string directory)
		{
			// https://clashroyale.fandom.com/wiki/Giant (lvl15)
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Giant_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Giant",
				type: UnitType.Troop,
				hitPoints: 5944f,
				transport: TransportType.Ground,
				movementSpeed: 1.0f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Melee,
				damage: 368f,
				attackSpeed: 1.5f,
				attackRange: 1.0f,
				areaDamageRadius: 0f,
				sightRange: 1.5f,
				projectileSpeed: 0f,
				targetPreference: new List<UnitType> { UnitType.Building, UnitType.ArenaTower }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Giant"),
				projectilePrefab: null
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateGoblinUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Goblin_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Goblin",
				type: UnitType.Troop,
				hitPoints: 80f,
				transport: TransportType.Ground,
				movementSpeed: 3.0f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Melee,
				damage: 50f,
				attackSpeed: 1.1f,
				attackRange: 0.5f,
				areaDamageRadius: 0f,
				sightRange: 5.5f,
				projectileSpeed: 0f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building, UnitType.ArenaTower }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Goblin"),
				projectilePrefab: null
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateKnightUnit(string directory)
		{
			// https://clashroyale.fandom.com/wiki/Knight  (lvl15)
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Knight_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Knight",
				type: UnitType.Troop,
				hitPoints: 2566,
				transport: TransportType.Ground,
				movementSpeed: 1.2f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Melee,
				damage: 293,
				attackSpeed: 1.2f,
				attackRange: 0.6f,
				areaDamageRadius: 0f,
				sightRange: 5.5f,
				projectileSpeed: 0f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building, UnitType.ArenaTower }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Knight"),
				projectilePrefab: null
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateMusketerUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Musketeer_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Musketeer",
				type: UnitType.Troop,
				hitPoints: 720f,
				transport: TransportType.Ground,
				movementSpeed: 1.5f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Ranged,
				damage: 176f,
				attackSpeed: 1.1f,
				attackRange: 6.0f,
				areaDamageRadius: 0f,
				sightRange: 7.5f,
				projectileSpeed: 1000f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building, UnitType.ArenaTower }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Musketeer"),
				projectilePrefab: LoadProjectilePrefab("bullet")
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateHogRiderUnit(string directory)
		{
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/HogRider_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Hog Rider",
				type: UnitType.Troop,
				hitPoints: 1696f,
				transport: TransportType.Ground,
				movementSpeed: 3.5f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Melee,
				damage: 264f,
				attackSpeed: 1.6f,
				attackRange: 0.8f,
				areaDamageRadius: 0f,
				sightRange: 6.0f,
				projectileSpeed: 0f,
				targetPreference: new List<UnitType> { UnitType.Building, UnitType.ArenaTower }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("HogRider"),
				projectilePrefab: null
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateCannonUnit(string directory)
		{
			// https://clashroyale.fandom.com/wiki/Cannon
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Cannon_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Cannon",
				type: UnitType.Building,
				hitPoints: 322f,
				transport: TransportType.Ground,
				movementSpeed: 0f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Ranged,
				damage: 83f,
				attackSpeed: 1f,
				attackRange: 5.5f,
				areaDamageRadius: 0f,
				sightRange: 7.0f,
				projectileSpeed: 1000f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building, UnitType.ArenaTower }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Cannon"),
				projectilePrefab: LoadProjectilePrefab("cannonball")
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateSkeletonUnit(string directory)
		{
			// https://clashroyale.fandom.com/wiki/Skeletons (lvl 15)
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Skeleton_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Skeleton",
				type: UnitType.Troop,
				hitPoints: 119,
				transport: TransportType.Ground,
				movementSpeed: 2.5f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Melee,
				damage: 119,
				attackSpeed: 1.0f,
				attackRange: 0.2f,
				areaDamageRadius: 0f,
				sightRange: 5.5f,
				projectileSpeed: 0f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building, UnitType.ArenaTower }
			);
			UnitPrefabs prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Skeleton"),
				projectilePrefab: null
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateArchersUnit(string directory)
		{
			// https://clashroyale.fandom.com/wiki/Archers (lvl 15)
			AssetUtils.CreateDirectory(directory);
			string assetPath = $"{directory}/Archer_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "Archer",
				type: UnitType.Troop,
				hitPoints: 442,
				transport: TransportType.Ground,
				movementSpeed: 1.2f,
				permanentCorpse: false
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Ranged,
				damage: 156f,
				attackSpeed: 0.9f,
				attackRange: 5.0f,
				areaDamageRadius: 0f,
				sightRange: 6.0f,
				projectileSpeed: 600f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building, UnitType.ArenaTower }
			);

			UnitPrefabs prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("Archer"),
				projectilePrefab: LoadProjectilePrefab("arrow")
			);

			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
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
				CreateArchersUnit(UNITS_DIRECTORY)
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
			AssetUtils.CreateDirectory(UNITS_DIRECTORY);

			var createdBuildings = new List<UnitSO>
			{
				CreateCannonUnit(UNITS_DIRECTORY)
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
				name: "Princess Tower",
				type: UnitType.ArenaTower,
				hitPoints: 2534f,
				transport: TransportType.Ground,
				movementSpeed: 0f,
				permanentCorpse: true
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Ranged,
				damage: 90f,
				attackSpeed: 0.8f,
				attackRange: 7.5f,
				areaDamageRadius: 0f,
				sightRange: 8.5f,
				projectileSpeed: 1000f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("PrincessTower"),
				projectilePrefab: LoadProjectilePrefab("arrow")
			);
			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}

		private UnitSO CreateKingTower(string directory)
		{
			string assetPath = $"{directory}/KingTower_Unit.asset";
			var unitStats = UnitStats.Build(
				name: "King Tower",
				type: UnitType.ArenaTower,
				hitPoints: 4008f,
				transport: TransportType.Ground,
				movementSpeed: 0f,
				permanentCorpse: true
			);
			var combatStats = CombatStats.Build(
				attackType: AttackType.Ranged,
				damage: 120f,
				attackSpeed: 1.0f,
				attackRange: 7.0f,
				areaDamageRadius: 0f,
				sightRange: 8.0f,
				projectileSpeed: 1000f,
				targetPreference: new List<UnitType> { UnitType.Troop, UnitType.Building }
			);

			var prefabs = UnitPrefabs.Build(
				unitPrefab: LoadUnitPrefab("KingTower"),
				projectilePrefab: LoadProjectilePrefab("cannonball")
			);

			return CreateOrLoadUnit(assetPath, unitStats, combatStats, prefabs);
		}
		#endregion

		#region Helper Methods
		private UnitSO CreateOrLoadUnit(string assetPath, UnitStats unitStats, CombatStats combatStats, UnitPrefabs prefabs)
		{
			if (File.Exists(assetPath))
			{
				return AssetDatabase.LoadAssetAtPath<UnitSO>(assetPath);
			}

			var unitSO = UnitSO.Build(unitStats: unitStats, combatStats: combatStats, unitPrefabs: prefabs);
			AssetDatabase.CreateAsset(unitSO, assetPath);
			return unitSO;
		}

		private UnitSO CreateOrLoadUnit(string assetPath, UnitStats unitStats, CombatStats combatStats)
		{
			if (File.Exists(assetPath))
			{
				return AssetDatabase.LoadAssetAtPath<UnitSO>(assetPath);
			}

			var defaultPrefabs = UnitPrefabs.Build(
				unitPrefab: null,
				projectilePrefab: null
			);
			var unitSO = UnitSO.Build(unitStats: unitStats, combatStats: combatStats, unitPrefabs: defaultPrefabs);
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

		private PooledGameObject LoadUnitPrefab(string cardName)
		{
			string prefabName = $"ut_{cardName.ToLower()}";
			string prefabPath = $"{PREFABS_DIRECTORY}/{prefabName}.prefab";
			return AssetDatabase.LoadAssetAtPath<PooledGameObject>(prefabPath);
		}

		private PooledGameObject LoadSquadPrefab(string cardName)
		{
			string prefabName = $"spawnable_{cardName.ToLower()}";
			string prefabPath = $"{PREFABS_DIRECTORY}/{prefabName}.prefab";
			return AssetDatabase.LoadAssetAtPath<PooledGameObject>(prefabPath);
		}

		private PooledGameObject LoadProjectilePrefab(string cardName)
		{
			string prefabName = $"projectile_{cardName.ToLower()}";
			string prefabPath = $"{PREFABS_DIRECTORY}/{prefabName}.prefab";
			return AssetDatabase.LoadAssetAtPath<PooledGameObject>(prefabPath);
		}
		#endregion // Helper Methods
	}
}