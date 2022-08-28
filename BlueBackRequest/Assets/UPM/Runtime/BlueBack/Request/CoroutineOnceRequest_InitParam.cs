

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。コルーチン。初期化パラメータ。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineOnceRequest_InitParam
	*/
	public struct CoroutineOnceRequest_InitParam<ITEM>
		where ITEM : class
	{
		/** monobehaviour
		*/
		public UnityEngine.MonoBehaviour monobehaviour;

		/** execute
		*/
		public CoroutineOnceRequest_Execute_Base<ITEM> execute;

		/** CreateDefault
		*/
		public static CoroutineOnceRequest_InitParam<ITEM> CreateDefault()
		{
			return new CoroutineOnceRequest_InitParam<ITEM>(){
				monobehaviour = null,
				execute = null,
			};
		}
	}
}

