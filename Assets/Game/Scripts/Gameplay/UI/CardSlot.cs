using ForestLib.Utils;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gui;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
	[ExecuteInEditMode]
	public class CardSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		private enum State
		{
			NotSelected,
			Selected,
			DraggingCard,
			CastPreview,
		}


		public Action<CardSlot, CardData> OnSelected;
		public Action<CardSlot, CardData> OnEndDragEvent;

		[SerializeField] private RectTransform _cardView;
		[SerializeField] private Image _cardPicture;
		[SerializeField] private TMP_Text _cardName;
		[SerializeField] private TMP_Text _elixirCost;

		[SerializeField] private CardData _cardData;

		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[ShowInInspector, Unity.Collections.ReadOnly]
		private State _state = State.NotSelected;

		private UIFollower _mouseFollower;
		private RectTransform _cardRectTransform;
		private RectTransform _slotRectTransform;
		private Vector2 _cardOriginalAnchor;

		private Camera _camera;

		private float _castingLinePosition;

		// internal values for gizmo drawing
		private float _cardDistanceToLine;
		private float _slotDistanceToLine;
		private float _scale;
		private bool _debugInitialized = false;


		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[SerializeField]
		private bool _showDebugPosition = true;

		// dynamic values depending on camera or scene context
		private float CastingLinePosition => _castingLinePosition * _camera.pixelHeight;
		private float SlotHeigh => _slotRectTransform.rect.height * _slotRectTransform.lossyScale.y;

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

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			Debug.Log($"click on card - {_cardData.CardName}");

			if (_state == State.Selected)
			{
				return;
			}

			switch (_state)
			{
				case State.NotSelected:
					SetState(State.Selected);
					OnSelected?.Invoke(this, _cardData);
					break;

				case State.Selected:
				case State.DraggingCard:
				case State.CastPreview:
					Debug.LogError($"The player shouldn't be able to click on a card in this state ({_state})");
					break;

				default:
					Debug.LogError($"Unknown state {_state}");
					break;
			}
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			StartDragging();
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			// Do nothing
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			OnEndDragEvent?.Invoke(this, _cardData);
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			// Do nothing
		}

		public void Deselect()
		{
			Debug.Assert(_state == State.Selected || _state == State.NotSelected, $"Cannot deselect a slot in state - {_state}");
			SetState(State.NotSelected);
		}

		public void StartDragging()
		{
			Debug.Assert(_state == State.Selected, $"Cannot start dragging a slot in state - {_state}");
			Debug.Log("card - Selected");
			SetState(State.DraggingCard);
		}

		public void StopDragging()
		{
			Debug.Log("card - StopDragging");
			Debug.Log("card - Selected");
			SetState(State.Selected);
		}

		private void SetState(State newState)
		{
			if (_state == newState)
			{
				return;
			}

			// On Exit State
			switch (_state)
			{
				case State.NotSelected:
					break;
				case State.Selected:
					break;
				case State.DraggingCard:
					_mouseFollower.enabled = false;
					break;
				case State.CastPreview:
					_cardView.gameObject.SetActive(true);
					break;
			}

			_state = newState;

			// On Enter State
			switch (newState)
			{
				case State.NotSelected:
					_cardRectTransform.anchoredPosition = _cardOriginalAnchor;
					break;
				case State.Selected:
					_cardRectTransform.anchoredPosition = _cardOriginalAnchor + new Vector2(0, 40);
					break;
				case State.DraggingCard:
					_mouseFollower.enabled = true;
					break;
				case State.CastPreview:
					_cardView.gameObject.SetActive(false);
					break;
			}
		}

		private void Update()
		{
			// Update state
			switch (_state)
			{
				case State.DraggingCard:
					if (IsDraggingAboveCastingLine())
					{
						SetState(State.CastPreview);
					}
					break;

				case State.CastPreview:
					if (IsDraggingUnderCastingLine())
					{
						SetState(State.DraggingCard);
					}
					break;
			}

			// Update view
			switch (_state)
			{
				case State.NotSelected:
				case State.Selected:
					// do nothing
					break;

				case State.DraggingCard:
					ReduceSizeWhenApproachingCastingLine();
					break;
				case State.CastPreview:
					UpdateSpawnPreview();
					break;
			}
		}

		private bool IsDraggingUnderCastingLine()
		{
			return Input.mousePosition.y <= CastingLinePosition;
		}

		private bool IsDraggingAboveCastingLine()
		{
			return Input.mousePosition.y > CastingLinePosition;
		}

		private void ReduceSizeWhenApproachingCastingLine()
		{
			_cardDistanceToLine = CastingLinePosition - _cardRectTransform.position.y;
			_slotDistanceToLine = CastingLinePosition - _slotRectTransform.position.y;

			float margin = SlotHeigh * 0.5f;
			float ratio = _cardDistanceToLine / (_slotDistanceToLine - margin);
			_scale = Mathf.Lerp(0.3f, 1f, ratio);
			_cardRectTransform.localScale = new Vector3(_scale, _scale, 1f);
		}

		private void UpdateSpawnPreview()
		{
			if (_cardData == null)
			{
				Debug.LogError("CardSlot - UpdateSpawnPreview: _cardData is null");
				return;
			}


			/*
			switch (_cardData.CardType)
			{
				case CardType.Character:
					break;
				case CardType.Spell:
					break;
				case CardType.Building:
					break;
			}
			*/
		}

		private void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				DebugInit();
			}

			//Draw margin line
			GizmoUtils.DrawHorizontalLineOnScreen(_slotRectTransform.position.y + SlotHeigh * 0.5f, Color.blue);
			GizmoUtils.DrawHorizontalLineOnScreen(_slotRectTransform.position.y, Color.white);
		}

		void OnGUI()
		{
			DrawDebugGUI();
		}

		private void DrawDebugGUI()
		{
			if (!_showDebugPosition)
			{
				return;
			}

			GUIUtils.Property[] properties = {
				new ("_state", _state.ToString()),
				//	new ("_castingLine", _castingLinePosition),
				//	new ("_scale", _scale),
				//	new ("_slotDistanceToLine", _slotDistanceToLine),
				//	new ("_cardDistanceToLine", _cardDistanceToLine),
			};

			GUIUtils.DrawDebugPanel(properties, _cardView, GUIUtils.PanelPlacement.Top);
		}
	}
}