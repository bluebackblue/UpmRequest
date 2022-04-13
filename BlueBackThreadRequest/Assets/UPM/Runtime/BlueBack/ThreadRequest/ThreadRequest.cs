

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
		where REQUESTITEM : class
	{
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
			//thread
			this.thread = new Thread<REQUESTITEM>();

			//requestlist
			this.requestlist = new RequestList<REQUESTITEM>(this.thread);

			//Start
			this.thread.Start(this.requestlist,a_initparam.execute,a_initparam.context);
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//thread
			this.thread.Dispose();
			this.thread = null;

			//requestlist
			this.requestlist.Dispose();
			this.requestlist = null;
		}

		/** 発行。
		*/
		public void Request(REQUESTITEM a_requestitem)
		{
			this.requestlist.Enqueue(a_requestitem);
		}
	}
}

