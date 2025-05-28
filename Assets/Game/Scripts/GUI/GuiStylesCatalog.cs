using ForestLib.Utils;
using UnityEngine;

namespace ForestRoyale.Gui
{
	public class GuiStylesCatalog
	{
		private static Texture2D s_blackTransparentTexture;

		private static GUIStyle s_labelBlueStyle;
		private static GUIStyle s_labelGreenStyle;
		private static GUIStyle s_labelRedStyle;
		private static GUIStyle s_labelYellowStyle;
		private static GUIStyle s_labelOrangeStyle;
		private static GUIStyle s_labelPurpleStyle;
		private static GUIStyle s_labelPinkStyle;


		private static GUIStyle s_debugLabelStyle;
		private static GUIStyle s_debugPanelStyle;
		private static GUIStyle s_debugTextFieldStyle;
		public static GUIStyle DebugLabelStyle
		{
			get
			{
				s_debugLabelStyle ??= GUI.skin.label;
				return s_debugLabelStyle;
			}
			set
			{
				s_debugLabelStyle = value;
			}
		}

		public static GUIStyle DebugPanelStyle
		{
			get
			{
				s_debugPanelStyle ??= GUI.skin.box;
				return s_debugPanelStyle;
			}
			set
			{
				s_debugPanelStyle = value;
			}
		}

		public static GUIStyle DebugTextFieldStyle
		{
			get
			{
				s_debugTextFieldStyle ??= GUI.skin.textField;
				return s_debugTextFieldStyle;
			}
			set
			{
				s_debugTextFieldStyle = value;
			}
		}

		// Static initializer
		static GuiStylesCatalog()
		{
			s_blackTransparentTexture = null;
			s_labelBlueStyle = null;
			s_labelGreenStyle = null;
			s_labelRedStyle = null;
		}

		public static GUIStyle LabelBlueStyle
		{
			get
			{
				s_labelBlueStyle ??= new GUIStyle(DebugTextFieldStyle) { normal = { textColor = Color.blue } };
				return s_labelBlueStyle;
			}
		}

		public static GUIStyle LabelGreenStyle
		{
			get
			{
				s_labelGreenStyle ??= new GUIStyle(DebugTextFieldStyle) { normal = { textColor = Color.green } };
				return s_labelGreenStyle;
			}
		}

		public static GUIStyle LabelRedStyle
		{
			get
			{
				s_labelRedStyle ??= new GUIStyle(DebugTextFieldStyle) { normal = { textColor = Color.red } };
				return s_labelRedStyle;
			}
		}

		public static GUIStyle LabelYellowStyle
		{
			get
			{
				s_labelYellowStyle ??= new GUIStyle(DebugTextFieldStyle) { normal = { textColor = Color.yellow } };
				return s_labelYellowStyle;
			}
		}

		public static GUIStyle LabelOrangeStyle
		{
			get
			{
				s_labelOrangeStyle ??= new GUIStyle(DebugTextFieldStyle) { normal = { textColor = ColorConstants.Orange } };
				return s_labelOrangeStyle;
			}
		}

		public static GUIStyle LabelPurpleStyle
		{
			get
			{
				s_labelPurpleStyle ??= new GUIStyle(DebugTextFieldStyle) { normal = { textColor = ColorConstants.Purple } };
				return s_labelPurpleStyle;
			}
		}

		public static GUIStyle LabelPinkStyle
		{
			get
			{
				s_labelPinkStyle ??= new GUIStyle(DebugTextFieldStyle) { normal = { textColor = ColorConstants.Pink } };
				return s_labelPinkStyle;
			}
		}

		public static void InitializeTexture()
		{
			if (s_blackTransparentTexture != null)
			{
				return;
			}

			s_blackTransparentTexture = Resources.Load<Texture2D>("GUI/square-16px-4r-black-50t-solid");
		}
	}
}