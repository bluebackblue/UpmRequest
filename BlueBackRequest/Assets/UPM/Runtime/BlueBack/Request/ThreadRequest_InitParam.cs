

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。初期化パラメータ。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadRequest_InitParam
	*/
	public struct ThreadRequest_InitParam<ITEM>
		where ITEM : class
	{
		/** context
		*/
		public System.Threading.SynchronizationContext context;

		/** execute
		*/
		public ThreadRequest_Execute_Base<ITEM> execute;

		/** CreateDefault
		*/
		public static ThreadRequest_InitParam<ITEM> CreateDefault()
		{
			return new ThreadRequest_InitParam<ITEM>(){
				context = System.Threading.SynchronizationContext.Current,
				execute = null,
			};
		}
	}
}

