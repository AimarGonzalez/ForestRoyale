using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards.Components
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
		public void Initialize(float hitPoints, bool isAirUnit, float movementSpeed)
		{
			InitializeUnitCard(hitPoints);
			_isAirUnit = isAirUnit;
			_movementSpeed = movementSpeed;
		}
#endif
	}
}