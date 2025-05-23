using ForestRoyale.Gameplay.Cards;
using ForestLib.Utils;
using ForestRoyale.Gui;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Game.UI
{
	public class CardView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public Action<CardView, CardData> OnClick;

		[SerializeField] private Image _cardPicture;
		[SerializeField] private TMP_Text _cardName;
		[SerializeField] private TMP_Text _elixirCost;

		[SerializeField] private CardData _cardData;

		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[ShowInInspector, Unity.Collections.ReadOnly] private bool _selected = false;

		private UIFollower _follower;
		private RectTransform _rectTransform;
		private Vector2 _originalPosition;
		private bool _isDragging = false;
		private Camera _camera;

		private float _castingLinePosition;
		private float _initialDistanceToCastingLine;

		private bool _debugInitialized = false;

		public CardData CardData
		{
			get => _cardData;
			set
			{
				_cardData = value;
				UpdateView();
			}
		}

		private void Awake()
		{
			_follower = GetComponent<UIFollower>();
			_rectTransform = GetComponent<RectTransform>();
			_originalPosition = _rectTransform.anchoredPosition;
			_camera = Camera.main;
		}

		public void Init(float castingLinePosition)
		{
			_castingLinePosition = castingLinePosition;
			_initialDistanceToCastingLine = DistanceToCastingLine(_rectTransform);
		}

		private void DebugInit()
		{
			if (_debugInitialized)
			{
				return;
			}
			
			Awake();
			Init(0.26f);
			_debugInitialized = true;
		}

		private void Start()
		{
			UpdateView();
		}

		private void OnValidate()
		{
			UpdateView();
		}

		private void UpdateView()
		{
			if (_cardData == null)
			{
				return;
			}

			if (_cardPicture != null)
			{
				_cardPicture.sprite = _cardData.Portrait;
			}

			if (_cardName != null)
			{
				_cardName.text = _cardData.CardName;
			}

			if (_elixirCost != null)
			{
				_elixirCost.text = _cardData.ElixirCost.ToString();
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Debug.Log($"click on card - {_cardData.CardName}");
			OnClick?.Invoke(this, _cardData);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			StartDragging();
		}

		public void OnDrag(PointerEventData eventData)
		{
			// Do nothing
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			StopDragging();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			StopDragging();
		}

		public void SetSelected(bool selected)
		{
			_selected = selected;
			UpdatePosition();
		}

		public void StartDragging()
		{
			_isDragging = true;
			UpdatePosition();
		}

		public void StopDragging()
		{
			_isDragging = false;
			UpdatePosition();
		}

		private void UpdatePosition()
		{
			if (_isDragging)
			{
				_follower.TargetMouse = true;
				_follower.enabled = true;
			}
			else
			{
				_follower.TargetMouse = false;
				_follower.enabled = false;

				if (_selected)
				{
					_rectTransform.anchoredPosition = _originalPosition + new Vector2(0, 40);
				}
				else
				{
					_rectTransform.anchoredPosition = _originalPosition;
				}
			}
		}

		private void Update()
		{
			ReduceSizeWhenApproachingCastingLine();
		}

		private void ReduceSizeWhenApproachingCastingLine()
		{
			float distance = DistanceToCastingLine(_rectTransform);
			float scale = Mathf.Clamp(distance / _initialDistanceToCastingLine, 0.0f, 1f);
			_rectTransform.localScale = new Vector3(scale, scale, scale);
		}

		private float DistanceToCastingLine(RectTransform rectTransform)
		{
			float verticalPosition = rectTransform.position.y / _camera.pixelHeight;
			float distance = _castingLinePosition - verticalPosition;
			return distance;
		}

		private void OnDrawGizmos()
		{
			//if (!_isDragging)
			//{
			//	return;
			//}

			if (!Application.isPlaying)
			{
				DebugInit();
			}

			GUIUtils.Property[] properties = new[] {
					new GUIUtils.Property ("_castingLine", _castingLinePosition),
					new GUIUtils.Property ("_initialDistanceToCastingLine", _initialDistanceToCastingLine),
					new GUIUtils.Property ("_distanceToCastingLine", DistanceToCastingLine(_rectTransform)),
					new GUIUtils.Property ("rectTransform.position.y", _rectTransform.position.y),
					new GUIUtils.Property ("_camera.pixelHeight", _camera.pixelHeight),
			};
			
			GUIUtils.DrawDebugPanel(properties, transform, GUIUtils.PanelPlacement.Top);
		}
	}
}