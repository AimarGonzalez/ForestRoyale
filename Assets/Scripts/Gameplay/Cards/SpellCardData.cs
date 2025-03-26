using UnityEngine;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewSpell", menuName = "ForestRoyale/Spell Data", order = 2)]
	public class SpellCardData : CardData
	{
		[SerializeField]
		private SpellStats _spellEffects = new SpellStats();
		public SpellStats SpellEffects => _spellEffects;

#if UNITY_EDITOR
		public void InitializeSpellCardData(SpellStats spellEffects)
		{
			// Initialize the SpellEffectComponent
			_spellEffects = spellEffects;
		}
#endif
	}
}