using UnityEditor;

namespace ForestLib.ExtensionMethods
{
	public static class AssetExtensionMethods
	{
		public static bool IsAsset(this UnityEngine.Object obj)
		{
			return AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out _, out _);
		}
	}
}
