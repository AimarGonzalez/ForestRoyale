using System;
using ForestRoyale.Gameplay.Units;
using UnityEngine;
using VContainer.Unity;

namespace ForestRoyale.Gameplay.Systems
{
	public class ArenaEvents : IPostStartable
	{
		public event Action OnSceneUnitsInitialized;

		public event Action<Unit> OnUnitCreated;
		public event Action<Unit> OnUnitRemoved;
		public event Action<Unit> OnUnitDamaged;

		public event Action<Unit, Unit> OnUnitAttacked;
		public event Action<Unit, Unit> OnProjectileFired;
		public event Action<Unit, Unit> OnProjectileHit;

		public void TriggerUnitCreated(Unit unit)
		{
			OnUnitCreated?.Invoke(unit);
		}

		public void TriggerUnitRemoved(Unit unit)
		{
			OnUnitRemoved?.Invoke(unit);
		}

		public void TriggerUnitDamaged(Unit unit)
		{
			OnUnitDamaged?.Invoke(unit);
		}

		public void TriggerUnitAttacked(Unit attacker, Unit target)
		{
			OnUnitAttacked?.Invoke(attacker, target);
		}

		public void TriggerProjectileFired(Unit attacker, Unit target)
		{
			OnProjectileFired?.Invoke(attacker, target);
		}

		public void TriggerProjectileHit(Unit attacker, Unit target)
		{
			OnProjectileHit?.Invoke(attacker, target);
		}

		// ----- 

		/// <summary>
		/// We know Units do AutoInitialization during Start() so its safe to assume they are initialized.
		/// </summary>
		void IPostStartable.PostStart()
		{
			Debug.Log("ArenaEvents - SceneUnitsInitialized!");
			OnSceneUnitsInitialized?.Invoke();
		}
	}
}