using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using UnityEditor;
using UnityEngine;
using FilePathAttribute = UnityEditor.FilePathAttribute;

namespace ForestRoyale.Gameplay.UI
{
	[FilePath("Assets/Game/Settings/UISettings.asset", FilePathAttribute.Location.ProjectFolder)]
	[CreateAssetMenu(fileName = "New Unit", menuName = "Forest Royale/Settings/UISettings")]
	public class UISettings : ScriptableSingleton<UISettings>
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
		[OnValueChanged(nameof(SaveMe))]
		private HealthBarColors _allyHealthBarColors;

		[BoxGroup("Unit Colors")]
		[SerializeField]
		[OnValueChanged(nameof(SaveMe))]
		private HealthBarColors _enemyHealthBarColors;

		public HealthBarColors AllyHealthBarColors => _allyHealthBarColors;
		public HealthBarColors EnemyHealthBarColors => _enemyHealthBarColors;

		private void SaveMe()
		{
			Save(true);
		}
	}

	public class UISettingsWindow : OdinEditorWindow
	{
		[MenuItem("ForestRoyale/Settings/UI Settings")]
		private static void OpenWindow()
		{
			GetWindow<UISettingsWindow>("UI Settings").Show();
		}

		protected override object GetTarget()
		{
			return UISettings.instance;
		}
	}
}
