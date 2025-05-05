using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ForestRoyale
{
	public class TimeController : MonoBehaviour
	{
		[SerializeField] private float _timeScale = 1f;
		[SerializeField] private float _minTimeScale = 0.25f;
		[SerializeField] private float _maxTimeScale = 4f;
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
			_timeScale = Mathf.Min(_timeScale * 2f, _maxTimeScale);
			ApplyTimeScale();
		}

		private void OnDecreaseTimeScale(InputAction.CallbackContext context)
		{
			_timeScale = Mathf.Max(_timeScale * 0.5f, _minTimeScale);
			ApplyTimeScale();
		}

		private void OnPauseTime(InputAction.CallbackContext obj)
		{
			_paused = !_paused;

			if (_paused)
			{
				UnityEngine.Time.timeScale = 0;
			}
			else
			{
				ApplyTimeScale();
			}
		}

		private void OnResetTimeScale(InputAction.CallbackContext obj)
		{
			_timeScale = 1f;
			ApplyTimeScale();
		}

		private void ApplyTimeScale()
		{
			UnityEngine.Time.timeScale = _timeScale;
			Debug.Log($"Time scale: {_timeScale}");
		}
	}
}