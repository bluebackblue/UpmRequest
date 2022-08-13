

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineCall_Execute_Base
	*/
	public interface CoroutineCall_Execute_Base<PARAM>
		where PARAM : class
	{
		/** [BlueBack.Request.CoroutineCall_Execute_Base<PARAM>]コルーチンから呼び出される。
		*/
		System.Collections.IEnumerator CoroutineMain(PARAM a_param);

		/** [BlueBack.Request.CoroutineCall_Execute_Base<PARAM>]コルーチンキャンセル。
		*/
		void CoroutineCancel(PARAM a_param);
	}
}

