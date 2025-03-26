using UnityEngine;
using UnityEditor;
using System.IO;
using ForestRoyale.Gameplay.Cards;

namespace ForestRoyale.Editor.Gameplay.Cards
{
	public class CreateArrowsData : EditorWindow
	{
		[MenuItem("ForestRoyale/Create Arrows Data")]
		public static void CreateArrows()
		{
			// Create a new instance of the SpellData ScriptableObject
			SpellData arrowsData = ScriptableObject.CreateInstance<SpellData>();

			// Create the directory if it doesn't exist
			string directory = "Assets/Resources/Cards";
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Generate the asset
			string assetPath = $"{directory}/Arrows.asset";
			AssetDatabase.CreateAsset(arrowsData, assetPath);

			// Select the asset in the project window
			Selection.activeObject = arrowsData;

			// Get the serialized object to modify its values
			SerializedObject serializedObject = new SerializedObject(arrowsData);

			// Set card data based on Clash Royale Wiki
			SetFieldValue(serializedObject, "_cardName", "Arrows");
			SetFieldValue(serializedObject, "_description",
				"Arrows shower a large area, dealing moderate area damage to both air and ground units. Effective against swarms of weak enemies.");
			// Portrait would be set manually
			SetFieldValue(serializedObject, "_elixirCost", 3);
			SetFieldValue(serializedObject, "_rarity", CardRarity.Common);
			SetFieldValue(serializedObject, "_arenaUnlock", 0); // Available in Training Camp

			// Stats values based on level 9 (tournament standard)
			SetFieldValue(serializedObject, "_damage", 162f); // Damage at level 9
			SetFieldValue(serializedObject, "_radius", 4.0f); // 4 tile radius
			SetFieldValue(serializedObject, "_duration", 0f); // Instant effect, no duration

			// Attributes
			SetFieldValue(serializedObject, "_affectsAir", true);
			SetFieldValue(serializedObject, "_affectsGround", true);
			SetFieldValue(serializedObject, "_affectsBuildings", true);

			SetFieldValue(serializedObject, "_status", DevelopmentStatus.Ready);

			// Apply the changes
			serializedObject.ApplyModifiedProperties();

			// Save changes
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			Debug.Log("Arrows data created successfully at " + assetPath);
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
							property.enumValueIndex = System.Convert.ToInt32(value);
						else
							property.enumValueIndex = (int)value;
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