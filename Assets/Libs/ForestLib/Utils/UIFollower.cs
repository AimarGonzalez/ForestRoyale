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

		[Header("Target")]
		[Tooltip("Whether to follow the mouse/touch position instead of a target transform")]
		[SerializeField] private TargetMode _targetMode = TargetMode.Mouse;

		[Tooltip("The target to follow"), ShowIf("_targetMode", TargetMode.Object)]
		[SerializeField] private Transform _targetObject;

		[BoxGroup("Easing settings"), PropertyOrder(3)]
		[Tooltip("How quickly to move toward the target")]
		[Range(1f, 100f)]
		[SerializeField] private float _easing = 6f;

		// TimeToTarget is a gentle approximation of the time it takes to reach the target. 
		// On the estimation time the follower will be at 99.9% of the distance.
		// Consider implementing a snapping mechanism if this is not sufficient.
		[BoxGroup("Easing settings"), PropertyOrder(4)]
		[ShowInInspector, ReadOnly]
		[SuffixLabel("sec.")]
		[Tooltip("$" + nameof(TimeToTargetTooltip))]
		public float TimeToTarget => 6f / _easing;
		private string TimeToTargetTooltip => $"{TimeToTarget} is an aproximation.\n" +
											  $"The follower reaches 99% of the total distance in {TimeToTarget} seconds, " +
											  $"but it will really take {12f / _easing} seconds to completely reach the target.";

		[BoxGroup("Easing settings"), PropertyOrder(5)]
		[Tooltip("Whether to follow on the X axis")]
		[SerializeField] private bool _followX = true;

		[BoxGroup("Easing settings"), PropertyOrder(6)]
		[Tooltip("Whether to follow on the Y axis")]
		[SerializeField] private bool _followY = true;

		[BoxGroup("Easing settings"), PropertyOrder(7)]
		[Tooltip("Offset from the target position")]
		[SerializeField] private Vector2 _offset = Vector2.zero;

		[BoxGroup("Easing settings"), PropertyOrder(8)]
		[Tooltip("Minimum distance to keep from target")]
		[SerializeField] private float _keepDistanceToTarget = 0f;

		[BoxGroup("Update mode"), PropertyOrder(20)]
		[Tooltip("When to run the follow logic")]
		[SerializeField] private UpdateType _updateType = UpdateType.Update;

		[Tooltip("Whether the movement is affected by time scale")]
		[BoxGroup("Update mode"), PropertyOrder(21)]
		[SerializeField] private bool _affectedByTimeScale = true;


		[BoxGroup("Advanced"), PropertyOrder(30)]
		[Tooltip("Camera used for UI canvas calculations (defaults to canvas camera or main camera)")]
		[SerializeField] private Camera _canvasCamera;

		[BoxGroup("Advanced"), PropertyOrder(31)]
		[Tooltip("Show debug information in game view")]
		[SerializeField]
		[OnValueChanged("SetupDebugUI")]
		private bool _showDebugInfo = false;

		private RectTransform _rectTransform;
		private Canvas _canvas;
		private bool _isCanvasOverlay;
		private Camera _camera;
		private TextMeshProUGUI _debugText;

		private Vector2 _targetScreenPos;

		public Transform Target
		{
			get => _targetObject;
			set => _targetObject = value;
		}

		public float Easing
		{
			get => _easing;
			set => _easing = Mathf.Clamp01(value);
		}

		public Vector2 Offset
		{
			get => _offset;
			set => _offset = value;
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
			if (!_isCanvasOverlay && _canvas.renderMode == RenderMode.ScreenSpaceCamera && _canvasCamera == null)
			{
				_canvasCamera = _canvas.worldCamera;
			}

			// Ensure we have a camera reference for calculations
			_camera = _canvasCamera != null ? _canvasCamera : Camera.main;
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

		private bool HasValidTarget()
		{
			return _targetMode == TargetMode.Mouse || _targetObject != null;
		}

		private void FollowTarget(float easing)
		{
			if (!HasValidTarget())
			{
				return;
			}

			_targetScreenPos = GetTargetScreenPosition();

			_targetScreenPos += _offset;

			// Interpolate to target position
			Vector2 originPosition = _rectTransform.position;
			Vector2 newPosition = originPosition;

			float deltaTime = _affectedByTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;

			// This formula ensures the movement is consistent regardless of the frame rate
			float consistentEasing = 1f - Mathf.Exp(-deltaTime * easing);

			if (_followX)
			{
				newPosition.x = Mathf.Lerp(originPosition.x, _targetScreenPos.x, consistentEasing);
			}

			if (_followY)
			{
				newPosition.y = Mathf.Lerp(originPosition.y, _targetScreenPos.y, consistentEasing);
			}

			// Keep distance to target
			if (_keepDistanceToTarget > 0)
			{
				float distance = Vector2.Distance(newPosition, _targetScreenPos);
				if (distance < _keepDistanceToTarget)
				{
					Vector2 direction = (originPosition - _targetScreenPos).normalized;
					newPosition = _targetScreenPos + (direction * _keepDistanceToTarget);
				}
			}

			_rectTransform.position = new Vector3(newPosition.x, newPosition.y, _rectTransform.position.z);

			// Update debug text
			if (_showDebugInfo && _debugText != null)
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
			else if (_targetObject != null)
			{
				// If target is another UI element
				RectTransform targetRect = _targetObject.GetComponent<RectTransform>();
				if (targetRect != null)
				{
					targetScreenPos = targetRect.position; // screen space
				}
				else
				{
					targetScreenPos = _camera.WorldToScreenPoint(_targetObject.position);
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
			if (!_showDebugInfo || _debugText != null)
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

		public void JumpToTargetPosition()
		{
			FollowTarget(9999f);
		}
	}
}