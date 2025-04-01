using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards.CardStats
{
	[System.Serializable]
	public class TroopStats : UnitStats
	{
		[BoxGroup("Troop Properties")]
		[Tooltip("Is this troop an air unit?")]
		[SerializeField]
		private bool _isAirUnit;

		[BoxGroup("Troop Properties")]
		[Tooltip("Speed of movement")]
		[SerializeField]
		private float _movementSpeed;

		public bool IsAirborn => _isAirUnit;
		public float MovementSpeed => _movementSpeed;

#if UNITY_EDITOR
		public static TroopStats Build(float hitPoints, bool isAirUnit, float movementSpeed)
		{
			TroopStats stats = new TroopStats();

			// Set base class properties
			UnitStats baseStats = UnitStats.Build(hitPoints);
			// Copy hitPoints from the baseStats to our instance
			typeof(UnitStats).GetField("_hitPoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(stats, baseStats.HitPoints);

			// Set TroopStats specific properties
			stats._isAirUnit = isAirUnit;
			stats._movementSpeed = movementSpeed;

			return stats;
		}
#endif
	}
}