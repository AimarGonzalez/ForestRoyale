using UnityEngine;

namespace ForestRoyale.VFX
{
	public class ColorChanger : MonoBehaviour
	{
		private Renderer meshRenderer;
		private Color[] colors = {Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan};
		private int currentColorIndex = 0;

		void Start()
		{
			meshRenderer = GetComponent<Renderer>();
			if (meshRenderer == null)
			{
				Debug.LogError("No Renderer found on the GameObject.");
			}
		}

		void OnMouseDown()
		{
			if (meshRenderer != null)
			{
				currentColorIndex = (currentColorIndex + 1) % colors.Length;
				meshRenderer.material.color = colors[currentColorIndex];
			}
		}
	}
}