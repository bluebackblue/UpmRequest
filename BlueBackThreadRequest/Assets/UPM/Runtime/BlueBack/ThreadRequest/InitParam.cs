

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
		where REQUESTITEM : struct
	{
		/** execute
		*/
		public Execute_Base<REQUESTITEM> execute;

		/** CreateDefault
		*/
		public static InitParam<REQUESTITEM> CreateDefault()
		{
			return new InitParam<REQUESTITEM>(){
				execute = null,
			};
		}
	}
}

