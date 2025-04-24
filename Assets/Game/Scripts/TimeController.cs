using UnityEngine;
using UnityEngine.InputSystem;

namespace ForestRoyale
{
	public class TimeController : MonoBehaviour
	{
		[SerializeField] private float _timeScale = 1f;
		[SerializeField] private float _minTimeScale = 0.25f;
		[SerializeField] private float _maxTimeScale = 4f;

		private InputAction _increaseAction;
		private InputAction _decreaseAction;

		private void Awake()
		{
			_increaseAction = InputSystem.actions.FindAction("TimeScale.Increase");
			_decreaseAction = InputSystem.actions.FindAction("TimeScale.Decrease");
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

		private void ApplyTimeScale()
		{
			UnityEngine.Time.timeScale = _timeScale;
		}
	}
}