namespace ForestLib.Utils.Pool
{
		public interface IPooledComponent
	{
		void OnGetFromPool();
		void OnReturnToPool();
		void OnDestroyFromPool();
	}
}