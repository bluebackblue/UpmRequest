

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineCall
	*/
	public sealed class CoroutineCall<PARAM> : System.IDisposable
		where PARAM : class
	{
		/** 処理中。
		*/
		public bool busy;

		/** execute
		*/
		public CoroutineCall_Execute_Base<PARAM> execute;

		/** param
		*/
		public PARAM param;

		/** constructor
		*/
		public CoroutineCall()
		{
			//busy
			this.busy = false;

			//execute
			this.execute = null;

			//param
			this.param = null;
		}

		/** [System.IDisposable]Dispose
		*/
		public void Dispose()
		{
			if(this.busy == true){
				if(this.execute != null){
					this.execute.CoroutineCancel(this.param);
				}
			}
		}

		/** Start
		*/
		public void Start(CoroutineCall_Execute_Base<PARAM> a_execute,PARAM a_param,UnityEngine.MonoBehaviour a_monobehaviour)
		{
			//busy
			this.busy = true;

			//execute
			this.execute = a_execute;

			//param
			this.param = a_param;

			//StartCoroutine
			a_monobehaviour.StartCoroutine(this.Inner_Main(a_param));
		}

		/** TryEnd
		*/
		public bool TryEnd()
		{
			if(this.busy == true){
				if(this.execute != null){
					this.execute.CoroutineCancel(this.param);
				}
			}

			return (this.busy == false);
		}

		/** Inner_Main
		*/
		private System.Collections.IEnumerator Inner_Main(System.Object a_object)
		{
			if(this.execute != null){
				yield return this.execute.CoroutineMain((PARAM)a_object);
			}else{
				#if(DEF_BLUEBACK_DEBUG_ASSERT)
				DebugTool.Assert(false,"execute == null");
				#endif
			}
			this.busy = false;
			yield break;
		}
	}
}

