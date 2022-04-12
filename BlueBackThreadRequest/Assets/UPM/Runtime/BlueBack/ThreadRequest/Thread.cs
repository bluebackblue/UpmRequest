

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief ThreadRequest。
*/


/** BlueBack.ThreadRequest
*/
namespace BlueBack.ThreadRequest
{
	/** Thread
	*/
	public sealed class Thread<REQUESTITEM> : System.IDisposable
		where REQUESTITEM : struct
	{
		/** [cache]requestlist
		*/
		public RequestList<REQUESTITEM> requestlist;

		/** [cache]execute
		*/
		public Execute_Base<REQUESTITEM> execute;

		/** lockobject
		*/
		public object lockobject;

		/** cancel
		*/
		public long cancel;

		/** manualresetevent
		*/
		public System.Threading.ManualResetEvent manualresetevent;

		/** raw
		*/
		public System.Threading.Thread raw;

		/** Thread
		*/
		public Thread(Execute_Base<REQUESTITEM> a_execute)
		{
			//requestlist
			this.requestlist = null;

			//execute
			this.execute = a_execute;

			//lockobject
			this.lockobject = new object();

			//cancel
			this.cancel = 0;

			//manualresetevent
			this.manualresetevent = new System.Threading.ManualResetEvent(false);

			//raw
			this.raw = new System.Threading.Thread(this.ThreadMain);
			this.raw.Start();
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			System.Threading.Interlocked.Exchange(ref this.cancel,1);

			//manualresetevent
			if(this.manualresetevent.Set() == true){
			}else{
				#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
				DebugTool.Assert(false,"error");
				#endif
			}

			//raw
			this.raw.Join();
			this.raw.Abort();
			this.raw = null;

			//lockobject
			this.lockobject = null;
		}

		/** Start
		*/
		public void Start(RequestList<REQUESTITEM> a_requestlist)
		{
			//requestlist
			this.requestlist = a_requestlist;
		}

		/** Wakeup
		*/
		public void Wakeup()
		{
			lock(this.lockobject){
				if(this.manualresetevent.Set() == true){
				}else{
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,"error");
					#endif
				}
			}
		}

		/** ThreadMain
		*/
		private void ThreadMain()
		{
			do{
				if(this.manualresetevent.WaitOne() == true){
				}else{
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,"error");
					#endif
				}

				try{
					if(this.requestlist.Dequeue(out REQUESTITEM t_requestitem) == true){
						this.execute.Load(in t_requestitem,ref this.cancel);
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif
				}

				lock(this.lockobject){
					if(this.requestlist.list.Count == 0){
						this.manualresetevent.Reset();
					}
				}
			}while(System.Threading.Interlocked.Read(ref this.cancel) == 0);
		}
	}
}

