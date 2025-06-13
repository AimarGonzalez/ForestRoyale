using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace ForestRoyale.Core
{
	public class TimeController : MonoBehaviour, IGUIDrawer
	{
		[SerializeField] private float _timeScale = 1f;
		[ShowInInspector, ReadOnly] private bool _paused = false;

		[SerializeField]
		private List<float> _timeScales = new() { 0.0f, 0.25f, 0.5f, 1f, 2f, 4f, 10f };

		[Inject]
		private CheatsStyleProvider _cheatsStyleProvider;

		private InputAction _increaseAction;
		private InputAction _decreaseAction;
		private InputAction _pauseTime;
		private InputAction _resetTimeScale;
		private GUIStyle _buttonStyle;

		private int _timeScaleIndex = 3;

		private void Awake()
		{
			_increaseAction = InputSystem.actions.FindAction("TimeScale.Increase");
			_decreaseAction = InputSystem.actions.FindAction("TimeScale.Decrease");
			_pauseTime = InputSystem.actions.FindAction("TimeScale.Pause");
			_resetTimeScale = InputSystem.actions.FindAction("TimeScale.Reset");
		}

		private void OnEnable()
		{
			if (_increaseAction != null)
			{
				_increaseAction.performed += OnIncreaseTimeScaleKey;
			}

			if (_decreaseAction != null)
			{
				_decreaseAction.performed += OnDecreaseTimeScaleKey;
			}

			if (_pauseTime != null)
			{
				_pauseTime.performed += OnPauseTimeKey;
			}

			if (_resetTimeScale != null)
			{
				_resetTimeScale.performed += OnResetTimeScaleKey;
			}
		}

		private void OnDisable()
		{
			if (_increaseAction != null)
			{
				_increaseAction.performed -= OnIncreaseTimeScaleKey;
			}

			if (_decreaseAction != null)
			{
				_decreaseAction.performed -= OnDecreaseTimeScaleKey;
			}

			if (_pauseTime != null)
			{
				_pauseTime.performed -= OnPauseTimeKey;
			}

			if (_resetTimeScale != null)
			{
				_resetTimeScale.performed -= OnResetTimeScaleKey;
			}
		}

		private void Start()
		{
			ApplyTimeScale();
		}

		private void OnIncreaseTimeScaleKey(InputAction.CallbackContext context)
		{
			IncreaseTimeScale();
		}

		private void OnDecreaseTimeScaleKey(InputAction.CallbackContext context)
		{
			DecreaseTimeScale();
		}

		private void OnPauseTimeKey(InputAction.CallbackContext obj)
		{
			TogglePause();
		}

		private void OnResetTimeScaleKey(InputAction.CallbackContext obj)
		{
			_timeScale = 1f;
			ApplyTimeScale();
		}

		private void DecreaseTimeScale()
		{
			_timeScaleIndex = Math.Max(_timeScaleIndex - 1, 0);
			_timeScale = _timeScales[_timeScaleIndex];

			ApplyTimeScale();
		}

		private void IncreaseTimeScale()
		{
			_timeScaleIndex = Math.Min(_timeScaleIndex + 1, _timeScales.Count - 1);
			_timeScale = _timeScales[_timeScaleIndex];

			ApplyTimeScale();
		}

		private void TogglePause()
		{
			_paused = !_paused;

			ApplyTimeScale();
		}


		private void ApplyTimeScale()
		{
			if (_paused)
			{
				Time.timeScale = 0;
			}
			else
			{
				Time.timeScale = _timeScale;
			}
		}

		private void OnGUI()
		{
			_cheatsStyleProvider.PushButtonStyle();

			const float margin = 10f;
			GUILayout.BeginArea(new Rect(GUIUtils.HalfScreenW + margin, margin, GUIUtils.HalfScreenW, GUIUtils.HalfScreenH), new GUIStyle());

			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();

				GUILayout.BeginVertical();
				{
					float size = Screen.width * 0.2f;

					GUI.enabled = _timeScaleIndex < _timeScales.Count - 1;
					if (GUILayout.Button("+", GUILayout.Width(size), GUILayout.Height(size)))
					{
						IncreaseTimeScale();
					}

					GUI.enabled = _timeScaleIndex > 0;
					if (GUILayout.Button("-", GUILayout.Width(size), GUILayout.Height(size)))
					{
						DecreaseTimeScale();
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();

			GUILayout.EndArea();

			_cheatsStyleProvider.PopButtonStyle();
		}

		
		void IGUIDrawer.DrawGUI()
		{
			/*
			// Show slider in logarithmic space
			_timeScale = GUILayoutUtils.LogSlider("Time Scale", _timeScale, 0.01f, 100f);

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("-", GUILayoutOptions.ExpandWidth()))
				{
					DecreaseTimeScale();
				}

				GUILayout.TextField(_timeScale.ToString("F2"));

				if (GUILayout.Button("+", GUILayoutOptions.ExpandWidth()))
				{
					IncreaseTimeScale();
				}
			}
			GUILayout.EndHorizontal();



			GUIUtils.PushBackgroundColor(_paused ? Color.red : Color.white);
			if (GUILayout.Button(_paused ? "Resume" : "Pause", GUILayoutOptions.ExpandWidth()))
			{
				TogglePause();
			}
			GUIUtils.PopBackgroundColor();
			*/
		}
	}
}