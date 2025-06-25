using ForestRoyale.Core.UI;
using UnityEngine;

namespace ForestRoyale.Gameplay.Settings
{
	[CreateAssetMenu(fileName = "CombatSettings", menuName = "ForestRoyale/Settings/CombatSettings", order = ToolConstants.SettingsMenuOrder)]
	public class CombatSettings : ScriptableObject
	{
		[SerializeField]
		[Min(0.0001f)]
		private float _hitDistance = 0.1f;

		public float HitDistance => _hitDistance;
	}
}