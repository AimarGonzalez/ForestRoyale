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

		public readonly CardData Card;

		public Unit(UnitCard card)
		{
			Card = card;
			Health = card.HitPoints;
		}
	}
}

