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
		private enum UIAction
		{
			Hide = 1 << 0,
			Show = 1 << 1,
		}
		
		// ---------------------------

		[Serializable]
		private class ViewSettings
		{
			[EnumToggleButtons]
			[TableColumnWidth(width: 100, resizable: false)]
			public TState FromState;

			[EnumToggleButtons]
			[TableColumnWidth(width: 100, resizable: false)]
			public TState ToState;
			
			[EnumToggleButtons]
			public UIAction Action;
			public List<GameObject> ActiveGameObjects = new List<GameObject>();
		}

		// ---------------------------

		[SerializeField]
		[LabelText("Visibility Map")]
		[TableList]
		private List<ViewSettings> _settinbsByState = new List<ViewSettings>();

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

			foreach (ViewSettings settings in _settinbsByState)
			{
				if (!settings.FromState.HasAnyFlag(oldState) || !settings.ToState.HasAnyFlag(newState))
				{
					foreach (GameObject go in settings.ActiveGameObjects)
					{
						_objectsToHide.Add(go);
					}
				}
			}

			foreach (ViewSettings settings in _settinbsByState)
			{
				if (settings.FromState.HasAnyFlag(oldState) && settings.ToState.HasAnyFlag(newState))
				{
					foreach (GameObject go in settings.ActiveGameObjects)
					{
						_objectsToHide.Remove(go);
						_objectsToShow.Add(go);
					}
				}
			}

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

