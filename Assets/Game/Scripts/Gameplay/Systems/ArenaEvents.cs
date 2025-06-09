using ForestRoyale.Gameplay.Combat;
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
		public event Action<Unit> OnUnitDestroyed;
		public event Action<Unit> OnUnitDamaged;

		public event Action<Unit, Unit> OnUnitAttacked;

		public void TriggerUnitCreated(Unit unit)
		{
			OnUnitCreated?.Invoke(unit);
		}

		public void TriggerUnitDestroyed(Unit unit)
		{
			OnUnitDestroyed?.Invoke(unit);
		}

		public void TriggerUnitDamaged(Unit unit)
		{
			OnUnitDamaged?.Invoke(unit);
		}

		public void TriggerUnitAttacked(Unit attacker, Unit target)
		{
			OnUnitAttacked?.Invoke(attacker, target);
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