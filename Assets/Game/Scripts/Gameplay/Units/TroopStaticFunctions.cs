using UnityEngine;
using UnityEngine.Assertions;

namespace ForestRoyale.Gameplay.Units
{
	public static class TroopStaticFunctions
	{
		private const int MAX_PARENT_DEPTH = 3;

		public static bool IsBodyCollider(this Collider other)
		{
			return other.gameObject.layer == LayerMask.NameToLayer("Default");
		}

		public static bool IsCombatCollider(this Collider other)
		{
			return other.gameObject.layer == LayerMask.NameToLayer("Combat");
		}

		public static TroopController GetTroopController(this Collider other)
		{
			var troopController = other.transform.GetComponentInParent<TroopController>();
			Assert.IsNotNull(troopController);
			return troopController;
		}

		public static TroopData GetTroopData(this Collider other)
		{
			var troopController = other.GetTroopController();
			return troopController != null ? troopController.TroopData : null;
		}

		private static T GetComponentInParent<T>(this Transform current, int maxDepth = MAX_PARENT_DEPTH) where T : Component
		{
			int currentDepth = 0;
			Transform parent = current;

			while (parent != null && currentDepth <= maxDepth)
			{
				if (parent.TryGetComponent(out T component))
				{
					return component;
				}

				parent = parent.parent;
				currentDepth++;
			}

			return null;
		}
	}
}