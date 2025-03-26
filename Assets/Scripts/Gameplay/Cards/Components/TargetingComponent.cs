using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards.Components
{
	[System.Serializable]
	public class TargetingComponent
	{
		[BoxGroup("Targeting")]
		[Tooltip("Can this unit attack air units?")]
		[SerializeField] private bool _canTargetAir;

		[BoxGroup("Targeting")]
		[Tooltip("Can this unit attack ground units?")]
		[SerializeField] private bool _canTargetGround = true;

		[BoxGroup("Targeting")]
		[Tooltip("What type of targets this unit prioritizes")]
		[SerializeField] private FigureTargetPreference _targetPreference = FigureTargetPreference.Any;

		// Public getters for properties
		public bool CanTargetAir => _canTargetAir;
		public bool CanTargetGround => _canTargetGround;
		public FigureTargetPreference TargetingPreference => _targetPreference;
	}

	public enum FigureTargetPreference
	{
		Any, // Target any unit in range
		Buildings, // Only target buildings
		Ground, // Prefer ground targets
		Air, // Prefer air targets
		Troops // Only target troops (not buildings)
	}
}