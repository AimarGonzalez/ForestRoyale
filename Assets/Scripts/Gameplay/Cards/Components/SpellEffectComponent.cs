using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards.Components
{
	[System.Serializable]
	public class SpellEffectComponent
	{
		[BoxGroup("Spell Effects")]
		[Tooltip("Can this spell affect air units?")]
		[SerializeField] private bool _affectsAir = true;

		[BoxGroup("Spell Effects")]
		[Tooltip("Can this spell affect ground units?")]
		[SerializeField] private bool _affectsGround = true;

		[BoxGroup("Spell Effects")]
		[Tooltip("Can this spell affect buildings?")]
		[SerializeField] private bool _affectsBuildings = true;

		[BoxGroup("Spell Effects")]
		[Tooltip("Special attributes that define spell behavior")]
		[SerializeField] private SpellAttributes _spellAttributes;

		// Public getters for properties
		public bool AffectsAir => _affectsAir;
		public bool AffectsGround => _affectsGround;
		public bool AffectsBuildings => _affectsBuildings;
		public SpellAttributes Attributes => _spellAttributes;
	}

	[System.Flags]
	public enum SpellAttributes
	{
		None = 0,
		Damage = 1 << 0, // Deals direct damage
		AreaEffect = 1 << 1, // Affects an area
		Slow = 1 << 2, // Slows units down
		Freeze = 1 << 3, // Freezes units completely
		Poison = 1 << 4, // Deals damage over time
		Push = 1 << 5, // Pushes units away
		Pull = 1 << 6, // Pulls units toward center
		Stun = 1 << 7, // Stuns units temporarily
		Buff = 1 << 8, // Buffs friendly units
		Debuff = 1 << 9, // Debuffs enemy units
		Spawner = 1 << 10, // Spawns units
		Transform = 1 << 11, // Transforms units into something else
		Heal = 1 << 12, // Heals friendly units
		Shield = 1 << 13, // Provides shield to friendly units
		Rage = 1 << 14 // Increases damage and speed
	}
}