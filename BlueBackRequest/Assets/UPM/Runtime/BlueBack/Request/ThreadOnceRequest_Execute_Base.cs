

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。スレッド。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadOnceRequest_Execute_Base
	*/
	public interface ThreadOnceRequest_Execute_Base<ITEM>
		where ITEM : class
	{
		/** [BlueBack.Request.ThreadOnceRequest_Execute_Base<ITEM>]スレッドから呼び出される。

			a_cancel.Get() != 0 : キャンセルリクエストあり。

		*/
		void ThreadMain(ITEM a_item,BlueBack.Request.Cancel a_cancel);

		/** [BlueBack.Request.ThreadOnceRequest_Execute_Base<ITEM>]コンテキストから呼び出される。
		*/
		void AfterContextMain(ITEM a_item);
	}
}

