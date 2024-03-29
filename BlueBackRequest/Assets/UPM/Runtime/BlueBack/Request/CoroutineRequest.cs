

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。コルーチン。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineRequest
	*/
	public sealed class CoroutineRequest<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** list
		*/
		private CoroutineRequest_List<ITEM> list;

		/** core
		*/
		private CoroutineRequest_Core<ITEM> core;

		/** constructor
		*/
		public CoroutineRequest(in CoroutineRequest_InitParam<ITEM> a_initparam)
		{
			//list
			this.list = new CoroutineRequest_List<ITEM>();

			//core
			this.core = new CoroutineRequest_Core<ITEM>(in a_initparam,this.list);
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//core
			if(this.core != null){
				this.core.Dispose();
				this.core = null;
			}

			//list
			if(this.list != null){
				this.list.Dispose();
				this.list = null;
			}
		}

		/** 発行。
		*/
		public void Request(ITEM a_item)
		{
			this.list.Enqueue(a_item);
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

