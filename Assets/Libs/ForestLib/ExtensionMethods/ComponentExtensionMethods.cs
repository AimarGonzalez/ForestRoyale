using UnityEngine;

namespace ForestRoyale.Gameplay.Units
{
	public static class ComponentExtensionMethods
	{
		public const int MAX_PARENT_DEPTH = 3;

		public static T GetComponentInParentRecursive<T>(this Transform current, int maxDepth = MAX_PARENT_DEPTH) where T : Component
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