using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace ForestLib.Utils
{
	public class UIFollower : MonoBehaviour
	{
		public enum TargetMode
		{
			Mouse,
			Object,
		}

		[Tooltip("Whether to follow the mouse/touch position instead of a target transform")]
		[SerializeField] private TargetMode _targetMode = TargetMode.Mouse;

		[Tooltip("The target to follow")]
		[SerializeField] private Transform targetObject;


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
		private TextMeshProUGUI _debugText;

		private Vector2 _targetScreenPos;

		public Transform Target
		{
			get => targetObject;
			set => targetObject = value;
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
		
		private bool HasValidTarget()
		{
			return _targetMode == TargetMode.Mouse || targetObject != null;
		}

		private void FollowTarget()
		{
			if (!HasValidTarget())
			{
				return;
			}

			_targetScreenPos = GetTargetScreenPosition();

			_targetScreenPos += offset;

			// Interpolate to target position
			Vector2 originPosition = _rectTransform.position;
			Vector2 newPosition = originPosition;
			if (followX)
			{
				newPosition.x = Mathf.Lerp(originPosition.x, _targetScreenPos.x, easing);
			}

			if (followY)
			{
				newPosition.y = Mathf.Lerp(originPosition.y, _targetScreenPos.y, easing);
			}

			// Keep distance to target
			if (keepDistanceToTarget > 0)
			{
				float distance = Vector2.Distance(newPosition, _targetScreenPos);
				if (distance < keepDistanceToTarget)
				{
					Vector2 direction = (originPosition - _targetScreenPos).normalized;
					newPosition = _targetScreenPos + (direction * keepDistanceToTarget);
				}
			}

			_rectTransform.position = newPosition;

			// Update debug text
			if (showDebugInfo && _debugText != null)
			{
				_debugText.text = $"Mouse: {Input.mousePosition:F0}\n" +
								  $"Target: {_targetScreenPos:F0}\n" +
								  $"New Position: {newPosition:F0}\n";
			}
		}

		private Vector2 GetTargetScreenPosition()
		{
			Vector2 targetScreenPos = Vector2.zero;
			if (_targetMode == TargetMode.Mouse)
			{
				// For mouse, we already have screen coordinates
				targetScreenPos = Input.mousePosition;
			}
			else if (targetObject != null)
			{
				// If target is another UI element
				RectTransform targetRect = targetObject.GetComponent<RectTransform>();
				if (targetRect != null)
				{
					targetScreenPos = targetRect.position; // screen space
				}
				else
				{
					targetScreenPos = _camera.WorldToScreenPoint(targetObject.position);
				}
			}

			return targetScreenPos;
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

			// Convert screen space positions to world space for gizmo drawing
			Vector3 originScreenPos = _rectTransform.position; //screen space
			Vector3 originGizmoPos = ScreenToGizmoPosition(originScreenPos);
			Vector3 targetGizmoPos = ScreenToGizmoPosition(_targetScreenPos);

			// Draw current position
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(originGizmoPos, 0.5f);

			// Draw target position
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(targetGizmoPos, new Vector3(0.5f, 0.5f, 0.5f));

			// Draw lines connecting positions
			Gizmos.color = Color.white;
			Gizmos.DrawLine(originGizmoPos, targetGizmoPos);
		}

		private Vector3 ScreenToGizmoPosition(Vector3 screenPosition)
		{
			return _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _camera.nearClipPlane + 1));
		}
	}
}