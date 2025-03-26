using UnityEngine;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewSpell", menuName = "ForestRoyale/Spell Data", order = 2)]
	public class SpellCard : CardData
	{
		[SerializeField]
		private SpellStats _spellEffects = new SpellStats();
		public SpellStats SpellEffects => _spellEffects;

#if UNITY_EDITOR
		public void InitializeSpellCardData(
			bool affectsAir,
			bool affectsGround,
			bool affectsBuildings,
			SpellAttributes attributes)
		{
			// Initialize the SpellEffectComponent
			_spellEffects.Initialize(
				affectsAir,
				affectsGround,
				affectsBuildings,
				attributes);
		}
#endif
	}
}