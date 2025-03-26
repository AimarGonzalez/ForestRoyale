using UnityEngine;
using UnityEditor;
using System.IO;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CreateGiantData : EditorWindow
	{
		[MenuItem("ForestRoyale/Create Giant Data")]
		public static void CreateGiant()
		{
			// Create a new instance of the TroopData ScriptableObject
			TroopData giantData = ScriptableObject.CreateInstance<TroopData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset
			string assetPath = $"{directory}/Giant.asset";
			AssetDatabase.CreateAsset(giantData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = giantData;

			// Get the serialized object to modify its values
			SerializedObject serializedObject = new SerializedObject(giantData);

			// Set character data based on Clash Royale Wiki
			SetFieldValue(serializedObject, "_cardName", "Giant");
			SetFieldValue(serializedObject, "_description",
				"Slow but durable, the Giant is a powerful tank that only targets buildings. He leads the charge while other troops deal damage.");
			// Portrait would be set manually
			SetFieldValue(serializedObject, "_elixirCost", 5);
			SetFieldValue(serializedObject, "_rarity", CardRarity.Rare);
			SetFieldValue(serializedObject, "_arenaUnlock", 0); // Available in Training Camp

			// Stats values based on level 9 (tournament standard)
			SetFieldValue(serializedObject, "_unitCount", 1);
			SetFieldValue(serializedObject, "_hitPoints", 3344f); // HP at level 9
			SetFieldValue(serializedObject, "_damage", 188f); // Damage per hit at level 9 
			SetFieldValue(serializedObject, "_attackSpeed", 1.5f); // Attacks every 1.5 seconds
			SetFieldValue(serializedObject, "_movementSpeed", 1.0f); // Slow movement
			SetFieldValue(serializedObject, "_attackRange", 1.0f); // Melee range
			SetFieldValue(serializedObject, "_areaDamageRadius", 0f); // Single target damage
			SetFieldValue(serializedObject, "_deploymentTime", 1.0f); // Standard deployment time

			// Attributes
			SetFieldValue(serializedObject, "_canTargetAir", false);
			SetFieldValue(serializedObject, "_isAirUnit", false);
			SetFieldValue(serializedObject, "_canTargetGround", true);
			SetFieldValue(serializedObject, "_hasArmor", false);
			SetFieldValue(serializedObject, "_isMelee", true);
			SetFieldValue(serializedObject, "_targetPreference", FigureTargetPreference.Buildings);

			SetFieldValue(serializedObject, "_status", DevelopmentStatus.Ready);

			// Apply the changes
			serializedObject.ApplyModifiedProperties();

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log("Giant data created successfully at " + assetPath);
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
						if (value is System.Enum)
						{
							property.enumValueIndex = System.Convert.ToInt32(value);
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