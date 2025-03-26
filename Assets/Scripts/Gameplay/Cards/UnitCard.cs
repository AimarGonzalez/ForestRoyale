using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards
{
	public class UnitCard : CardData
	{
		[BoxGroup("Combat Stats")]
		[Tooltip("Base health points")]
		[SerializeField]
		private float _hitPoints;

		public float HitPoints => _hitPoints;
	}
}

