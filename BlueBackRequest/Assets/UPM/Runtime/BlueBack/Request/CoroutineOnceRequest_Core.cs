

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。コルーチン。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineOnceRequest_Core
	*/
	public sealed class CoroutineOnceRequest_Core<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** ITEM
		*/
		public ITEM item;

		/** cancel
		*/
		private Cancel cancel;

		/** execute
		*/
		public CoroutineOnceRequest_Execute_Base<ITEM> execute;

		/** coroutine
		*/
		public UnityEngine.Coroutine coroutine;
		public UnityEngine.MonoBehaviour coroutine_monobehaviour;
		public bool coroutine_busy;

		/** constructor
		*/
		public CoroutineOnceRequest_Core(in CoroutineOnceRequest_InitParam<ITEM> a_initparam)
		{
			//item
			this.item = null;

			//cancel
			this.cancel = new Cancel();

			//execute
			this.execute = a_initparam.execute;

			//coroutine
			this.coroutine = null;
			this.coroutine_monobehaviour = a_initparam.monobehaviour;
			this.coroutine_busy = false;
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			this.cancel.Set(1);

			//coroutine
			{
				if(this.coroutine != null){
					this.coroutine_monobehaviour.StopCoroutine(this.coroutine);
					this.coroutine = null;
				}
				this.coroutine_monobehaviour = null;
				this.coroutine_busy = false;
			}

			//execute
			this.execute = null;

			//item
			this.item = null;
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

		/** Start
		*/
		public bool Start(ITEM a_item)
		{
			if(a_item == null){
				return false;
			}

			if(this.coroutine_busy == true){
				return false;
			}

			//item
			this.item = a_item;

			//StartCoroutine
			this.coroutine_busy = true;
			this.coroutine = this.coroutine_monobehaviour.StartCoroutine(this.Inner_CoroutineMain());

			return true;
		}

		/** End
		*/
		public System.Collections.IEnumerator End()
		{
			//coroutine
			{
				do{
					yield return null;
				}while(this.coroutine_busy == true);
				this.coroutine = null;
			}

			//item
			this.item = null;
		}

		/** TryEnd
		*/
		public bool TryEnd()
		{
			if(this.coroutine_busy == false){

				//coroutine
				this.coroutine = null;

				//item
				this.item = null;

				return true;
			}else{
				return false;
			}
		}

		/** Inner_CoroutineMain
		*/
		private System.Collections.IEnumerator Inner_CoroutineMain()
		{
			do{
				if(this.execute != null){
					yield return this.execute.CoroutineMain(this.item,this.cancel);
				}

				//コルーチン終了。
				break;
			}while(false);

			//coroutine_busy
			this.coroutine_busy = false;
			yield break;
		}
	}
}

