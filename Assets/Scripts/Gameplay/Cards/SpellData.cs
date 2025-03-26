using UnityEngine;
using Raven.Attributes;
using ForestRoyale.Gameplay.Cards.Components;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewSpell", menuName = "ForestRoyale/Spell Data", order = 2)]
	public class SpellData : CardData
	{
		[SerializeField] private SpellEffectComponent _spellEffects = new SpellEffectComponent();
		public SpellEffectComponent SpellEffects => _spellEffects;

	}
}