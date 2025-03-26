using ForestRoyale.Gameplay.Cards.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewBuilding", menuName = "ForestRoyale/Building Data", order = 1)]
	public class BuildingCard : UnitCard
	{
		public CombatStats CombatStats = new CombatStats();
	}
}