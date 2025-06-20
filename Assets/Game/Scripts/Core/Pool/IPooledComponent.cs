namespace ForestRoyale.Core.Pool
{
		public interface IPooledComponent
	{
		void OnGetFromPool();
		void OnReturnToPool();
		void OnDestroyFromPool();
	}
}