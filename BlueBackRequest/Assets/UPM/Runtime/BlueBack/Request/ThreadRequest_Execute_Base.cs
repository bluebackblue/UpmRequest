

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadRequest_Execute_Base
	*/
	public interface ThreadRequest_Execute_Base<ITEM>
		where ITEM : class
	{
		/** [BlueBack.Request.ThreadRequest_Execute_Base<ITEM>]ThreadExecute

			System.Threading.Interlocked.Read(ref a_cancel) == 1 : キャンセルリクエストあり。

		*/
		void ThreadExecute(ITEM a_item,ref long a_cancel);

		/** [BlueBack.Request.ThreadRequest_Execute_Base<ITEM>]AfterContextExecute
		*/
		void AfterContextExecute(ITEM a_item);
	}
}

