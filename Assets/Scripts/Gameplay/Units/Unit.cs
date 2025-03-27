using ForestRoyale.Gameplay.Cards;
using UnityEngine;


namespace ForestRoyale.Gameplay.Units
{
	public class Unit
	{
		public float Health;
		public Vector3 Position;

		public bool IsPlayerTeam;
		public bool IsForestTeam => !IsPlayerTeam;

		public FighterComponent Fighter;

		public readonly UnitStats UnitStats;

		public Unit(IUnitCard card) : this(card.UnitStats)
		{
		}

		public Unit(UnitStats unitStats)
		{
			Health = unitStats.HitPoints;
		}
	}
}

