

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadCall_Execute_Base
	*/
	public interface ThreadCall_Execute_Base<PARAM>
		where PARAM : class
	{
		/** [BlueBack.Request.ThreadCall_Execute_Base<PARAM>]スレッドから呼び出される。
		*/
		void ThreadMain(PARAM a_param);

		/** [BlueBack.Request.ThreadCall_Execute_Base<PARAM>]スレッドキャンセル。
		*/
		void ThreadCancel(PARAM a_param);
	}
}

