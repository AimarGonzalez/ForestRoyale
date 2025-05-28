using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestRoyale.Core.UI
{
	[DefaultExecutionOrder(-1000)]
	[ExecuteInEditMode]
	public class CustomGUIStyles : MonoBehaviour
	{
		[SerializeField]
		[OnValueChanged(nameof(SetCatalogStyles))]
		private GUISkin _customSkin;

		private void Awake()
		{
			SetCatalogStyles();
		}

		[Button("Re-apply Styles")]
		private void SetCatalogStyles()
		{
			GuiStylesCatalog.DebugLabelStyle = _customSkin.label;
			GuiStylesCatalog.DebugPanelStyle = _customSkin.box;
			GuiStylesCatalog.DebugTextFieldStyle = _customSkin.textField;
		}
	}
}
