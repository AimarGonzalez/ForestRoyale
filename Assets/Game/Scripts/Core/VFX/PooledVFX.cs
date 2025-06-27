using ForestRoyale.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Core
{

	public class VFXInstance : PooledGameObject
	{
		public enum State
		{
			NotStarted,
			Playing,
			Finished,
		}

		private ParticleSystem[] _particleSystems;

		private State _state;

		private List<WaitParticleEnd> _waitingParticles = new List<WaitParticleEnd>();
		private bool HaveAllParticlesFinished => _waitingParticles.Count == 0;

		public bool HasFinished => _state == State.Finished;

		protected override void Awake()
		{
			_particleSystems = GetComponentsInChildren<ParticleSystem>();

			SetupWaitForParticles();
		}

		private void SetupWaitForParticles()
		{
			foreach (ParticleSystem particle in _particleSystems)
			{
				if (!particle.TryGetComponent(out WaitParticleEnd waitParticleEnd))
				{
					waitParticleEnd = particle.gameObject.AddComponent<WaitParticleEnd>();
				}

				_waitingParticles.Add(waitParticleEnd);
				waitParticleEnd.OnParticleEnd += OnParticleEnd;
			}
		}

		private void OnEnable()
		{
			Play();
		}

		public void Play()
		{
			_state = State.Playing;
			foreach (var particle in _particleSystems)
			{
				particle.Play();
			}
		}

		private void OnParticleEnd(WaitParticleEnd waitParticleEnd)
		{
			_waitingParticles.Remove(waitParticleEnd);

			CheckVFXEnded();
		}

		private void CheckVFXEnded()
		{
			if (HaveAllParticlesFinished)
			{
				_state = State.Finished;
				ReleaseToPool();
			}
		}
	}
}
