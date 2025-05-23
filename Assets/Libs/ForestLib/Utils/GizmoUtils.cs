using UnityEngine;

namespace ForestLib.Utils
{
	public static class GizmoUtils
	{
		public static void DrawHorizontalLineOnScreen(float lineY, Color color, Camera camera = null)
		{
			camera ??= Camera.main;

			if (camera == null)
			{
				Debug.LogWarning("Can't draw gizmo - No camera found!");
				return;
			}

			float screenWidth =  camera.pixelWidth;

			Vector3 originScreenSpace = new Vector3(0, lineY, 10);
			Vector3 destinationScreenSpace = new Vector3(screenWidth, lineY, 10);
			Vector3 originWorldSpace = camera.ScreenToWorldPoint(originScreenSpace);
			Vector3 destinationWorldSpace = camera.ScreenToWorldPoint(destinationScreenSpace);
			Debug.DrawLine(originWorldSpace, destinationWorldSpace, color);
		}		
	}
}
