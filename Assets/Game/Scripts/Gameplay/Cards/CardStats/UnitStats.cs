using Game.Scripts.Gameplay.Cards.CardStats;
using UnityEngine;
using Raven.Attributes;
using UnityEngine.Serialization;

namespace ForestRoyale.Gameplay.Cards.CardStats
{
	[System.Serializable]
	public class UnitStats
	{
		private TroopType _type;
		
		[BoxGroup("Combat Stats")]
		[Tooltip("Base health points")]
		[SerializeField]
		private float _hitPoints;

		[BoxGroup("Troop Properties")]
		[Tooltip("Is this troop an air unit?")]
		[SerializeField]
		private TransportType _transport;

		[BoxGroup("Troop Properties")]
		[Tooltip("Speed of movement")]
		[SerializeField]
		private float _movementSpeed;

		public float HitPoints => _hitPoints;
		public TransportType Transport => _transport;
		public float MovementSpeed => _movementSpeed;

#if UNITY_EDITOR
		public static UnitStats Build(TroopType type, float hitPoints, TransportType transport, float movementSpeed)
		{
			UnitStats stats = new UnitStats();

			// Set TroopStats specific properties
			stats._type = type;
			stats._transport = transport;
			stats._movementSpeed = movementSpeed;

			return stats;
		}
#endif
	}
}