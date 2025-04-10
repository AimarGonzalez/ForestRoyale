using System.Runtime.CompilerServices;
using UnityEngine;

namespace ForestLib.Utils
{
	public static class ColorConstants
	{
#pragma warning disable SA1025 // Code should not contain multiple white spaces in a row
		public static Color White => Color.white;
		public static Color Black => Color.black;
		public static Color Cyan => Color.cyan;
		public static Color Magenta => Color.magenta;
		
		public static Color VeryLightGrey =>    new(0.8f, 0.8f, 0.8f);
		public static Color LightGrey =>		new(0.5f, 0.5f, 0.5f);
		public static Color Grey =>				new(0.3f, 0.3f, 0.3f);
		public static Color DarkGrey =>			new(0.2f, 0.2f, 0.2f);
		
		public static Color LightYellow =>		new(1.0f, 1.0f, 0.6f);
		public static Color Yellow =>			new(1.0f, 1.0f, 0.0f);
		public static Color DarkYellow =>		new(0.5f, 0.4f, 0.0f);
		
		public static Color LightOrange =>		new(1.0f, 0.8f, 0.6f);
		public static Color Orange =>			new(1.0f, 0.5f, 0.0f);
		public static Color DarkOrange =>		new(0.8f, 0.3f, 0.0f);
		
		public static Color LightBrown => 		new(1.0f, 0.7f, 0.5f);
		public static Color Brown =>			new(0.6f, 0.3f, 0.1f);
		public static Color DarkBrown =>		new(0.2f, 0.1f, 0.0f);
		
		public static Color LightRed =>			new(1.0f, 0.4f, 0.4f);
		public static Color Red =>				new(1.0f, 0.0f, 0.0f);
		public static Color DarkRed => 			new(0.4f, 0.0f, 0.0f);
		
		public static Color LightPink =>		new(1.0f, 0.6f, 1.0f);
		public static Color Pink => 			new(1.0f, 0.0f, 1.0f);
		public static Color DarkPink =>			new(0.5f, 0.0f, 0.4f);
		
		public static Color LightPurple =>		new(0.7f, 0.5f, 1.0f);
		public static Color Purple =>			new(0.6f, 0.0f, 1.0f);
		public static Color DarkPurple =>		new(0.3f, 0.0f, 0.4f);
		
		public static Color VeryLightBlue =>	new(0.5f, 0.9f, 1.0f);
		public static Color LightBlue =>		new(0.2f, 0.6f, 1.0f);
		public static Color Blue =>				new(0.0f, 0.0f, 1.0f);
		public static Color DarkBlue =>			new(0.0f, 0.0f, 0.5f);
		
		public static Color VeryLightGreen =>	new(0.8f, 1.0f, 0.8f);
		public static Color LightGreen =>		new(0.6f, 1.0f, 0.5f);
		public static Color Green =>			new(0.0f, 1.0f, 0.0f);
		public static Color DarkGreen =>		new(0.0f, 0.5f, 0.0f);
		public static Color VeryDarkGreen =>	new(0.0f, 0.2f, 0.0f);
#pragma warning restore SA1025
	}
}