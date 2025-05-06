using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units
{
	public class Collider2DListener : MonoBehaviour
	{
		public event Action<Collider2D> OnTriggerEnterEvent;
		public event Action<Collider2D> OnTriggerExitEvent;
		public event Action<Collider2D> OnTriggerStayEvent;

		public event Action<Collider2D> OnCollisionEnterEvent;
		public event Action<Collider2D> OnCollisionExitEvent;
		public event Action<Collider2D> OnCollisionStayEvent;

		public event Action OnMouseDownEvent;


		//--------------------------------

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

		//--------------------------------

		private void OnCollisionEnter2D(Collision2D other)
		{
			OnCollisionEnterEvent?.Invoke(other.collider);
		}

		private void OnCollisionStay2D(Collision2D other)
		{
			OnCollisionStayEvent?.Invoke(other.collider);
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			OnCollisionExitEvent?.Invoke(other.collider);
		}

		//--------------------------------

		private void OnMouseDown()
		{
			OnMouseDownEvent?.Invoke();
		}
	}
}