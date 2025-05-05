using ForestLib.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviors;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace ForestRoyale.Gameplay.Units
{
	public static class TroopStaticFunctions
	{
		private static readonly int DEFAULT_LAYER = LayerMask.NameToLayer("Default");
		private static readonly int COMBAT_LAYER = LayerMask.NameToLayer("Combat");

		public static bool IsBodyCollider(this Collider2D other)
		{
			return other.gameObject.layer == DEFAULT_LAYER;
		}

		public static bool IsCombatCollider(this Collider2D other)
		{
			return other.gameObject.layer == COMBAT_LAYER;
		}

		public static UnitRoot GetUnitComponent(this Behaviour other)
		{
			UnitRoot unitRoot = other.transform.GetComponentInParent<UnitRoot>();

#if UNITY_EDITOR
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				Assert.IsNotNull(unitRoot);
			}
#endif
			
			return unitRoot;
		}

		public static Unit GetUnit(this Behaviour other)
		{
			UnitRoot unitRoot = other.GetUnitComponent();
			return unitRoot != null ? unitRoot.Unit : null;
		}
	}
}