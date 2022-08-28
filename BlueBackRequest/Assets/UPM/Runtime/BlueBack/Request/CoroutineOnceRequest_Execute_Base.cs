

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。コルーチン。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineOnceRequest_Execute_Base
	*/
	public interface CoroutineOnceRequest_Execute_Base<ITEM>
		where ITEM : class
	{
		/** [BlueBack.Request.CoroutineOnceRequest_Execute_Base<ITEM>]コルーチンから呼び出される。

			a_cancel.Get() != 0 : キャンセルリクエストあり。

		*/
		System.Collections.IEnumerator CoroutineMain(ITEM a_item,BlueBack.Request.Cancel a_cancel);
	}
}

