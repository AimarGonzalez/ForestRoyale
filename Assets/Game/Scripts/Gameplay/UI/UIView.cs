using ForestLib.ExtensionMethods;
using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class UIView<TState> : MonoBehaviour where TState : struct, Enum
	{
		[Flags]
		private enum UIActionType
		{
			Hide = 1 << 0,
			Show = 1 << 1,
		}

		// ---------------------------

		[Serializable]
		private class Transition
		{
			[EnumToggleButtons]
			[TableColumnWidth(width: 100, resizable: false)]
			public TState FromState;

			[EnumToggleButtons]
			[TableColumnWidth(width: 100, resizable: false)]
			public TState ToState;

			public Action Action;
		}

		[Serializable]
		private class Action
		{
			[EnumToggleButtons]
			[HideLabel]
			public UIActionType ActionType;

			[HideLabel]
			public List<GameObject> Targets = new List<GameObject>();
		}

		// ---------------------------

		[SerializeField]
		[LabelText("Visibility Map")]
		[TableList]
		private List<Transition> _transitions = new List<Transition>();

		private List<GameObject> _objectsToHide= new List<GameObject>();
		private List<GameObject> _objectsToShow= new List<GameObject>();


		// ---------------------------

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[ShowInInspector, OnValueChanged(nameof(SimulateState))]
		private TState _oldState;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[ShowInInspector, OnValueChanged(nameof(SimulateState))]
		private TState _newState;

		private void SimulateState()
		{
			UpdateVisibleObjects(_oldState, _newState);
			_oldState = _newState;
		}

		protected virtual void OnStateChanged(TState oldState, TState newState)
		{
			UpdateVisibleObjects(oldState, newState);
		}

		private void UpdateVisibleObjects(TState oldState, TState newState)
		{
			if (oldState.IsSameFlag(newState))
			{
				return;
			}

			_objectsToHide.Clear();
			_objectsToShow.Clear();

			foreach (Transition transition in _transitions)
			{
				if (transition.FromState.HasAnyFlag(oldState) && transition.ToState.HasAnyFlag(newState))
				{
					switch (transition.Action.ActionType)
					{
						case UIActionType.Hide:
							_objectsToHide.AddRange(transition.Action.Targets);
							break;
						case UIActionType.Show:
							_objectsToShow.AddRange(transition.Action.Targets);
							break;
					}
				}
			}

			//Remove duplicates
			_objectsToHide.RemoveAll(go => _objectsToShow.Contains(go));
			_objectsToShow.RemoveAll(go => _objectsToHide.Contains(go));

			foreach (GameObject go in _objectsToHide)
			{
				go.SetActive(false);
			}

			foreach (GameObject go in _objectsToShow)
			{
				go.SetActive(true);
			}
		}
	}
}

