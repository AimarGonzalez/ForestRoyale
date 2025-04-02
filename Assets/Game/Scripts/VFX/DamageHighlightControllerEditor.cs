using ForestRoyale.VFX;
using UnityEditor;
using UnityEngine;

namespace ForestRoyaleEditor.VFX
{
	[CustomEditor(typeof(DamageHighlightController))]
	public class DamageHighlightControllerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI(); // Draw the default inspector

			DamageHighlightController controller = (DamageHighlightController)target;

			if (GUILayout.Button("Flash Damage"))
			{
				controller.FlashDamage();
			}
		}
	}
}