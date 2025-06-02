using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestLib.Utils
{
	public enum UpdateType
	{
		Update,
		LateUpdate,
		FixedUpdate
	}

	public class Follower : MonoBehaviour
	{
		[Tooltip("The target to follow")]
		[SerializeField] private Transform _target;

		[Tooltip("Whether to follow the cursor position instead of a target transform")]
		[SerializeField] private bool _targetCursor = false;

		[Tooltip("How quickly to move toward the target")]
		[Range(1f, 100f)]
		[SerializeField] private float _easing = 20f;

		// TimeToTarget is a gentle approximation of the time it takes to reach the target. 
		// On the estimation time the follower will be at 99.9% of the distance.
		// Consider implementing a snapping mechanism if this is not sufficient.
		[ShowInInspector, ReadOnly]
		[Tooltip("$" + nameof(TimeToTargetTooltip))]
		public float TimeToTarget => 6f / _easing;
		private string TimeToTargetTooltip => $"{TimeToTarget} is an aproximation.\n" +
			$"The follower reaches 99% of the total distance in {TimeToTarget} seconds, " +
			$"but it will really take {12f / _easing} seconds to completely reach the target.";

		[Tooltip("Whether to follow on the X axis")]
		[SerializeField] private bool _followX = true;

		[Tooltip("Whether to follow on the Y axis")]
		[SerializeField] private bool _followY = true;

		[Tooltip("Whether to follow on the Z axis")]
		[SerializeField] private bool _followZ = true;

		[Tooltip("Minimum distance to keep from target")]
		[SerializeField] private float _keepDistanceToTarget = 0f;

		[Tooltip("When to run the follow logic")]
		[SerializeField] private UpdateType _updateType = UpdateType.Update;

		[Tooltip("Whether the movement is affected by time scale")]
		[SerializeField] private bool _affectedByTimeScale = true;

		[Tooltip("Offset from the target position")]
		[SerializeField] private Vector3 _offset = Vector3.zero;

		[Tooltip("Camera used for cursor position calculation (uses main camera if null)")]
		[SerializeField] private Camera _cursorCamera;

		public Transform Target
		{
			get => _target;
			set => _target = value;
		}

		public float Easing
		{
			get => _easing;
			set => _easing = Mathf.Clamp01(value);
		}

		public Vector3 Offset
		{
			get => _offset;
			set => _offset = value;
		}

		public bool TargetCursor
		{
			get => _targetCursor;
			set => _targetCursor = value;
		}

		private Camera _camera;

		private void Awake()
		{
			_camera = _cursorCamera != null ? _cursorCamera : Camera.main;
		}

		private void Update()
		{
			if (_updateType == UpdateType.Update)
			{
				FollowTarget(_easing);
			}
		}

		private void LateUpdate()
		{
			if (_updateType == UpdateType.LateUpdate)
			{
				FollowTarget(_easing);
			}
		}

		private void FixedUpdate()
		{
			if (_updateType == UpdateType.FixedUpdate)
			{
				FollowTarget(_easing);
			}
		}

		private void FollowTarget(float easing)
		{
			if (_target == null && !_targetCursor)
			{
				return;
			}

			Vector3 currentPosition = transform.position;
			Vector3 targetPosition;

			if (_targetCursor)
			{
				targetPosition = GetCursorWorldPosition();
			}
			else
			{
				targetPosition = _target.position;
			}

			targetPosition += _offset;
			Vector3 newPosition = currentPosition;

			float deltaTime = _affectedByTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;

			// This formula ensures the movement is consistent regardless of the frame rate
			float consistentEasing = 1f - Mathf.Exp(-deltaTime * easing);

			if (_followX)
			{
				newPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, consistentEasing);
			}

			if (_followY)
			{
				newPosition.y = Mathf.Lerp(currentPosition.y, targetPosition.y, consistentEasing);
			}

			if (_followZ)
			{
				newPosition.z = Mathf.Lerp(currentPosition.z, targetPosition.z, consistentEasing);
			}

			if (_keepDistanceToTarget > 0)
			{
				float distance = Vector3.Distance(newPosition, targetPosition);
				if (distance < _keepDistanceToTarget)
				{
					Vector3 direction = (newPosition - targetPosition).normalized;
					newPosition = targetPosition + (direction * _keepDistanceToTarget);
				}
			}

			transform.position = newPosition;
		}

		private Vector3 GetCursorWorldPosition()
		{
			if (_camera == null)
			{
				Debug.LogWarning("No camera found for cursor position calculation.");
				return Vector3.zero;
			}

			Vector3 mousePos = Input.mousePosition;
			mousePos.z = transform.position.z - _camera.transform.position.z;
			return _camera.ScreenToWorldPoint(mousePos);
		}

		public void JumpToTargetPosition()
		{
			FollowTarget(9999f);
		}
	}
}