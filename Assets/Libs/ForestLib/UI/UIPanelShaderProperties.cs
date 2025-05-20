using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestLib.UI
{
	public class UIPanelShaderProperties : MonoBehaviour
	{
		private RectTransform _rectTransform;
		private RectTransform RectTransform
		{
			get
			{
				if (_rectTransform == null)
				{
					FetchDependencies();
				}
				return _rectTransform;
			}
		}


		private CanvasRenderer _canvasRenderer;

		[ShowInInspector, ReadOnly]
		public float AspectRatio => RectTransform.rect.height / RectTransform.rect.width;

		private void Start()
		{
			FetchDependencies();
		}

		private void FetchDependencies()
		{
			_rectTransform = GetComponent<RectTransform>();
			_canvasRenderer = GetComponent<CanvasRenderer>();
		}

		private void Update()
		{
			UpdateMaterial();
		}


		private void UpdateMaterial()
		{
			_canvasRenderer.GetMaterial().SetFloat("_AspectRatio", AspectRatio);
		}

		[Button("Update Material")]
		private void UpdateMaterialButton()
		{
			FetchDependencies();
			UpdateMaterial();
		}
	}
}
