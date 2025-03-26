using System;
using System.IO;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.Components;
using UnityEditor;
using UnityEngine;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CreateMinionsData : EditorWindow
	{
		[MenuItem("ForestRoyale/Create Minions Data")]
		public static void CreateMinions()
		{
			// Create a new instance of the TroopData ScriptableObject
			TroopData minionsData = CreateInstance<TroopData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset
			string assetPath = $"{directory}/Minions.asset";
			AssetDatabase.CreateAsset(minionsData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = minionsData;

			// Get the serialized object to modify its values
			SerializedObject serializedObject = new SerializedObject(minionsData);

			// Set card data based on Clash Royale Wiki
			SetFieldValue(serializedObject, "_cardName", "Minions");
			SetFieldValue(serializedObject, "_description",
				"Three fast, unarmored flying attackers. Weak to arrows, fireballs, dragons, and anything else that targets air units.");
			// Portrait would be set manually
			SetFieldValue(serializedObject, "_elixirCost", 3);
			SetFieldValue(serializedObject, "_rarity", CardRarity.Common);
			SetFieldValue(serializedObject, "_arenaUnlock", 0); // Available in Training Camp

			// Stats values based on level 9 (tournament standard)
			SetFieldValue(serializedObject, "_unitCount", 3);
			SetFieldValue(serializedObject, "_hitPoints", 190f); // HP at level 9
			SetFieldValue(serializedObject, "_damage", 103f); // Damage per hit at level 9 
			SetFieldValue(serializedObject, "_attackSpeed", 1.0f); // Attacks every 1 second
			SetFieldValue(serializedObject, "_movementSpeed", 3.0f); // Fast movement
			SetFieldValue(serializedObject, "_attackRange", 2.0f); // Short range
			SetFieldValue(serializedObject, "_areaDamageRadius", 0f); // Single target damage
			SetFieldValue(serializedObject, "_deploymentTime", 1.0f); // Standard deployment time

			// Attributes
			SetFieldValue(serializedObject, "_canTargetAir", true);
			SetFieldValue(serializedObject, "_isAirUnit", true);
			SetFieldValue(serializedObject, "_canTargetGround", true);
			SetFieldValue(serializedObject, "_hasArmor", false);
			SetFieldValue(serializedObject, "_isMelee", false);
			SetFieldValue(serializedObject, "_targetPreference", FigureTargetPreference.Any);

			SetFieldValue(serializedObject, "_status", DevelopmentStatus.Ready);

			// Apply the changes
			serializedObject.ApplyModifiedProperties();

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log("Minions data created successfully at " + assetPath);
		}

		private static void SetFieldValue(SerializedObject serializedObject, string fieldName, object value)
		{
			SerializedProperty property = serializedObject.FindProperty(fieldName);
			if (property != null)
			{
				switch (property.propertyType)
				{
					case SerializedPropertyType.Integer:
						property.intValue = (int)value;
						break;
					case SerializedPropertyType.Float:
						property.floatValue = (float)value;
						break;
					case SerializedPropertyType.Boolean:
						property.boolValue = (bool)value;
						break;
					case SerializedPropertyType.String:
						property.stringValue = (string)value;
						break;
					case SerializedPropertyType.Enum:
						if (value is Enum)
						{
							property.enumValueIndex = Convert.ToInt32(value);
						}
						else
						{
							property.enumValueIndex = (int)value;
						}

						break;
					default:
						Debug.LogWarning($"Unsupported property type for {fieldName}");
						break;
				}
			}
			else
			{
				Debug.LogWarning($"Property {fieldName} not found");
			}
		}
	}
}