

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineRequest_Core
	*/
	public sealed class CoroutineRequest_Core<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** [cache]list
		*/
		public CoroutineRequest_List<ITEM> list;

		/** execute
		*/
		public CoroutineRequest_Execute_Base<ITEM> execute;

		/** monobehaviour
		*/
		public UnityEngine.MonoBehaviour monobehaviour;

		/** cancel
		*/
		public CoroutineRequest_Cancel cancel;

		/** constructor
		*/
		public CoroutineRequest_Core()
		{
			//list
			this.list = null;

			//execute
			this.execute = null;

			//monobehaviour
			this.monobehaviour = null;

			//cancel
			this.cancel = new CoroutineRequest_Cancel(0);
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			this.cancel.Set(1);

			//monobehaviour
			this.monobehaviour = null;

			//list
			this.list = null;

			//execute
			this.execute = null;
		}

		/** コルーチン。開始。
		*/
		public void Start(CoroutineRequest_List<ITEM> a_list,in CoroutineRequest_InitParam<ITEM> a_initparam)
		{
			//list
			this.list = a_list;

			//execute
			this.execute = a_initparam.execute;

			//monobehaviour
			this.monobehaviour = a_initparam.monobehaviour;

			//StartCoroutine
			this.monobehaviour.StartCoroutine(this.Inner_CoroutineMain());
		}

		/** Inner_CoroutineMain
		*/
		private System.Collections.IEnumerator Inner_CoroutineMain()
		{
			#pragma warning disable 0168
			do{
				ITEM t_item;

				//Dequeue
				if(this.list != null){
					try{
						t_item = this.list.Dequeue();
					}catch(System.Exception t_exception){
						#if(DEF_BLUEBACK_REQUEST_ASSERT)
						DebugTool.Assert(false,t_exception.Message);
						#endif

						//コルーチン終了。
						break;
					}
				}else{
					//コルーチン終了。
					break;
				}

				//CoroutineExecute
				if(t_item != null){
					if(this.execute != null){
						yield return this.execute.CoroutineExecute(t_item,this.cancel);
					}
				}else{
					yield return null;
				}

			}while(this.cancel.Get() == 0);
			#pragma warning restore

			yield break;
		}
	}
}

