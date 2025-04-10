
using UnityEngine;

namespace ForestLib.Gameplay.Units
{
	public static class Vector3ExtensionMethods
	{
		public static float DistanceSquared(this Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(b - a);
		}
	}
}