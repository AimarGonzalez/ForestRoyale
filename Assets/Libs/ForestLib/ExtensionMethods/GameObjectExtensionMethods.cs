using UnityEngine;

namespace ForestLib.ExtensionMethods
{
	public static class GameObjectExtensionMethods
	{
		public static bool HasComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.GetComponent<T>() != null;
		}

		public static bool HasComponent<T>(this Component component) where T : Component
		{
			return component.GetComponent<T>() != null;
		}

		/// <summary>
		/// Genetrates string with the full path to the given object in the scene hierarchy.
		/// <br/>format: <c>"{Scene}/{Ancestor_1}/.../{Ancestor_N}/{CurrentGameObject}"</c>
		/// <br/>example: <c>"Main_Menu/Root/UI/Bottom/HorizontalBox/PlayerIcon"</c>
		/// </summary>
		public static string GetPathInHierarchy(this Component behaviour)
		{
			if (behaviour == null)
			{
				return string.Empty;
			}

			return GetPathInHierarchy(behaviour.gameObject);
		}

		/// <summary>
		/// Genetrates string with the full path to the given object in the scene hierarchy.
		/// <br/>format: <c>"{Scene}/{Ancestor_1}/.../{Ancestor_N}/{CurrentGameObject}"</c>
		/// <br/>example: <c>"Main_Menu/Root/UI/Bottom/HorizontalBox/PlayerIcon"</c>
		/// </summary>
		public static string GetPathInHierarchy(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return string.Empty;
			}

			if (gameObject.transform.parent == null)
			{
				return $"{gameObject.scene.name}/{gameObject.name}";
			}

			return $"{GetPathInHierarchy(gameObject.transform.parent.gameObject)}/{gameObject.name}";
		}

		public static bool TryGetComponentInChildren<T>(this Component source, out T component, bool includeInactive = false) where T : Component
		{
			return source.gameObject.TryGetComponentInChildren(out component, includeInactive);
		}

		public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool includeInactive = false) where T : Component
		{
			component = gameObject.GetComponentInChildren<T>(includeInactive);
			return component != null;
		}
	}
}