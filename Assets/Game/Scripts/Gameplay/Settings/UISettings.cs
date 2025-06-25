using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
#if UNITY_EDITOR
using FilePathAttribute = UnityEditor.FilePathAttribute;
#endif

namespace ForestRoyale.Gameplay.Settings
{
	
#if UNITY_EDITOR
	[FilePath("Assets/Game/Settings/UISettings.asset", FilePathAttribute.Location.ProjectFolder)]
#endif
	[CreateAssetMenu(fileName = "UISettings", menuName = "ForestRoyale/Settings/UISettings", order = ToolConstants.SettingsMenuOrder)]
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
}
