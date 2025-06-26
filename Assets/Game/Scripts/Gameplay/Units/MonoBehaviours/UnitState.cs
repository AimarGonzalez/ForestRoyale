using ForestLib.ExtensionMethods;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	[Flags]
	public enum UnitState
	{
		None = 0,
		[LabelText(SdfIconType.CloudDownloadFill, IconColor = "blue")]  CastingPreview = 1 << 0,
		[LabelText(SdfIconType.PauseCircleFill, IconColor = "green")]  Idle = 1 << 1,
		[LabelText(SdfIconType.ArrowRightCircleFill, IconColor = "green")]	MovingToTarget = 1 << 2,
		[LabelText(SdfIconType.ExclamationCircleFill, IconColor = "red")]	Attacking = 1 << 3,
		[LabelText(SdfIconType.XCircleFill, IconColor = "purple")]	Dying = 1 << 4,
		[LabelText(SdfIconType.XCircleFill, IconColor = "purple")]	Dead = 1 << 5,
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