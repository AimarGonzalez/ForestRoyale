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
		private string _name;

		[SerializeField]
		private UnitType _type;

		[Tooltip("Base health points")]
		[SerializeField]
		private float _hitPoints;

		[Tooltip("Is this troop an air unit?")]
		[SerializeField]
		private TransportType _transport;

		[Tooltip("Speed of movement")]
		[SerializeField]
		private float _movementSpeed;

		[Tooltip("If true, the unit will not be destroyed when it dies")]
		[SerializeField]
		private bool _permanentCorpse;

		public string Name => _name;
		public UnitType UnitType => _type;
		public float HitPoints => _hitPoints;
		public TransportType Transport => _transport;
		public float MovementSpeed => _movementSpeed;
		public bool PermanentCorpse => _permanentCorpse;

#if UNITY_EDITOR
		public static UnitStats Build(string name, UnitType type, float hitPoints, TransportType transport, float movementSpeed, bool permanentCorpse)
		{
			UnitStats stats = new UnitStats();

			// Set TroopStats specific properties
			stats._name = name;
			stats._type = type;
			stats._hitPoints = hitPoints;
			stats._transport = transport;
			stats._movementSpeed = movementSpeed;
			stats._permanentCorpse = permanentCorpse;

			return stats;
		}
#endif
	}
}