

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
		where REQUESTITEM : struct
	{
		/** [BlueBack.ThreadRequest.Execute_Base<REQUESTITEM>]Load

			System.Threading.Interlocked.Read(ref a_cancel) == 1 : キャンセルリクエストあり。

		*/
		void Load(in REQUESTITEM a_requestitem,ref long a_cancel);
	}
}

