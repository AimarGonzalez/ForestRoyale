using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards.Components
{
	[System.Serializable]
	public class TroopPropertiesComponent
	{
		[BoxGroup("Troop Properties")]
		[Tooltip("Number of units in this card")]
		[SerializeField] private int _unitCount = 1;

		[BoxGroup("Troop Properties")]
		[Tooltip("Is this troop an air unit?")]
		[SerializeField] private bool _isAirUnit;

		[BoxGroup("Troop Properties")]
		[Tooltip("Does this troop have armor?")]
		[SerializeField] private bool _hasArmor;

		[BoxGroup("Troop Properties")]
		[Tooltip("Is this troop a melee unit?")]
		[SerializeField] private bool _isMelee;

		[BoxGroup("Troop Properties")]
		[Tooltip("Speed of movement")]
		[SerializeField] private float _movementSpeed;

		[BoxGroup("Troop Properties")]
		[Tooltip("Deployment time in seconds")]
		[SerializeField] private float _deploymentTime = 1.0f;

		// Public getters for properties
		public int UnitCount => _unitCount;
		public bool IsAirUnit => _isAirUnit;
		public bool HasArmor => _hasArmor;
		public bool IsMelee => _isMelee;
		public float MovementSpeed => _movementSpeed;
		public float DeploymentTime => _deploymentTime;
	}
}