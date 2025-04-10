using UnityEngine;

namespace ForestRoyale.Gui
{
	public class GuiStylesCatalog
	{
		private static GUIStyle s_blackBoxStyle;
		private static GUIStyle s_debugPanelStyle;

		private static Texture2D s_blackTransparentTexture;

		// Static initializer
		static GuiStylesCatalog()
		{
			s_blackBoxStyle = null;
			s_debugPanelStyle = null;
			s_blackTransparentTexture = null;
		}

		public static GUIStyle BlackBoxStyle
		{
			// Lazy initialization because GUIStyle can only be initialized during OnGUI
			get
			{
				InitializeStyles();
				return s_blackBoxStyle;
			}
		}

		public static GUIStyle DebugPanelStyle
		{
			// Lazy initialization because GUIStyle can only be initialized during OnGUI
			get
			{
				InitializeStyles();
				return s_debugPanelStyle;
			}
		}

		public static void InitializeStyles()
		{
			if (s_blackBoxStyle != null)
			{
				return;
			}
			
			s_blackTransparentTexture = Resources.Load<Texture2D>("GUI/square-16px-4r-black-50t-solid");
			
			s_blackBoxStyle = new GUIStyle(GUI.skin.box)
			{
				normal =
				{
					background = s_blackTransparentTexture
				}
			};

			s_debugPanelStyle = new GUIStyle()
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
		}
	}
}