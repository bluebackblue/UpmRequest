

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief ThreadRequest。
*/


/** BlueBack.ThreadRequest
*/
namespace BlueBack.ThreadRequest
{
	/** Execute_Base
	*/
	public interface Execute_Base<REQUESTITEM>
		where REQUESTITEM : class
	{
		/** [BlueBack.ThreadRequest.Execute_Base<REQUESTITEM>]ThreadExecute

			System.Threading.Interlocked.Read(ref a_cancel) == 1 : キャンセルリクエストあり。

		*/
		void ThreadExecute(REQUESTITEM a_requestitem,ref long a_cancel);

		/** [BlueBack.ThreadRequest.Execute_Base<REQUESTITEM>]AfterContextExecute
		*/
		void AfterContextExecute(REQUESTITEM a_requestitem);
	}
}

