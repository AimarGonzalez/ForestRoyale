using ForestRoyale.Gui;
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