using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ForestRoyale.Core
{
	public class TimeController : MonoBehaviour, IGUIDrawer
	{
		[SerializeField] private float _timeScale = 1f;
		[SerializeField, MinValue(0.00001)] private float _minTimeScale = 0.25f;
		[SerializeField, MaxValue(100)] private float _maxTimeScale = 4f;
		[ShowInInspector, ReadOnly] private bool _paused = false;

		private InputAction _increaseAction;
		private InputAction _decreaseAction;
		private InputAction _pauseTime;
		private InputAction _resetTimeScale;

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
				_increaseAction.performed += OnIncreaseTimeScale;
			}

			if (_decreaseAction != null)
			{
				_decreaseAction.performed += OnDecreaseTimeScale;
			}

			if (_pauseTime != null)
			{
				_pauseTime.performed += OnPauseTime;
			}

			if (_resetTimeScale != null)
			{
				_resetTimeScale.performed += OnResetTimeScale;
			}
		}

		private void OnDisable()
		{
			if (_increaseAction != null)
			{
				_increaseAction.performed -= OnIncreaseTimeScale;
			}

			if (_decreaseAction != null)
			{
				_decreaseAction.performed -= OnDecreaseTimeScale;
			}

			if (_pauseTime != null)
			{
				_pauseTime.performed -= OnPauseTime;
			}

			if (_resetTimeScale != null)
			{
				_resetTimeScale.performed -= OnResetTimeScale;
			}
		}

		private void Start()
		{
			ApplyTimeScale();
		}

		private void OnIncreaseTimeScale(InputAction.CallbackContext context)
		{
			_timeScale = IncreaseTimeScale(_timeScale);
			ApplyTimeScale();
		}

		private void OnDecreaseTimeScale(InputAction.CallbackContext context)
		{
			_timeScale = DecreaseTimeScale(_timeScale);
			ApplyTimeScale();
		}

		private void OnPauseTime(InputAction.CallbackContext obj)
		{
			TogglePause();
			ApplyTimeScale();
		}

		private void OnResetTimeScale(InputAction.CallbackContext obj)
		{
			_timeScale = 1f;
			ApplyTimeScale();
		}

		private float DecreaseTimeScale(float scale)
		{
			scale = Mathf.Max(_timeScale * 0.5f, _minTimeScale);
			return scale;
		}

		private float IncreaseTimeScale(float scale)
		{
			scale = Mathf.Min(_timeScale * 2f, _maxTimeScale);
			return scale;
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

		private void TogglePause()
		{
			_paused = !_paused;
		}

		void IGUIDrawer.DrawGUI()
		{
			// Show slider in logarithmic space
			_timeScale = GUILayoutUtils.LogSlider("Time Scale", _timeScale, 0.01f, 100f);

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("-", GUILayoutOptions.ExpandWidth()))
				{
					_timeScale = DecreaseTimeScale(_timeScale);
					ApplyTimeScale();
				}

				GUILayout.TextField(_timeScale.ToString("F2"));

				if (GUILayout.Button("+", GUILayoutOptions.ExpandWidth()))
				{
					_timeScale = IncreaseTimeScale(_timeScale);
					ApplyTimeScale();
				}
			}
			GUILayout.EndHorizontal();



			GUIUtils.PushBackgroundColor(_paused ? Color.red : Color.white);
			if (GUILayout.Button(_paused ? "Resume" : "Pause", GUILayoutOptions.ExpandWidth()))
			{
				TogglePause();
			}
			GUIUtils.PopBackgroundColor();
			
			ApplyTimeScale();
		}
	}
}