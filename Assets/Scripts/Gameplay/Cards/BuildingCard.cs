using ForestRoyale.Gameplay.Cards.Components;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewBuilding", menuName = "ForestRoyale/Building Data", order = 1)]
	public class BuildingCard : CardData
	{
		public CombatStatsComponent _combatStats = new CombatStatsComponent();
		public DeployableComponent _deployable = new DeployableComponent();
	}
}