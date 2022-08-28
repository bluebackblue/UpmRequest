

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。コルーチン。
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
		private CoroutineRequest_List<ITEM> list;

		/** cancel
		*/
		private Cancel cancel;

		/** execute
		*/
		private CoroutineRequest_Execute_Base<ITEM> execute;

		/** coroutine_monobehaviour
		*/
		private UnityEngine.Coroutine coroutine;
		private UnityEngine.MonoBehaviour coroutine_monobehaviour;
		private long coroutine_end;

		/** constructor
		*/
		public CoroutineRequest_Core(in CoroutineRequest_InitParam<ITEM> a_initparam,CoroutineRequest_List<ITEM> a_list)
		{
			//[cache]list
			this.list = a_list;

			//cancel
			this.cancel = new Cancel();

			//execute
			this.execute = a_initparam.execute;

			//coroutine
			{
				this.coroutine = null;
				this.coroutine_monobehaviour = a_initparam.monobehaviour;
				this.coroutine_end = 0;
				this.coroutine_monobehaviour.StartCoroutine(this.Inner_CoroutineMain());
			}
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			this.cancel.Set(1);

			//coroutine_end
			this.coroutine_end = 1;

			//coroutine
			{
				if(this.coroutine != null){
					this.coroutine_monobehaviour.StopCoroutine(this.coroutine);
					this.coroutine = null;
				}
				this.coroutine_monobehaviour = null;
			}

			//[cache]list
			this.list = null;

			//execute
			this.execute = null;
		}

		/** SetCancelValue
		*/
		public void SetCancelValue(long a_value)
		{
			//cancel
			this.cancel.Set(a_value);
		}

		/** GetCancelValue
		*/
		public long GetCancelValue()
		{
			return this.cancel.Get();
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
						#if(DEF_BLUEBACK_DEBUG_ASSERT)
						DebugTool.Assert(false,t_exception);
						#endif

						//コルーチン終了。
						break;
					}
				}else{
					//コルーチン終了。
					break;
				}

				if(t_item != null){
					//execute
					if(this.execute != null){
						yield return this.execute.CoroutineMain(t_item,this.cancel);
					}
				}else{
					yield return null;
				}

			}while(this.coroutine_end == 0);
			#pragma warning restore

			yield break;
		}
	}
}

