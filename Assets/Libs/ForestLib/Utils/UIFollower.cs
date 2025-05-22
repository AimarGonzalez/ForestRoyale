using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace ForestLib.Utils
{
	public class UIFollower : MonoBehaviour
	{
		[Tooltip("The target to follow")]
		[SerializeField] private Transform target;

		[Tooltip("Whether to follow the cursor position instead of a target transform")]
		[SerializeField] private bool targetMouse = false;

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

		[Tooltip("Show debug information in game view")]
		[SerializeField]
		[OnValueChanged("SetupDebugUI")]
		private bool showDebugInfo = false;

		private RectTransform _rectTransform;
		private Canvas _canvas;
		private bool _isCanvasOverlay;
		private Camera _camera;
		private Vector2 _originalAnchor;
		private TextMeshProUGUI _debugText;

		private Vector2 _targetScreenPosition;

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

		public bool TargetMouse
		{
			get => targetMouse;
			set => targetMouse = value;
		}

		public static Vector2 GetScreenPosition(RectTransform rectTransform)
		{
			return rectTransform.position;
		}

		public static void SetScreenPosition(RectTransform rectTransform, Vector2 screenPosition, Canvas canvas, Camera camera = null)
		{
			if (rectTransform == null || canvas == null)
			{
				return;
			}

			Debug.Assert(canvas.renderMode == RenderMode.ScreenSpaceOverlay, "UIFollower: SetScreenPosition can only be used for ScreenSpaceOverlay canvas!");

			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				canvas.GetComponent<RectTransform>(),
				screenPosition,
				null,
				out Vector2 localPoint);

			rectTransform.position = canvas.GetComponent<RectTransform>().TransformPoint(localPoint);
		}

		private void Awake()
		{
			FetchDependencies();
			SetupDebugUI();
		}

		private void FetchDependencies()
		{
#if UNITY_EDITOR
			if (_rectTransform != null)
			{
				// Skip if already initialized - can happen in editor mode due to OnDrawGizmos
				return;
			}
#endif

			_rectTransform = GetComponent<RectTransform>();
			if (_rectTransform == null)
			{
				Debug.LogError("UIFollower requires a RectTransform component!", this);
				enabled = false;
				return;
			}

			_originalAnchor = _rectTransform.anchoredPosition;

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
			if (target == null && !targetMouse)
			{
				return;
			}

			if (targetMouse)
			{
				// For mouse, we already have screen coordinates
				_targetScreenPosition = Input.mousePosition;
			}
			else if (target != null)
			{
				// If target is another UI element
				RectTransform targetRect = target.GetComponent<RectTransform>();
				if (targetRect != null)
				{
					// Get screen position of the target UI element
					_targetScreenPosition = GetScreenPosition(targetRect);
				}
				else
				{
					// Target is a world object, convert to canvas space
					_targetScreenPosition = WorldToCanvasPosition(target.position);
				}
			}
			else
			{
				return;
			}

			_targetScreenPosition += offset;


			Vector2 originPosition = _rectTransform.position;
			Vector2 newPosition = originPosition;
			if (followX)
			{
				newPosition.x = Mathf.Lerp(originPosition.x, _targetScreenPosition.x, easing);
			}

			if (followY)
			{
				newPosition.y = Mathf.Lerp(originPosition.y, _targetScreenPosition.y, easing);
			}

			if (keepDistanceToTarget > 0)
			{
				float distance = Vector2.Distance(newPosition, _targetScreenPosition);
				if (distance < keepDistanceToTarget)
				{
					Vector2 direction = (originPosition - _targetScreenPosition).normalized;
					newPosition = _targetScreenPosition + (direction * keepDistanceToTarget);
				}
			}

			_rectTransform.position = newPosition;
			//SetScreenPosition(_rectTransform, newPosition, _canvas, _camera);

			// Update debug text
			if (showDebugInfo && _debugText != null)
			{
				_debugText.text = $"Mouse: {Input.mousePosition}\n" +
								  	$"Target: {_targetScreenPosition}\n" +
									$"New Position: {newPosition}\n" +
									$"Original Anchor: {_originalAnchor}\n" +
									$"Current Anchor: {_rectTransform.anchoredPosition}\n" +
									$"Current Position: {_rectTransform.position}\n";
			}
		}

		private Vector2 GetCursorCanvasPosition()
		{
			if (_canvas == null)
			{
				return Vector2.zero;
			}

			// Mouse position is already in screen coordinates, convert to canvas space
			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				_canvas.GetComponent<RectTransform>(),
				Input.mousePosition,
				_isCanvasOverlay ? null : _camera,
				out Vector2 localPoint);

			return localPoint;
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
			_rectTransform.anchoredPosition = _originalAnchor;
		}

		private void SetupDebugUI()
		{
			if (!showDebugInfo || _debugText != null)
			{
				return;
			}

			// Create debug text object
			GameObject debugObj = new GameObject("DebugText");
			debugObj.transform.SetParent(transform, false);

			_debugText = debugObj.AddComponent<TextMeshProUGUI>();
			_debugText.fontSize = 44;
			_debugText.color = Color.black;
			_debugText.fontWeight = FontWeight.Bold;
			_debugText.alignment = TextAlignmentOptions.Left;

			RectTransform textRect = _debugText.rectTransform;
			textRect.anchorMin = new Vector2(0, 1);
			textRect.anchorMax = new Vector2(0, 1);
			textRect.pivot = new Vector2(0, 1);
			textRect.anchoredPosition = new Vector2(200, -10);
			textRect.sizeDelta = new Vector2(800, 100);
			textRect.position = new Vector3(textRect.position.x, textRect.position.y, 10);
		}


		private void OnDrawGizmos()
		{
			if (!Application.isPlaying || !enabled)
			{
				return;
			}

			FetchDependencies();

			if (_rectTransform == null || _camera == null)
			{
				return;
			}

			// Convert UI rect transform position to world space for gizmo drawing
			Vector3 myScreenPos = _rectTransform.position;

			// Draw current position
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(ScreenToGizmoPosition(myScreenPos), 0.5f);

			// Draw target position
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(ScreenToGizmoPosition(_targetScreenPosition), new Vector3(1f, 1f, 1f));


			// Draw lines connecting positions
			Gizmos.color = Color.white;
			Gizmos.DrawLine(_targetScreenPosition, myScreenPos);
		}

		private Vector3 ScreenToGizmoPosition(Vector3 screenPosition)
		{
			return _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _camera.nearClipPlane + 1));
		}
	}
}