using ForestLib.ExtensionMethods;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ForestLib.Utils
{
	public static class AssetUtils
	{
		/// <summary>
		/// Creates a directory if it doesn't exist.
		/// </summary>
		public static string CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			return path;
		}

		/// <summary>
		/// Creates or replaces an asset at the given path.
		/// </summary>
		public static T CreateOrReplaceAsset<T>(T asset, string path) where T : Object
		{
			if (asset.IsAsset())
			{
				return SaveAssetToPath(asset, path);
			}
			
			T existingAsset = AssetDatabase.LoadAssetAtPath<T>(path);
			if (existingAsset != null)
			{
				return ReplaceAssetAtPath(asset, path, existingAsset);
			}
				
			return CreateAsset(asset, path);
		}

		private static T ReplaceAssetAtPath<T>(T asset, string path, T existingAsset) where T : Object
		{
			// Replace existing asset
			var tempPath = AssetDatabase.GenerateUniqueAssetPath(path);
			AssetDatabase.CreateAsset(asset, tempPath);
			FileUtil.ReplaceFile(tempPath, path);
			AssetDatabase.DeleteAsset(tempPath);
			return existingAsset;
		}

		private static T CreateAsset<T>(T asset, string path) where T : Object
		{
			// Create asset
			string directory = Path.GetDirectoryName(path);
			CreateDirectory(directory);

			AssetDatabase.CreateAsset(asset, path);
			return asset;
		}

		private static T SaveAssetToPath<T>(T asset, string newPath) where T : Object
		{
			string currentPath = AssetDatabase.GetAssetPath(asset);
			if (string.IsNullOrEmpty(currentPath))
			{
				Debug.LogError($"Failed to get current path for asset {asset.name}");
				return null;
			}

			T finalAsset = null;
			if (newPath == currentPath)
			{
				// Save at current location
				EditorUtility.SetDirty(asset);
				AssetDatabase.SaveAssetIfDirty(asset);
				finalAsset = asset;
			}
			else
			{
				// Save at new location
				string directory = Path.GetDirectoryName(newPath);
				CreateDirectory(directory);

				if (AssetDatabase.CopyAsset(currentPath, newPath))
				{
					finalAsset =  AssetDatabase.LoadAssetAtPath<T>(newPath);
				}
				else
				{
					Debug.LogError($"Failed to copy asset from {currentPath} to {newPath}");
				}
			}
			
			return finalAsset;
		}
	}
}
