

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief ThreadRequest。
*/


/** BlueBack.ThreadRequest
*/
namespace BlueBack.ThreadRequest
{
	/** ThreadRequest
	*/
	public sealed class ThreadRequest<REQUESTITEM>
		where REQUESTITEM : struct
	{
		/** execute
		*/
		public Execute_Base<REQUESTITEM> execute;

		/** requestlist
		*/
		public RequestList<REQUESTITEM> requestlist;

		/** thread
		*/
		public Thread<REQUESTITEM> thread;

		/** constructor
		*/
		public ThreadRequest(in InitParam<REQUESTITEM> a_initparam)
		{
			//execute
			this.execute = a_initparam.execute;

			//thread
			this.thread = new Thread<REQUESTITEM>(this.execute);

			//requestlist
			this.requestlist = new RequestList<REQUESTITEM>(this.thread);

			//Start
			this.thread.Start(this.requestlist);
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//execute
			this.execute = null;

			//thread
			this.thread.Dispose();
			this.thread = null;

			//requestlist
			this.requestlist.Dispose();
			this.requestlist = null;
		}

		/** 発行。
		*/
		public void Request(in REQUESTITEM a_requestitem)
		{
			this.requestlist.Enqueue(in a_requestitem);
		}
	}
}

