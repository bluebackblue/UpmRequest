

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。スレッド。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadRequest
	*/
	public sealed class ThreadRequest<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** list
		*/
		private ThreadRequest_List<ITEM> list;

		/** core
		*/
		private ThreadRequest_Core<ITEM> core;

		/** constructor
		*/
		public ThreadRequest(in ThreadRequest_InitParam<ITEM> a_initparam)
		{
			//list
			this.list = new ThreadRequest_List<ITEM>();

			//core
			this.core = new ThreadRequest_Core<ITEM>(in a_initparam,this.list);

			//SetCore
			this.list.SetCore(this.core);
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

