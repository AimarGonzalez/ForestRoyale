using System.IO;

namespace ForestLib.Utils
{
	public static class AssetUtils
	{
		public static string CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			return path;
		}
	}
}
