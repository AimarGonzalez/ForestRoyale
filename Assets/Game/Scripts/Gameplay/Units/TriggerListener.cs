using UnityEngine;

namespace ForestRoyale.Gameplay.Units
{
	public class TriggerListener : MonoBehaviour
	{
		public event System.Action<Collider2D> OnTriggerEnterEvent;
		public event System.Action<Collider2D> OnTriggerExitEvent;
		public event System.Action<Collider2D> OnTriggerStayEvent;

		private void OnTriggerEnter2D(Collider2D other)
		{
			OnTriggerEnterEvent?.Invoke(other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			OnTriggerExitEvent?.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			OnTriggerStayEvent?.Invoke(other);
		}
	}
}