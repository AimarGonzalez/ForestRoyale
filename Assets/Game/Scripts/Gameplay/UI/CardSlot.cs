using ForestLib.Utils;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gui;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
	public class CardSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public Action<CardSlot, CardData> OnClick;

		[SerializeField] private RectTransform _cardView;
		[SerializeField] private Image _cardPicture;
		[SerializeField] private TMP_Text _cardName;
		[SerializeField] private TMP_Text _elixirCost;

		[SerializeField] private CardData _cardData;

		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[ShowInInspector, Unity.Collections.ReadOnly] private bool _selected = false;

		private UIFollower _mouseFollower;
		private RectTransform _cardRectTransform;
		private RectTransform _slotRectTransform;
		private Vector2 _cardOriginalAnchor;
		private bool _isDragging = false;
		private Camera _camera;

		private float _castingLinePosition;

		// internal values for gizmo drawing
		private float _cardDistanceToLine;
		private float _slotDistanceToLine;
		private bool _debugInitialized = false;

		// dynamic values depending on camera or scene context
		private float CastingLinePosition => _castingLinePosition * _camera.pixelHeight;
		private float CardHeight => _cardRectTransform.rect.height * _cardRectTransform.lossyScale.y;

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
			_mouseFollower = _cardView.GetComponent<UIFollower>();
			_slotRectTransform = GetComponent<RectTransform>();
			_cardRectTransform = _cardView.GetComponent<RectTransform>();
			_cardOriginalAnchor = _cardRectTransform.anchoredPosition;
			_camera = Camera.main;
		}

		public void Init(float castingLinePosition)
		{
			_castingLinePosition = castingLinePosition;
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
			// Do nothing
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
				_mouseFollower.enabled = true;
			}
			else
			{
				_mouseFollower.enabled = false;

				if (_selected)
				{
					_cardRectTransform.anchoredPosition = _cardOriginalAnchor + new Vector2(0, 40);
				}
				else
				{
					_cardRectTransform.anchoredPosition = _cardOriginalAnchor;
				}
			}
		}

		private void Update()
		{
			ReduceSizeWhenApproachingCastingLine();
		}

		private void ReduceSizeWhenApproachingCastingLine()
		{
			_cardDistanceToLine = CastingLinePosition - _cardRectTransform.position.y;
			_slotDistanceToLine = CastingLinePosition - _slotRectTransform.position.y;

			float margin = CardHeight * 0.5f;

			float scale = Mathf.Clamp(_cardDistanceToLine / (_slotDistanceToLine - margin), 0.3f, 1f);
			_cardRectTransform.localScale = new Vector3(scale, scale, scale);
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
					new GUIUtils.Property ("_cardDistanceToLine", _cardDistanceToLine),
					new GUIUtils.Property ("_slotDistanceToLine", _slotDistanceToLine),
					new GUIUtils.Property ("rectTransform.position.y", _cardRectTransform.position.y),
					new GUIUtils.Property ("_camera.pixelHeight", _camera.pixelHeight),
			};

			GUIUtils.DrawDebugPanel(properties, transform, GUIUtils.PanelPlacement.Top);


			//Draw margin line
			GizmoUtils.DrawHorizontalLineOnScreen(_cardRectTransform.position.y + CardHeight * 0.5f, Color.blue);
			GizmoUtils.DrawHorizontalLineOnScreen(_cardRectTransform.position.y, Color.white);
		}
	}
}