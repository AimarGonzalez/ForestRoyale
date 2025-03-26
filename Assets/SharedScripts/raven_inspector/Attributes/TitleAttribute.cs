using UnityEngine;

namespace Raven.Attributes
{
	public class TitleAttribute : PropertyAttribute
	{
		public string Title { get; private set; }
		public string Subtitle { get; private set; }
		public bool Bold { get; private set; }
		public bool Italic { get; private set; }
		public bool Underline { get; private set; }
		public bool Strikethrough { get; private set; }
		public bool TitleColor { get; private set; }
		public Color Color { get; private set; }

		public TitleAttribute(string title, string subtitle = null, bool bold = true, bool italic = false, bool underline = false, bool strikethrough = false, bool titleColor = false, float r = 0, float g = 0, float b = 0)
		{
			Title = title;
			Subtitle = subtitle;
			Bold = bold;
			Italic = italic;
			Underline = underline;
			Strikethrough = strikethrough;
			TitleColor = titleColor;
			Color = new Color(r, g, b);
		}
	}
}