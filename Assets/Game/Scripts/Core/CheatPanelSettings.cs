using System;
using UnityEngine;

namespace ForestRoyale.Core
{
	[Serializable]
	public struct CheatPanelSettings
	{
		[Serializable]
		private struct AspectRatio
		{
			public int Width;
			public int Height;

			public AspectRatio(int width, int height)
			{
				Width = width;
				Height = height;
			}

			public float Ratio => (float)Width / Height;
		}

		[SerializeField]
		private AspectRatio _aspectRatio;

		public float Ratio => _aspectRatio.Ratio;
		public float LabelWidth;
		public float Width;
		public float Height;

		public CheatPanelSettings(float labelWidth, float width, float height)
		{
			_aspectRatio = new AspectRatio(1, 1);
			LabelWidth = labelWidth;
			Width = width;
			Height = height;
		}
	}
}