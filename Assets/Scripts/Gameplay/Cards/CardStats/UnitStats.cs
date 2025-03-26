using UnityEngine;
using Raven.Attributes;
using System;

namespace ForestRoyale.Gameplay.Cards
{
	[Serializable]
	public class UnitStats
	{
		[BoxGroup("Combat Stats")]
		[Tooltip("Base health points")]
		[SerializeField]
		private float _hitPoints;

		public float HitPoints => _hitPoints;

#if UNITY_EDITOR
		public void InitializeUnitCard(float hitPoints)
		{
			_hitPoints = hitPoints;
		}
#endif
	}
}

