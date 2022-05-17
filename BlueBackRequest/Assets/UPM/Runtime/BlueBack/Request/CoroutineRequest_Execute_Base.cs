

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineRequest_Execute_Base
	*/
	public interface CoroutineRequest_Execute_Base<ITEM>
		where ITEM : class
	{
		/** [BlueBack.Request.CoroutineRequest_Execute_Base<ITEM>]CoroutineExecute

			a_cancel.value == 1 : キャンセルリクエストあり。

		*/
		System.Collections.IEnumerator CoroutineExecute(ITEM a_item,CoroutineRequest_Cancel a_cancel);
	}
}

