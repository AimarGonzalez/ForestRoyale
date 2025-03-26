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

		public Unit(CardData card)
		{
			Card = card;

			if (card is TroopCardData troopCard)
			{
				Health = troopCard.TroopProperties.HitPoints;
			}
			else if (card is BuildingCardData buildingCard)
			{
				Health = buildingCard.UnitStats.HitPoints;
			}
		}
	}
}

