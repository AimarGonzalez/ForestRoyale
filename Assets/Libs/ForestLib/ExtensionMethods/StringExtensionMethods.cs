namespace ForestLib.ExtensionMethods
{
	public static class StringExtensionMethods
	{
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static bool IsNotEmpty(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}
	}
}