using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviors;
using ForestRoyale.Gui;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestRoyale.Game.Scripts.Gameplay.Units.MonoBehaviours
{
	public abstract class UnitComponent : MonoBehaviour
	{
		
		[SerializeField]
		private UnitRoot _root;

		[ShowInInspector, ReadOnly]
		[BoxGroup(InspectorConstants.DebugBoxGroup), PropertyOrder(InspectorConstants.DebugBoxGroupOrder)]
		private Unit _unit;
		
		protected Unit Unit => _unit;
		
		protected virtual void Awake()
		{
			_root ??= GetComponentInParent<UnitRoot>();

			_root.OnUnitChanged += OnUnitChanged_Internal;

			_unit = _root.Unit;
		}

		protected virtual void OnDestroy()
		{
			_root.OnUnitChanged -= OnUnitChanged_Internal;
		}
		
		private void OnUnitChanged_Internal(Unit newUnit)
		{
			_unit = newUnit;

			OnUnitChanged();
		}

		protected virtual void OnUnitChanged()
		{
			// implement in children
		}
	}
}