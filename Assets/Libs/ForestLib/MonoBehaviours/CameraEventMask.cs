using UnityEngine;

namespace ForestLib.Utils
{
	/// <summary>
	/// Defines which layers can receive OnMouseXXX events.
	/// 
	/// The Camera's event mask determines whether the GameObject is able to receive mouse events.
	/// Only objects visible by the camera and whose layerMask overlaps with the camera's eventMask will be able to
	/// receive OnMouseXXX events.
	/// </summary>
	public class CameraEventMask : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _eventMask = 1;

		private void Awake()
		{
			Camera.main.eventMask = _eventMask;
		}
	}
}
