using System;
using UnityEngine;

namespace ForestLib.Utils
{
	public static class Awaitables
	{
		public static async Awaitable WaitUntil(Func<bool> condition)
		{
			while (!condition())
			{
				await Awaitable.NextFrameAsync();
			}
		}
	}
}