

using ForestRoyale.Gameplay.Cards.CardStats;

namespace ForestRoyale.Gameplay.Cards
{
	public interface IUnitCard
	{
		UnitStats UnitStats { get; }
		CombatStats CombatStats { get; }
	}
}
