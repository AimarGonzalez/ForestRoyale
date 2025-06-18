using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ForestLib.UI
{
	public class UIPanelShaderProperties : UIBehaviour, IMaterialModifier
	{
		private static readonly int s_aspectRatioPropertyId = Shader.PropertyToID("_AspectRatio");
		
		private Graphic _graphic;
		private RectTransform _rectTransform;
		
		
		private RectTransform RectTransform
		{
			get
			{
				if (_rectTransform == null)
				{
					_rectTransform = GetComponent<RectTransform>();
				}
				return _rectTransform;
			}
		}

		private Graphic Graphic
		{
			get
			{
				if (_graphic == null)
				{
					_graphic = GetComponent<Graphic>();
				}
				return _graphic;
			}
		}

		[ShowInInspector, ReadOnly]
		public float AspectRatio => RectTransform.rect.height / RectTransform.rect.width;

		protected override void Start()
		{
			base.Start();
			FetchDependencies();
			UpdateMaterial();
		}
		
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			UpdateMaterial();
		}

		private void FetchDependencies()
		{
			_rectTransform = GetComponent<RectTransform>();
			_graphic = GetComponent<Graphic>();
		}

		[Button("Update Material")]
		private void UpdateMaterialButton()
		{
			FetchDependencies();
			UpdateMaterial();
		}
		
		private void UpdateMaterial()
		{
			if (Application.isPlaying)
			{
				Graphic.SetMaterialDirty();
			}
			else
			{
				Graphic.material.SetFloat(s_aspectRatioPropertyId, AspectRatio);
			}

		}

		public Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = new Material(baseMaterial);

			if (material.HasFloat(s_aspectRatioPropertyId))
			{
				material.SetFloat(s_aspectRatioPropertyId, AspectRatio);
			}

			return material;
		}
	}
}
