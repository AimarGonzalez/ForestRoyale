using ForestRoyale.Core.UI;
using UnityEngine;
using System;

namespace ForestRoyale.Gameplay.Settings
{
	[CreateAssetMenu(fileName = "CombatSettings", menuName = "ForestRoyale/Settings/CombatSettings", order = ToolConstants.SettingsMenuOrder)]
	public class
	CombatSettings : ScriptableObject
	{
		[Serializable]
		public class ProjectileSettings
		{
			[SerializeField]
			private float _initialOffset = 0.5f;

			[SerializeField]
			private float _hitDistance = 0.1f;

			public float InitialOffset => _initialOffset;
			public float HitDistance => _hitDistance;
		}

		[SerializeField]
		private ProjectileSettings _projectileSettings;

		public ProjectileSettings Projectile => _projectileSettings;
	}
}