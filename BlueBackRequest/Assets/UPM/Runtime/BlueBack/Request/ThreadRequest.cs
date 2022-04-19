

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
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
		public ThreadRequest_List<ITEM> list;

		/** core
		*/
		public ThreadRequest_Core<ITEM> core;

		/** constructor
		*/
		public ThreadRequest(in ThreadRequest_InitParam<ITEM> a_initparam)
		{
			//core
			this.core = new ThreadRequest_Core<ITEM>(in a_initparam);

			//list
			this.list = new ThreadRequest_List<ITEM>(this.core);

			//Start
			this.core.Start(this.list,in a_initparam);
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
	}
}

