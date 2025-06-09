using ForestLib.ExtensionMethods;
using ForestLib.Utils;
using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Combat;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

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
			Empty,
		}


		public Action<CardSlot, CardData> OnSelected;
		public Action<CardSlot, CardData> OnEndDragEvent;

		// TODO: Refactor - move picture, name and elixir into a new component CardView.
		[SerializeField] private RectTransform _cardView;
		[SerializeField] private Image _cardPicture;
		[SerializeField] private TMP_Text _cardName;
		[SerializeField] private TMP_Text _elixirCost;

		[SerializeField] private CardData _cardData;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[ShowInInspector, Unity.Collections.ReadOnly]
		private State _state = State.NotSelected;

		private Camera _camera;
		private Canvas _canvas;

		// Card scaling
		private UIFollower _mouseFollower;
		private RectTransform _cardRectTransform;
		private RectTransform _slotRectTransform;
		private Vector2 _cardOriginalAnchor;

		// Card casting
		private float _castingLinePosition;
		
		[Inject]
		private CardCastingViewFactory _cardCastingViewFactory;
		private ICastingView _castingView;
		
		// Gizmo: internal values for gizmo drawing
		private float _mouseDistanceToLine;
		private float _slotDistanceToLine;
		private float _scale;
		private bool _debugInitialized = false;

		public ICastingView CastingView => _castingView;


		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[SerializeField]
		private bool _showDebugPosition = true;

		private const float SELECTION_OFFSET = 40;
		private const int DRAG_SORTING_ORDER = 1;
		private Vector2 _touchOffset;
		private bool _forceJumpToMousePosition;
		private bool _renderOnTop;

		private bool RenderOnTop
		{
			get => _renderOnTop;
			set
			{
				_renderOnTop = value;
				if (value)
				{
					_canvas.overrideSorting = true;
					_canvas.sortingOrder = DRAG_SORTING_ORDER;
				}
				else
				{
					_canvas.sortingOrder = 0;
					_canvas.overrideSorting = false;
				}
			}
		}


		// dynamic values depending on camera or scene context

		private float CastingLinePosition => _castingLinePosition * _camera.pixelHeight;

		private float SlotHeigh => _slotRectTransform.rect.height * _slotRectTransform.lossyScale.y;

		public bool IsCastPreviewVisible => _state == State.CastPreview;

		public CardData CardData
		{
			get => _cardData;
			set
			{
				_cardData = value;
				PopulateCardView();
			}
		}
		
		public void Clear()
		{
			CardData = null;
		}

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_slotRectTransform = GetComponent<RectTransform>();
			_camera = Camera.main;

			_mouseFollower = _cardView.GetComponent<UIFollower>();
			_cardRectTransform = _cardView.GetComponent<RectTransform>();
			_cardOriginalAnchor = _cardRectTransform.anchoredPosition;
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
			PopulateCardView();
		}

		private void OnValidate()
		{
			PopulateCardView();
		}

		private void PopulateCardView()
		{
			if (_cardData == null)
			{
				_cardView.gameObject.SetActive(false);
				return;
			}

			_cardView.gameObject.SetActive(true);

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
			if (_cardData)
			{
				Debug.Log($"click on card - {_cardData.CardName}");
			}
			else
			{
				Debug.Log("click on empty slot");
				return;
			}

			switch (_state)
			{
				case State.NotSelected:
					_touchOffset = eventData.pressPosition - _cardView.position.xy();
					SetState(State.Selected);
					OnSelected?.Invoke(this, _cardData);
					break;

				case State.Selected:
					_touchOffset = eventData.pressPosition - _cardView.position.xy();
					break;
				
				case State.Empty:
					// do nothing
					return;

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
			switch (_state)
			{
				case State.Selected:
				case State.DraggingCard:
				case State.CastPreview:
					StartDragging();
					break;

				case State.Empty:
					// do nothing
					break;
				
				case State.NotSelected:
					Debug.LogError($"The player shouldn't be able to drag a card in this state ({_state})");
					break;

				default:
					Debug.LogError($"Unknown state {_state}");
					break;
			}
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
			Debug.Assert(_state == State.Selected || _state == State.NotSelected || _state == State.Empty, $"Cannot deselect a slot in state - {_state}");
			if (_state == State.Selected)
			{
				SetState(State.NotSelected);
			}
		}

		public void StartDragging()
		{
			if (_state == State.Selected)
			{
				Debug.Log("card - StartDragging");
				SetState(State.DraggingCard);
			}
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

			State oldState = _state;
			_state = newState;

			// On Exit State
			switch (oldState)
			{
				case State.Empty:
					break;
				case State.NotSelected:
					break;
				case State.Selected:
					break;
				case State.DraggingCard:
					RenderOnTop = false;
					_mouseFollower.enabled = false;
					_scale = 1f;
					ApplyScale();
					break;
				case State.CastPreview:
					_castingView.SetActive(false);
					_cardView.gameObject.SetActive(true);
					break;
			}



			// On Enter State
			switch (newState)
			{
				case State.NotSelected:
					
					_cardView.gameObject.SetActive(true);
					_cardRectTransform.anchoredPosition = _cardOriginalAnchor;
					break;

				case State.Selected:
					// TODO: replace offset with a PlaySelectFX 
					_cardRectTransform.anchoredPosition = _cardOriginalAnchor + new Vector2(0, SELECTION_OFFSET);
					break;

				case State.DraggingCard:
					RenderOnTop = true;
					_mouseFollower.enabled = true;
					_forceJumpToMousePosition = true;
					break;

				case State.CastPreview:
					_cardView.gameObject.SetActive(false);
					
					//TODO: pool casting views, instead of caching them
					_castingView ??= _cardCastingViewFactory.BuildCastingPreview(_cardData, ArenaTeam.Player, _castingView);
					_castingView.SetActive(true);
					break;

				case State.Empty:
					_cardView.gameObject.SetActive(false);
					_castingView.SetActive(false);
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
					if (_forceJumpToMousePosition)
					{
						_mouseFollower.JumpToTargetPosition();
						_forceJumpToMousePosition = false;
					}
					break;
				
				case State.CastPreview:
					// managed in ICastingView
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
			_mouseDistanceToLine = CastingLinePosition - Input.mousePosition.y;
			_slotDistanceToLine = CastingLinePosition - _slotRectTransform.position.y;
			_slotDistanceToLine = Mathf.Max(_slotDistanceToLine, 0.001f);

			float margin = SlotHeigh * 0.5f;
			float safeSlotDistance = Mathf.Max(Mathf.Epsilon, _slotDistanceToLine - margin);
			float ratio = (_mouseDistanceToLine) / safeSlotDistance;
			_scale = Mathf.Lerp(0.3f, 1f, ratio);

			ApplyScale();
		}

		private void ApplyScale()
		{
			_cardRectTransform.localScale = new Vector3(_scale, _scale, 1f);
			
			// apply scale to follower to keep the card centered on the mouse position
			_mouseFollower.Offset = -_touchOffset.xy() * _scale;
		}
		

		public void Cast(Transform _charactersRoot)
		{
			_castingView.Cast(_charactersRoot);

			// TODO: change State to State.Casting
			// TODO: make async to wait for casting to finish
			SetState(State.Empty);
			CardData = null;
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