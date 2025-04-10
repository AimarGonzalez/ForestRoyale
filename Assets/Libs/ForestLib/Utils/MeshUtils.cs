using UnityEngine;

namespace ForestLib.Utils
{
	public static class MeshUtils
	{
		public static Bounds GetBoundingBox(Transform transform)
		{
			var bounds = new Bounds(transform.position, Vector3.zero);

			MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				bounds.Encapsulate(meshRenderer.bounds);
			}

			return bounds;
		}
	}
}
