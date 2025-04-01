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
		public static UnitStats Build(float hitPoints)
		{
			UnitStats stats = new UnitStats
			{
				_hitPoints = hitPoints
			};
			return stats;
		}
#endif
	}
}

