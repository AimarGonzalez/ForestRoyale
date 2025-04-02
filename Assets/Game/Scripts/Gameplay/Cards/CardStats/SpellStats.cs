using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace ForestRoyale.Gameplay.Cards.CardStats
{
	[Serializable]
	public class SpellStats
	{
		[BoxGroup("Spell Effects")]
		[Tooltip("Can this spell affect air units?")]
		[SerializeField]
		private bool _affectsAir = true;

		[BoxGroup("Spell Effects")]
		[Tooltip("Can this spell affect ground units?")]
		[SerializeField]
		private bool _affectsGround = true;

		[BoxGroup("Spell Effects")]
		[Tooltip("Can this spell affect buildings?")]
		[SerializeField]
		private bool _affectsBuildings = true;

		[BoxGroup("Spell Effects")]
		[Tooltip("Special attributes that define spell behavior")]
		[SerializeField]
		private SpellAttributes _attributes;

		public bool AffectsAir => _affectsAir;
		public bool AffectsGround => _affectsGround;
		public bool AffectsBuildings => _affectsBuildings;
		public SpellAttributes Attributes => _attributes;

#if UNITY_EDITOR
		public static SpellStats Build(bool affectsAir, bool affectsGround, bool affectsBuildings, SpellAttributes attributes)
		{
			SpellStats stats = new SpellStats
			{
				_affectsAir = affectsAir,
				_affectsGround = affectsGround,
				_affectsBuildings = affectsBuildings,
				_attributes = attributes
			};
			return stats;
		}
#endif
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