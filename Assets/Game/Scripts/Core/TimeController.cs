using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.tvOS;
using VContainer;

namespace ForestRoyale.Core
{
	public class TimeController : MonoBehaviour, IGUIDrawer
	{
		[SerializeField] private float _timeScale = 1f;
		[ShowInInspector, ReadOnly] private bool _paused = false;

		[SerializeField]
		private List<float> _timeScales = new() { 0.0f, 0.25f, 0.5f, 1f, 2f, 4f };
		[SerializeField]
		[LabelText("Starting index")]
		private int _timeScaleIndex = 3;

		[Serializable]
		private class DebugControlsSettings
		{
			[SerializeField] private float _width = 0.45f;
			[SerializeField] private float _height = 0.16f;
			[SerializeField] private float _margin = 0.01f;
			[SerializeField] private float _labelWidth = 0.14f;
			[SerializeField] private float _labelMargin = 0.01f;

			public float Width => (_width - _margin) * Screen.width;
			public float Height => (_height - _margin) * Screen.width;
			public float Margin => _margin * Screen.width;

			public float LabeFullWidth => _labelWidth * Screen.width;

			public float LabelWidth => (_labelWidth - _labelMargin - _labelMargin) * Screen.width;
			public float LabelMargin => _labelMargin * Screen.width;
		}

		[SerializeField]
		private DebugControlsSettings _debugControlsSettings;

		[Inject]
		private CheatsStyleProvider _cheatsStyleProvider;

		private InputAction _increaseAction;
		private InputAction _decreaseAction;
		private InputAction _pauseTime;
		private InputAction _resetTimeScale;

		private GUIStyle _labelStyle;
		private GUIStyle LabelStyle => _labelStyle ??= new GUIStyle(GUI.skin.label)
		{
			alignment = TextAnchor.MiddleCenter,
			wordWrap = false,
			clipping = TextClipping.Overflow,
		};


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

			float debugControlsWidth = _debugControlsSettings.Width;
			float debugControlsHeight = _debugControlsSettings.Height;
			float debugControlsMargin = _debugControlsSettings.Margin;
			float debugControlsX = Screen.width - _debugControlsSettings.Width - _debugControlsSettings.Margin;
			float debugControlsY = debugControlsMargin;

			float buttonWidth = (debugControlsWidth - _debugControlsSettings.LabeFullWidth - _debugControlsSettings.LabelMargin) * 0.5f;

			GUILayout.BeginArea(new Rect(debugControlsX, debugControlsY, debugControlsWidth, debugControlsHeight), GUI.skin.box);
			GUILayout.BeginHorizontal();
			{
				GUI.enabled = _timeScaleIndex > 0;
				if (GUILayout.Button("-", GUILayoutOptions.Width(buttonWidth).ExpandHeight()))
				{
					DecreaseTimeScale();
				}

				GUI.enabled = true;
				LabelStyle.fontSize = GUIUtils.CalcAutoFontSize($"x{_timeScale:0.0#}", _debugControlsSettings.LabelWidth);
				GUILayout.Label($"x{_timeScale:0.##}", LabelStyle, GUILayoutOptions.ExpandHeight());

				GUI.enabled = _timeScaleIndex < _timeScales.Count - 1;
				if (GUILayout.Button("+", GUILayoutOptions.Width(buttonWidth).ExpandHeight()))
				{
					IncreaseTimeScale();
				}
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