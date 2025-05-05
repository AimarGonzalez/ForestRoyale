using UnityEngine;

namespace ForestLib.Utils
{
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
