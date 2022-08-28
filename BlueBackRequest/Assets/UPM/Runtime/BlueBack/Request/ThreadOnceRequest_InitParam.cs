

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。スレッド。初期化パラメータ。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadOnceRequest_InitParam
	*/
	public struct ThreadOnceRequest_InitParam<ITEM>
		where ITEM : class
	{
		/** context
		*/
		public System.Threading.SynchronizationContext context;

		/** execute
		*/
		public ThreadOnceRequest_Execute_Base<ITEM> execute;

		/** coremask
		*/
		public System.UInt64 coremask;

		/** threadpriority
		*/
		public ThreadPriority threadpriority;

		/** CreateDefault
		*/
		public static ThreadOnceRequest_InitParam<ITEM> CreateDefault()
		{
			return new ThreadOnceRequest_InitParam<ITEM>(){
				context = System.Threading.SynchronizationContext.Current,
				execute = null,
				coremask = 0,
				threadpriority = ThreadPriority.Middle,
			};
		}
	}
}

