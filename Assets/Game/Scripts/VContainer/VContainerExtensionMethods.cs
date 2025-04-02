using ForestRoyale.Gameplay.Units.MonoBehaviors;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VContainer.Unity
{
	public static class VContainerExtensionMethods
	{
		[Obsolete("This is broken do not use - it registrates as Lifetime.Singleton, " +
		          "so it can't be used for multiple components." +
		          "I didn't find an API to pass an instance and a Lifetime")]
		public static void RegisterAllObjectsByType<T>(this IContainerBuilder builder) where T : UnityEngine.Object
		{
			T[] unitRoots = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
			foreach (T unitRoot in unitRoots)
			{
				builder.RegisterComponent(unitRoot);
			}
		}
	}
}