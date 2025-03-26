using ForestRoyale.Gameplay.Cards.Components;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewBuilding", menuName = "ForestRoyale/Building Data", order = 1)]
	public class BuildingData : CardData
	{
		[SerializeField] private CombatStatsComponent _combatStats = new CombatStatsComponent();
		[SerializeField] private TargetingComponent _targeting = new TargetingComponent();

		// Public getters for component properties
		public float HitPoints => _combatStats.HitPoints;
		public float Damage => _combatStats.Damage;
		public float AttackSpeed => _combatStats.AttackSpeed;
		public float AttackRange => _combatStats.AttackRange;
		public float AreaDamageRadius => _combatStats.AreaDamageRadius;
		public float Duration => _combatStats.Duration;

		public bool CanTargetAir => _targeting.CanTargetAir;
		public bool CanTargetGround => _targeting.CanTargetGround;
		public FigureTargetPreference TargetingPreference => _targeting.TargetingPreference;
	}
}