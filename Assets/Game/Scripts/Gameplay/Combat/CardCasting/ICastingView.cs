
using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	public interface ICastingView
	{
		public void SetActive(bool value);
		public void Cast(Transform charactersRoot);

		public void ReturnToPool();
	}
}