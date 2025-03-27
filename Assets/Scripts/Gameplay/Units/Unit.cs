using ForestRoyale.Gameplay.Cards;
using UnityEngine;


namespace ForestRoyale.Gameplay.Units
{
	public class Unit
	{
		public float RemainingHealth;

		public bool IsPlayerTeam;
		public bool IsForestTeam => !IsPlayerTeam;

		public FighterComponent Fighter;

		public readonly UnitStats UnitStats;

		public float MaxHealth => UnitStats.HitPoints;

		public Unit(IUnitCard card) : this(card.UnitStats)
		{
		}

		public Unit(UnitStats unitStats)
		{
			RemainingHealth = unitStats.HitPoints;
		}

	}
}

