using UnityEngine;

namespace ForestRoyale.Gameplay.Units
{
	public class TriggerListener : MonoBehaviour
	{
		public event System.Action<Collider> OnTriggerEnterEvent;
		public event System.Action<Collider> OnTriggerExitEvent;
		public event System.Action<Collider> OnTriggerStayEvent;

		private void OnTriggerEnter(Collider other)
		{
			OnTriggerEnterEvent?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			OnTriggerExitEvent?.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			OnTriggerStayEvent?.Invoke(other);
		}
	}
}