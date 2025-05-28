using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	public abstract class UnitComponent : MonoBehaviour
	{

		[SerializeField]
		[BoxGroup(InspectorConstants.DebugGroup)]
		[FoldoutGroup(InspectorConstants.DebugGroupUnitComponent), PropertyOrder(InspectorConstants.DebugGroupUnitComponentOrder)]
		[SuffixLabel("auto populated")]
		private UnitRoot _root;

		[FoldoutGroup(InspectorConstants.DebugGroupUnitComponent), PropertyOrder(InspectorConstants.DebugGroupUnitComponentOrder)]
		[ShowInInspector, ReadOnly]
		[NonSerialized]
		private Unit _unit;

		public Unit Unit => _unit;

		protected virtual void Awake()
		{
			_root ??= GetComponentInParent<UnitRoot>();
			Debug.Assert(_root != null, "UnitComponent can't find a parent UnitRoot!");

			_root.OnUnitChanged += OnUnitChanged_Internal;

			_unit = _root.Unit;
		}

		protected virtual void OnDestroy()
		{
			if (_root)
			{
				_root.OnUnitChanged -= OnUnitChanged_Internal;
			}
		}

		private void OnUnitChanged_Internal(Unit newUnit)
		{
			Unit oldUnit = _unit;
			_unit = newUnit;
			
			OnUnitChanged(oldUnit, newUnit);
			OnUnitChanged();
		}
		
		protected virtual void OnUnitChanged(Unit oldUnit, Unit newUnit)
		{
			// implement in children
		}

		protected virtual void OnUnitChanged()
		{
			// implement in children
		}
		
#if UNITY_EDITOR
		public void ForceAwake(UnitRoot root)
		{
			_root = root;

			Awake();
		}

		public void ForceOnDestroy()
		{
			OnDestroy();
		}
#endif
	}
}