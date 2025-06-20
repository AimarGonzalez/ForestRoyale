using ForestRoyale.Core;
using ForestRoyale.Core.Pool;
using System;
using VContainer;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class DeathComponent : UnitComponent, IUnitStateChangeListener
	{
		[NonSerialized]
		private VFXInstance[] _vfxPrefabs;

		[Inject]
		private ObjectPoolService _objectPoolService;

		protected override void Awake()
		{
			base.Awake();

			_vfxPrefabs = GetComponentsInChildren<VFXInstance>(includeInactive: true);
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
					_objectPoolService.Release(Unit.UnitRoot.gameObject);
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
			foreach (VFXInstance vfxPrefab in _vfxPrefabs)
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
