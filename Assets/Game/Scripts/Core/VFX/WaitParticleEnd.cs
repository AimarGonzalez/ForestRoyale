using System;
using UnityEngine;

namespace ForestRoyale.Core
{
	public class WaitParticleEnd : MonoBehaviour
	{
		public event Action<WaitParticleEnd> OnParticleEnd;

		private void Awake()
		{
			ParticleSystem.MainModule mainModule = GetComponent<ParticleSystem>().main;
			mainModule.stopAction = ParticleSystemStopAction.Callback;
		}

		private void OnParticleSystemStopped()
		{
			OnParticleEnd?.Invoke(this);
		}
	}
}