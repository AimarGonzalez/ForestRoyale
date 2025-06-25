using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Cards.CardStats;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards.ScriptableObjects
{
	[CreateAssetMenu(fileName = "New Spell", menuName = "ForestRoyale/SpellSO", order = ToolConstants.RootMenuOrder)]
	public class SpellSO : ScriptableObject
	{
		[SerializeField]
		private SpellStats _spellStats;

		public SpellStats SpellStats => _spellStats;

#if UNITY_EDITOR
		public static SpellSO Build(SpellStats spellStats)
		{
			SpellSO spellSO = CreateInstance<SpellSO>();
			spellSO._spellStats = spellStats;

			return spellSO;
		}
#endif
	}
}
