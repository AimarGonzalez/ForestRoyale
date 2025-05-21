using UnityEngine;
using UnityEngine.UI;

namespace ForestLib.Utils
{
	public class UIFollower : MonoBehaviour
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

		[Tooltip("Minimum distance to keep from target")]
		[SerializeField] private float keepDistanceToTarget = 0f;

		[Tooltip("When to run the follow logic")]
		[SerializeField] private UpdateType updateType = UpdateType.Update;

		[Tooltip("Offset from the target position")]
		[SerializeField] private Vector2 offset = Vector2.zero;

		[Tooltip("Camera used for UI canvas calculations (defaults to canvas camera or main camera)")]
		[SerializeField] private Camera canvasCamera;

		private RectTransform _rectTransform;
		private Canvas _canvas;
		private bool _isCanvasOverlay;
		private Camera _camera;
		private Vector2 _originalPosition;

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

		public Vector2 Offset
		{
			get => offset;
			set => offset = value;
		}

		public bool TargetCursor
		{
			get => targetCursor;
			set => targetCursor = value;
		}

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
			if (_rectTransform == null)
			{
				Debug.LogError("UIFollower requires a RectTransform component!", this);
				enabled = false;
				return;
			}

			_originalPosition = _rectTransform.anchoredPosition;

			_canvas = GetComponentInParent<Canvas>();
			if (_canvas == null)
			{
				Debug.LogError("UIFollower requires a Canvas parent!", this);
				enabled = false;
				return;
			}

			_isCanvasOverlay = _canvas.renderMode == RenderMode.ScreenSpaceOverlay;

			// If in a ScreenSpace Camera mode and no camera is specified, use the canvas's camera
			if (!_isCanvasOverlay && _canvas.renderMode == RenderMode.ScreenSpaceCamera && canvasCamera == null)
			{
				canvasCamera = _canvas.worldCamera;
			}

			// Ensure we have a camera reference for calculations
			_camera = canvasCamera != null ? canvasCamera : Camera.main;
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

			Vector2 currentPosition = _rectTransform.anchoredPosition;
			Vector2 targetPosition;

			if (targetCursor)
			{
				targetPosition = GetCursorCanvasPosition();
			}
			else if (target != null)
			{
				// If target is another UI element
				RectTransform targetRect = target.GetComponent<RectTransform>();
				if (targetRect != null)
				{
					targetPosition = targetRect.anchoredPosition;
				}
				else
				{
					// Target is a world object, convert to canvas space
					targetPosition = WorldToCanvasPosition(target.position);
				}
			}
			else
			{
				return;
			}

			targetPosition += offset;
			Vector2 newPosition = currentPosition;

			if (followX)
			{
				newPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, easing);
			}

			if (followY)
			{
				newPosition.y = Mathf.Lerp(currentPosition.y, targetPosition.y, easing);
			}

			if (keepDistanceToTarget > 0)
			{
				float distance = Vector2.Distance(newPosition, targetPosition);
				if (distance < keepDistanceToTarget)
				{
					Vector2 direction = (newPosition - targetPosition).normalized;
					newPosition = targetPosition + (direction * keepDistanceToTarget);
				}
			}

			_rectTransform.anchoredPosition = newPosition;
		}

		private Vector2 GetCursorCanvasPosition()
		{
			if (_canvas == null)
			{
				return Vector2.zero;
			}

			if (_isCanvasOverlay)
			{
				// For Screen Space - Overlay canvas
				return Input.mousePosition;
			}
			else
			{
				// For Screen Space - Camera or World Space canvas
				RectTransformUtility.ScreenPointToLocalPointInRectangle(
					_canvas.GetComponent<RectTransform>(),
					Input.mousePosition,
					_canvas.worldCamera,
					out Vector2 localPoint);
				return localPoint;
			}
		}

		private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
		{
			if (_canvas == null || _camera == null)
			{
				return Vector2.zero;
			}

			Vector2 viewportPosition = _camera.WorldToViewportPoint(worldPosition);
			return new Vector2(
				(viewportPosition.x * _canvas.GetComponent<RectTransform>().sizeDelta.x) - (_canvas.GetComponent<RectTransform>().sizeDelta.x * 0.5f),
				(viewportPosition.y * _canvas.GetComponent<RectTransform>().sizeDelta.y) - (_canvas.GetComponent<RectTransform>().sizeDelta.y * 0.5f)
			);
		}
		
		public void ResetPosition()
		{
			_rectTransform.anchoredPosition = _originalPosition;
		}
	}
}