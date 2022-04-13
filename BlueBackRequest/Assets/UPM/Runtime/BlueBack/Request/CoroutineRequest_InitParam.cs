

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。初期化パラメータ。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineRequest_InitParam
	*/
	public struct CoroutineRequest_InitParam<ITEM>
		where ITEM : class
	{
		/** monobehaviour
		*/
		public UnityEngine.MonoBehaviour monobehaviour;

		/** execute
		*/
		public CoroutineRequest_Execute_Base<ITEM> execute;

		/** CreateDefault
		*/
		public static CoroutineRequest_InitParam<ITEM> CreateDefault()
		{
			return new CoroutineRequest_InitParam<ITEM>(){
				monobehaviour = null,
				execute = null,
			};
		}
	}
}

