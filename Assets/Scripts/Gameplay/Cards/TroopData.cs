using UnityEngine;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewTroop", menuName = "ForestRoyale/Troop Data", order = 1)]
	public class TroopData : CardData
	{
		[SerializeField] private CombatStatsComponent _combatStats = new CombatStatsComponent();
		[SerializeField] private TargetingComponent _targeting = new TargetingComponent();
		[SerializeField] private TroopPropertiesComponent _troopProperties = new TroopPropertiesComponent();

		// Public getters for component properties
		public float HitPoints => _combatStats.HitPoints;
		public float Damage => _combatStats.Damage;
		public float AttackSpeed => _combatStats.AttackSpeed;
		public float AttackRange => _combatStats.AttackRange;
		public float AreaDamageRadius => _combatStats.AreaDamageRadius;

		public bool CanTargetAir => _targeting.CanTargetAir;
		public bool CanTargetGround => _targeting.CanTargetGround;
		public FigureTargetPreference TargetingPreference => _targeting.TargetingPreference;

		public int UnitCount => _troopProperties.UnitCount;
		public bool IsAirUnit => _troopProperties.IsAirUnit;
		public bool HasArmor => _troopProperties.HasArmor;
		public bool IsMelee => _troopProperties.IsMelee;
		public float MovementSpeed => _troopProperties.MovementSpeed;
		public float DeploymentTime => _troopProperties.DeploymentTime;
	}
}