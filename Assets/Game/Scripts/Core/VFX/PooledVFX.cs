using ForestLib.ExtensionMethods;
using ForestRoyale.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Core
{

	public class PooledVFX : PooledGameObject
	{
		public enum State
		{
			NotStarted,
			Playing,
			Finished,
		}

		private ParticleSystem[] _particleSystems;

		private State _state;

		private List<WaitParticleEnd> _existingParticleWaiters = new List<WaitParticleEnd>();
		private List<WaitParticleEnd> _waitingParticles = new List<WaitParticleEnd>();
		private bool HaveAllParticlesFinished => _waitingParticles.Count == 0;

		public bool HasFinished => _state == State.Finished;

		protected override void Awake()
		{
			base.Awake();
			
			_particleSystems = GetComponentsInChildren<ParticleSystem>();

			SetupWaitForParticles();
		}

		private void SetupWaitForParticles()
		{
			foreach (ParticleSystem particle in _particleSystems)
			{
				WaitParticleEnd waitParticleEnd = particle.GetOrCreate<WaitParticleEnd>();
				_existingParticleWaiters.Add(waitParticleEnd);
				waitParticleEnd.OnParticleEnd += OnParticleEnd;
			}
		}

		protected override void OnBeforeGetFromPool()
		{
			base.OnBeforeGetFromPool();

			_state = State.NotStarted;

			_waitingParticles.Clear();
			_waitingParticles.AddRange(_existingParticleWaiters);
		}

		protected override void OnAfterGetFromPool()
		{
			base.OnAfterGetFromPool();
			Play();
		}

		protected override void OnReturnToPool()
		{
			base.OnReturnToPool();

			foreach (var particle in _particleSystems)
			{
				particle.Stop();
			}
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
