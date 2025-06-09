using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using UnityEditor;
using UnityEngine;
using FilePathAttribute = UnityEditor.FilePathAttribute;

namespace ForestRoyale.Gameplay.UI
{
	[FilePath("Assets/Game/Settings/UISettings.asset", FilePathAttribute.Location.ProjectFolder)]
	[CreateAssetMenu(fileName = "UISettings", menuName = "Forest Royale/Settings/UISettings")]
	public class UISettings : ScriptableObject
	{
		[Serializable]
		public class HealthBarColors
		{
			[SerializeField]
			public Color BarColor;
			[SerializeField]
			public Color FrameColor;
		}

		[BoxGroup("Unit Colors")]
		[SerializeField]
		private HealthBarColors _allyHealthBarColors;

		[BoxGroup("Unit Colors")]
		[SerializeField]
		private HealthBarColors _enemyHealthBarColors;

		public HealthBarColors AllyHealthBarColors => _allyHealthBarColors;
		public HealthBarColors EnemyHealthBarColors => _enemyHealthBarColors;
	}

	public class GameSettingsWindow : OdinMenuEditorWindow
	{
		[MenuItem("ForestRoyale/Settings/Game Settings")]
		private static void OpenWindow()
		{
			GetWindow<GameSettingsWindow>("Game Settings").Show();
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();
			//tree.AddAssetAtPath("UI Settings", "Assets/Game/Settings/UISettings.asset");
			tree.AddAllAssetsAtPath("Settings", "Assets/Game/Settings", typeof(ScriptableObject), includeSubDirectories: true);
			return tree;
		}
	}
}
