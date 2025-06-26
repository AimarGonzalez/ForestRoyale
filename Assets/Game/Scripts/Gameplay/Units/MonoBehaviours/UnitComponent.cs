using ForestLib.ExtensionMethods;
using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	// TODO: Rename to UnitSubComponent
	public abstract class UnitComponent : MonoBehaviour
	{

		[SerializeField, ReadOnly]
		[BoxGroup(DebugUI.Group)]
		[FoldoutGroup(DebugUI.GroupUnitComponent), PropertyOrder(DebugUI.OrderUnitComponent)]
		[SuffixLabel("auto populated")]
		private UnitRoot _root;

		[FoldoutGroup(DebugUI.GroupUnitComponent), PropertyOrder(DebugUI.OrderUnitComponent)]
		[ShowInInspector]
		[NonSerialized]
		private Unit _unit;

		public Unit Unit => _unit;
		public UnitRoot Root => _root;

		protected virtual void Awake()
		{
			_root ??= GetComponentInParent<UnitRoot>();
			Debug.Assert(_root != null, $"UnitComponent can't find a parent UnitRoot! - {gameObject.GetPathInHierarchy()}");

			_unit = _root.Unit;
		}

		protected virtual void OnDestroy()
		{
		}

		public void SetUnit(Unit newUnit)
		{
			_unit = newUnit;
		}


#if UNITY_EDITOR
		public void ForceAwake(UnitRoot root)
		{
			_root = root;

			Awake();
		}
#endif
	}
}