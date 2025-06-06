using ForestLib.ExtensionMethods;
using System;
using System.Collections.Generic;

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
	
	public sealed class UnitStateFlagEqualityComparer : IEqualityComparer<UnitState>
	{
		public bool Equals(UnitState flags, UnitState testFlag)
		{
			return flags.HasAllFlags(testFlag);
		}

		public int GetHashCode(UnitState obj)
		{
			return 0;
		}
	}
}