using ForestLib.Utils;
using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class UnitView : UnitComponent, IUnitStateChangeListener, ISerializationCallbackReceiver
	{
		// ---------------------------

		// We need GameObjects cause List<GameObject> failt to serialize in the UnitySerializedDictionary
		[Serializable]
		private class GameObjects
		{
			public List<GameObject> Objects = new List<GameObject>();
		}

		[Serializable]
		private class UnitStateToGameObjectDictionary : UnitySerializedDictionary<UnitState, GameObjects> { }

		[Serializable]
		private class GameObjectToUnitStateDictionary : UnitySerializedDictionary<GameObject, UnitState> { }

		// ---------------------------

		[SerializeField, OnValueChanged(nameof(PopulateGoDictionary))]
		[LabelText("Visibility Map")]
		private UnitStateToGameObjectDictionary _stateDictionary = new UnitStateToGameObjectDictionary();

		[SerializeField, ReadOnly]
		private GameObjectToUnitStateDictionary _goDictionary = new GameObjectToUnitStateDictionary();

		// ---------------------------

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[ShowInInspector, OnValueChanged(nameof(SimulateState))]
		private UnitState _simulateState;

		private void SimulateState()
		{
			(this as IUnitStateChangeListener).OnUnitStateChanged(UnitState.CastingPreview, _simulateState);
		}

		void IUnitStateChangeListener.OnUnitStateChanged(UnitState oldState, UnitState newState)
		{
			if (!_stateDictionary.ContainsKey(newState))
			{
				// Don't react unknown states
				return;
			}
			
			foreach (KeyValuePair<GameObject, UnitState> pair in _goDictionary)
			{
				GameObject go = pair.Key;
				UnitState state = pair.Value;
				go.SetActive(state.HasFlag(newState));
			}
		}

		public void OnBeforeSerialize()
		{
			PopulateGoDictionary();
		}

		public void OnAfterDeserialize()
		{
			PopulateGoDictionary();
		}

		private void PopulateGoDictionary()
		{
			_goDictionary.Clear();
			foreach (KeyValuePair<UnitState, GameObjects> pair in _stateDictionary)
			{
				foreach (GameObject go in pair.Value.Objects)
				{
					if (_goDictionary.ContainsKey(go))
					{
						_goDictionary[go] |= pair.Key;
					}
					else
					{
						_goDictionary.Add(go, pair.Key);
					}
				}
			}
		}
	}
}

