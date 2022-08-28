

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。スレッド。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadOnceRequest
	*/
	public sealed class ThreadOnceRequest<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** core
		*/
		private ThreadOnceRequest_Core<ITEM> core;

		/** constructor
		*/
		public ThreadOnceRequest(in ThreadOnceRequest_InitParam<ITEM> a_initparam)
		{
			//core
			this.core = new ThreadOnceRequest_Core<ITEM>(in a_initparam);
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
			return this.core.Start(a_item);
		}

		/** End
		*/
		public void End()
		{
			this.core.End();
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

