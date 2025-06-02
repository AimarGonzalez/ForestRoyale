namespace ForestLib.ExtensionMethods
{
	public static class MathExtensionMethods
	{
		public static float Divide_NoNaN(this float value, float divisor)
		{
			if (value == 0 && divisor == 0)
			{
				return 0;
			}

			return value / divisor;
		}
	}
}
