using UnityEngine;
using Raven.Attributes;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewSpell", menuName = "ForestRoyale/Spell Data", order = 2)]
	public class SpellData : CardData
	{
		[SerializeField] private CombatStatsComponent _combatStats = new CombatStatsComponent();
		[SerializeField] private SpellEffectComponent _spellEffects = new SpellEffectComponent();

		// Public getters for component properties
		public float Damage => _combatStats.Damage;
		public float Radius => _combatStats.AreaDamageRadius;
		public float Duration => _combatStats.Duration;

		public bool AffectsAir => _spellEffects.AffectsAir;
		public bool AffectsGround => _spellEffects.AffectsGround;
		public bool AffectsBuildings => _spellEffects.AffectsBuildings;
	}
}