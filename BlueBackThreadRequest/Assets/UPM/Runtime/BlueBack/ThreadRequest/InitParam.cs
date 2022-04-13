

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief ThreadRequest。初期化パラメータ。
*/


/** BlueBack.ThreadRequest
*/
namespace BlueBack.ThreadRequest
{
	/** InitParam
	*/
	public struct InitParam<REQUESTITEM>
		where REQUESTITEM : class
	{
		/** context
		*/
		public System.Threading.SynchronizationContext context;

		/** execute
		*/
		public Execute_Base<REQUESTITEM> execute;

		/** CreateDefault
		*/
		public static InitParam<REQUESTITEM> CreateDefault()
		{
			return new InitParam<REQUESTITEM>(){
				context = System.Threading.SynchronizationContext.Current,
				execute = null,
			};
		}
	}
}

