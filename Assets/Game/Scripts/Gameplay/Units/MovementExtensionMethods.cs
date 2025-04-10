using ForestLib.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviors;
using UnityEngine;
using UnityEngine.Assertions;

namespace ForestRoyale.Gameplay.Units
{
	public static class TroopStaticFunctions
	{
		public static bool IsBodyCollider(this Collider other)
		{
			return other.gameObject.layer == LayerMask.NameToLayer("Default");
		}

		public static bool IsCombatCollider(this Collider other)
		{
			return other.gameObject.layer == LayerMask.NameToLayer("Combat");
		}

		public static UnitRoot GetUnitComponent(this Collider other)
		{
			UnitRoot unitRoot = other.transform.GetComponentInParentRecursive<UnitRoot>();
			Assert.IsNotNull(unitRoot);
			return unitRoot;
		}

		public static Unit GetUnit(this Collider other)
		{
			UnitRoot unitRoot = other.GetUnitComponent();
			return unitRoot != null ? unitRoot.Unit : null;
		}
	}
}