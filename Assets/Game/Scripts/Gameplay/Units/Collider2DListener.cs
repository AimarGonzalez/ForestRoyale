using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units
{
	public class Collider2DListener : MonoBehaviour
	{
		public event Action<Collider2D> OnTriggerEnterEvent;
		public event Action<Collider2D> OnTriggerExitEvent;
		public event Action<Collider2D> OnTriggerStayEvent;
		
		public event Action OnMouseDownEvent;

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

		private void OnMouseDown()
		{
			OnMouseDownEvent?.Invoke();
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Debug.Log($"[{this.transform.parent.parent.name}>{name}] OnCollisionEnter2D: {other.transform.parent.parent.name}");
		}

		private void OnCollisionStay(Collision other)
		{
		}

		private void OnCollisionExit(Collision other)
		{
		}
	}
}