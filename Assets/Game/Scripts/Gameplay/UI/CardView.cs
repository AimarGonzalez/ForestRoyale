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

		[SerializeField] private Image cardPicture;
		[SerializeField] private TMP_Text cardName;
		[SerializeField] private TMP_Text elixirCost;

		[SerializeField] private CardData _cardData;

		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[ShowInInspector, Unity.Collections.ReadOnly] private bool _selected = false;

		private UIFollower _follower;
		private RectTransform _rectTransform;
		private Vector2 _originalPosition;
		private bool _isDragging = false;

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

			if (cardPicture != null)
			{
				cardPicture.sprite = _cardData.Portrait;
			}

			if (cardName != null)
			{
				cardName.text = _cardData.CardName;
			}

			if (elixirCost != null)
			{
				elixirCost.text = _cardData.ElixirCost.ToString();
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
				_follower.TargetCursor = true;
				_follower.enabled = true;
			}
			else
			{
				_follower.TargetCursor = false;
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
	}
}