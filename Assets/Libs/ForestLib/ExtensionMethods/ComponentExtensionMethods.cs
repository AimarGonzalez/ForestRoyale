using UnityEngine;

namespace ForestLib.ExtensionMethods
{
	public static class ComponentExtensionMethods
	{
		public static bool HasComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.GetComponent<T>() != null;
		}

		public static bool HasComponent<T>(this Component component) where T : Component
		{
			return component.GetComponent<T>() != null;
		}
	}
}