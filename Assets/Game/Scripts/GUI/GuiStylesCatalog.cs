using UnityEngine;

namespace ForestRoyale.Gui
{
	public class GuiStylesCatalog
	{
		private static Texture2D s_blackTransparentTexture;
		private static GUIStyle s_blackBoxStyle;
		private static GUIStyle s_debugPanelStyle;
		private static GUIStyle s_labelBlueStyle;
		private static GUIStyle s_labelGreenStyle;
		private static GUIStyle s_labelRedStyle;


		// Static initializer
		static GuiStylesCatalog()
		{
			s_blackBoxStyle = null;
			s_debugPanelStyle = null;
			s_blackTransparentTexture = null;
			s_labelBlueStyle = null;
			s_labelGreenStyle = null;
			s_labelRedStyle = null;
		}

		public static GUIStyle BlackBoxStyle
		{
			// Lazy initialization because GUIStyle can only be initialized during OnGUI
			get
			{
				InitializeTexture();
				
				s_blackBoxStyle ??= new GUIStyle(GUI.skin.box)
				{
					normal = {background = s_blackTransparentTexture}
				};
				return s_blackBoxStyle;
			}
		}

		public static GUIStyle DebugPanelStyle
		{
			// Lazy initialization because GUIStyle can only be initialized during OnGUI
			get
			{
				InitializeTexture();
				
				s_debugPanelStyle ??= new GUIStyle()
				{
					fontSize = 12,
					alignment = TextAnchor.MiddleLeft,
					wordWrap = false,
					margin = GUI.skin.box.margin,
					padding = new RectOffset(5, 5, 5, 5),
					border = new RectOffset(4, 4, 4, 4),
					normal =
					{
						textColor = Color.white,
						background = s_blackTransparentTexture,
					}
				};

				return s_debugPanelStyle;
			}
		}

		public static GUIStyle LabelBlueStyle
		{
			get
			{
				s_labelBlueStyle ??= new GUIStyle(GUI.skin.label) { normal = { textColor = Color.blue } };
				return s_labelBlueStyle;
			}
		}

		public static GUIStyle LabelGreenStyle
		{
			get
			{
				s_labelGreenStyle ??= new GUIStyle(GUI.skin.label) { normal = { textColor = Color.green } };
				return s_labelGreenStyle;
			}
		}

		public static GUIStyle LabelRedStyle
		{
			get
			{
				s_labelRedStyle ??= new GUIStyle(GUI.skin.label) { normal = { textColor = Color.red } };			
				return s_labelRedStyle;
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