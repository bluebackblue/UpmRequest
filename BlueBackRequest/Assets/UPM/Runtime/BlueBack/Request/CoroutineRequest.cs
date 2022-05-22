

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
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
			//core
			this.core = new CoroutineRequest_Core<ITEM>();

			//list
			this.list = new CoroutineRequest_List<ITEM>();

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

