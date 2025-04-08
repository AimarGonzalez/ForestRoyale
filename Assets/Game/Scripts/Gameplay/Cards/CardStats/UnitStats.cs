using Game.Scripts.Gameplay.Cards.CardStats;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards.CardStats
{
	[Serializable]
	public class UnitStats
	{
		[SerializeField]
		private TroopType _type;

		[BoxGroup("Troop Stats")]
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
			stats._hitPoints = hitPoints;
			stats._transport = transport;
			stats._movementSpeed = movementSpeed;

			return stats;
		}
#endif
	}
}