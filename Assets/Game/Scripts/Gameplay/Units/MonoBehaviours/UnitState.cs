using System;

namespace ForestRoyale.Gameplay.Units
{
	[Flags]
	public enum UnitState
	{
		None = 0,
		CastingPreview = 1 << 0,
		Idle = 1 << 1,
		MovingToTarget = 1 << 2,
		Attacking = 1 << 3,
		Dying = 1 << 4,
		Dead = 1 << 5,
		All = CastingPreview | Idle | MovingToTarget | Attacking | Dying | Dead
	}
}