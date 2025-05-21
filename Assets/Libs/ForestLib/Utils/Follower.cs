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
		[SerializeField] private Transform target;

		[Tooltip("Whether to follow the cursor position instead of a target transform")]
		[SerializeField] private bool targetCursor = false;

		[Tooltip("How quickly to move toward the target (0-1, higher values = faster movement)")]
		[Range(0.01f, 1f)]
		[SerializeField] private float easing = 0.1f;

		[Tooltip("Whether to follow on the X axis")]
		[SerializeField] private bool followX = true;

		[Tooltip("Whether to follow on the Y axis")]
		[SerializeField] private bool followY = true;

		[Tooltip("Whether to follow on the Z axis")]
		[SerializeField] private bool followZ = true;

		[Tooltip("Minimum distance to keep from target")]
		[SerializeField] private float keepDistanceToTarget = 0f;

		[Tooltip("When to run the follow logic")]
		[SerializeField] private UpdateType updateType = UpdateType.Update;

		[Tooltip("Offset from the target position")]
		[SerializeField] private Vector3 offset = Vector3.zero;

		[Tooltip("Camera used for cursor position calculation (uses main camera if null)")]
		[SerializeField] private Camera cursorCamera;

		public Transform Target
		{
			get => target;
			set => target = value;
		}

		public float Easing
		{
			get => easing;
			set => easing = Mathf.Clamp01(value);
		}

		public Vector3 Offset
		{
			get => offset;
			set => offset = value;
		}

		public bool TargetCursor
		{
			get => targetCursor;
			set => targetCursor = value;
		}

		private Camera _camera;

		private void Awake()
		{
			_camera = cursorCamera != null ? cursorCamera : Camera.main;
		}

		private void Update()
		{
			if (updateType == UpdateType.Update)
			{
				FollowTarget();
			}
		}

		private void LateUpdate()
		{
			if (updateType == UpdateType.LateUpdate)
			{
				FollowTarget();
			}
		}

		private void FixedUpdate()
		{
			if (updateType == UpdateType.FixedUpdate)
			{
				FollowTarget();
			}
		}

		private void FollowTarget()
		{
			if (target == null && !targetCursor)
			{
				return;
			}

			Vector3 currentPosition = transform.position;
			Vector3 targetPosition;

			if (targetCursor)
			{
				targetPosition = GetCursorWorldPosition();
			}
			else
			{
				targetPosition = target.position;
			}

			targetPosition += offset;
			Vector3 newPosition = currentPosition;

			if (followX)
			{
				newPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, easing);
			}

			if (followY)
			{
				newPosition.y = Mathf.Lerp(currentPosition.y, targetPosition.y, easing);
			}

			if (followZ)
			{
				newPosition.z = Mathf.Lerp(currentPosition.z, targetPosition.z, easing);
			}

			if (keepDistanceToTarget > 0)
			{
				float distance = Vector3.Distance(newPosition, targetPosition);
				if (distance < keepDistanceToTarget)
				{
					Vector3 direction = (newPosition - targetPosition).normalized;
					newPosition = targetPosition + (direction * keepDistanceToTarget);
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
	}
}