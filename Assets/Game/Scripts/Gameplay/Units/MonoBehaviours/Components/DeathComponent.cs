using ForestRoyale.Core;
using ForestRoyale.Core.Pool;
using System;
using VContainer;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class DeathComponent : UnitComponent, IUnitStateChangeListener
	{
		[NonSerialized]
		private PooledVFX[] _vfxPrefabs;

		[Inject]
		private GameObjectPoolService _gameObjectPoolService;

		protected override void Awake()
		{
			base.Awake();

			_vfxPrefabs = GetComponentsInChildren<PooledVFX>(includeInactive: true);
		}

		void IUnitStateChangeListener.OnUnitStateChanged(UnitState oldState, UnitState newState)
		{
			if (newState == UnitState.Dying)
			{
				PlayVFXs();
			}

			if (newState == UnitState.Dead)
			{
				if (!Unit.UnitStats.PermanentCorpse)
				{
					_gameObjectPoolService.Release(Unit.UnitRoot.gameObject);
				}
			}
		}

		private void PlayVFXs()
		{
			foreach (var vfxPrefab in _vfxPrefabs)
			{
				vfxPrefab.Play();
			}
		}

		public bool HasFinished()
		{
			foreach (PooledVFX vfxPrefab in _vfxPrefabs)
			{
				if (!vfxPrefab.HasFinished)
				{
					return false;
				}
			}

			return true;
		}
	}
}
