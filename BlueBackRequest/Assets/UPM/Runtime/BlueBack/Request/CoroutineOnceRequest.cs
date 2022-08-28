

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。コルーチン。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineOnceRequest
	*/
	public sealed class CoroutineOnceRequest<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** core
		*/
		private CoroutineOnceRequest_Core<ITEM> core;

		/** constructor
		*/
		public CoroutineOnceRequest(in CoroutineOnceRequest_InitParam<ITEM> a_initparam)
		{
			//core
			this.core = new CoroutineOnceRequest_Core<ITEM>(in a_initparam);
		}

		/** [System.IDisposable]Dispose
		*/
		public void Dispose()
		{
			//core
			if(this.core != null){
				this.core.Dispose();
				this.core = null;
			}
		}

		/** Start
		*/
		public bool Start(ITEM a_item)
		{
			//core
			return this.core.Start(a_item);
		}

		/** End
		*/
		public System.Collections.IEnumerator End()
		{
			yield return this.core.End();
		}

		/** TryEnd
		*/
		public bool TryEnd()
		{
			return this.core.TryEnd();
		}

		/** SetCancelValue
		*/
		public void SetCancelValue(long a_value)
		{
			this.core.SetCancelValue(a_value);
		}

		/** GetCancelValue
		*/
		public long GetCancelValue()
		{
			return this.core.GetCancelValue();
		}
	}
}

