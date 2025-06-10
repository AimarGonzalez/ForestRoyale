using ForestLib.ExtensionMethods;
using ForestLib.Utils;
using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class UnitView : UnitComponent, IUnitStateChangeListener, ISerializationCallbackReceiver
	{
		// ---------------------------

		// We need GameObjects cause List<GameObject> failt to serialize in the UnitySerializedDictionary
		[Serializable]
		private class ViewSettings
		{
			public Material Material;

			[FormerlySerializedAs("Objects")]
			public List<GameObject> ActiveGameObjects = new List<GameObject>();
		}

		[Serializable]
		private class UnitStateToViewSettingsDictionary : UnitySerializedDictionary<UnitState, ViewSettings>
		{
			private static UnitStateFlagEqualityComparer s_comparer = new UnitStateFlagEqualityComparer();
			public UnitStateToViewSettingsDictionary() : base(s_comparer)
			{
			}
		}

		[Serializable]
		private class GameObjectToUnitStateDictionary : UnitySerializedDictionary<GameObject, UnitState> { }

		// ---------------------------

		[SerializeField, Required]
		private Renderer _bodyRenderer;

		[SerializeField, OnValueChanged(nameof(PopulateGoDictionary))]
		[LabelText("Visibility Map")]
		private UnitStateToViewSettingsDictionary _stateDictionary = new UnitStateToViewSettingsDictionary();

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

			UpdateMaterial(newState);
			UpdateVisibleObjects(newState);
		}

		private void UpdateVisibleObjects(UnitState newState)
		{
			foreach (KeyValuePair<GameObject, UnitState> pair in _goDictionary)
			{
				GameObject go = pair.Key;
				UnitState state = pair.Value;
				bool active = state.HasFlag(newState);
				go.SetActive(active);
			}
		}

		private void UpdateMaterial(UnitState newState)
		{
			if (!_bodyRenderer)
			{
				return;
			}
			
			foreach(KeyValuePair<UnitState, ViewSettings> pair in _stateDictionary)
			{
				UnitState accpetedFlags = pair.Key;
				ViewSettings settings = pair.Value;
				if (settings.Material && newState.HasAnyFlag(accpetedFlags))
				{
					_bodyRenderer.material = settings.Material;
				}
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
			foreach (KeyValuePair<UnitState, ViewSettings> pair in _stateDictionary)
			{
				foreach (GameObject go in pair.Value.ActiveGameObjects)
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

