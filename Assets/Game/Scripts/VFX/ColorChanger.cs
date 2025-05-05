using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestRoyale.VFX
{
	[InfoBox("Debug only script:\n Changes the the material color every time the game object gets a mouse click. " +
	         "\n - Requires a collider component." +
	         "\n - Requires shader using with <i>\"_Color\"</i> property (or ShaderPropertyFlags.MainColor).")]
	public class ColorChanger : MonoBehaviour
	{
		private Renderer meshRenderer;
		private Color[] colors = {Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan};
		
		[ShowInInspector, ReadOnly]
		private int currentColorIndex = 0;

		[ShowInInspector, ReadOnly]

		private Color currentColor => colors[currentColorIndex];

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