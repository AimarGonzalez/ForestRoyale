using UnityEngine;

namespace ForestRoyale.Gui
{
	public class GuiStylesCatalog
	{
		public static GUIStyle BlackBoxStyle;
		public static GUIStyle DebugPanelStyle;

		private static Texture2D s_blackTransparentTexture;


		// Static initializer
		static GuiStylesCatalog()
		{
			if (BlackBoxStyle == null)
			{
				s_blackTransparentTexture = new Texture2D(1, 1);
				s_blackTransparentTexture.SetPixel(0, 0, new Color(0, 0, 0, 1f));
				s_blackTransparentTexture.Apply();

				Texture2D blackTexture = new Texture2D(1, 1);
				blackTexture.SetPixel(0, 0, Color.black);
				blackTexture.Apply();

				BlackBoxStyle = new GUIStyle(GUI.skin.box)
				{
					normal =
					{
						background = s_blackTransparentTexture
					}
				};

				DebugPanelStyle = new GUIStyle(GUI.skin.box)
				{
					fontSize = 12,
					alignment = TextAnchor.MiddleLeft,
					normal =
					{
						textColor = Color.white,
						//background = blackTexture
					}
				};
			}
		}
	}
}