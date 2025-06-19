using System;
using UnityEngine;

namespace ForestRoyale.Core
{
	public class WaitParticleEnd : MonoBehaviour
	{
		public event Action<WaitParticleEnd> OnParticleEnd;

		private void OnParticleSystemStopped()
		{
			OnParticleEnd?.Invoke(this);
		}
	}
}