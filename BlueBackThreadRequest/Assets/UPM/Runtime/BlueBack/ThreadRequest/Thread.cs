

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
		where REQUESTITEM : class
	{
		/** [cache]requestlist
		*/
		public RequestList<REQUESTITEM> requestlist;

		/** execute
		*/
		public Execute_Base<REQUESTITEM> execute;

		/** context
		*/
		public System.Threading.SynchronizationContext context;

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
		public Thread()
		{
			//requestlist
			this.requestlist = null;

			//execute
			this.execute = null;

			//context
			this.context = null;

			//lockobject
			this.lockobject = new object();

			//cancel
			this.cancel = 0;

			//manualresetevent
			this.manualresetevent = new System.Threading.ManualResetEvent(false);

			//raw
			this.raw = new System.Threading.Thread(this.ThreadMain);
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			System.Threading.Interlocked.Exchange(ref this.cancel,1);

			//Wakeup
			this.Wakeup();

			//raw
			this.raw.Join();
			this.raw.Abort();
			this.raw = null;

			//lockobject
			this.lockobject = null;

			//requestlist
			this.requestlist = null;

			//execute
			this.execute = null;
		}

		/** スレッド。開始。
		*/
		public void Start(RequestList<REQUESTITEM> a_requestlist,Execute_Base<REQUESTITEM> a_execute,System.Threading.SynchronizationContext a_context)
		{
			//requestlist
			this.requestlist = a_requestlist;

			//execute
			this.execute = a_execute;

			//context
			this.context = a_context;

			//Start
			this.raw.Start();
		}

		/** スレッド。復帰。

			return == false : 失敗。

		*/
		public bool Wakeup()
		{
			#pragma warning disable 0168
			lock(this.lockobject){
				try{
					if(this.manualresetevent.Set() == true){
						return true;
					}else{
						#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
						DebugTool.Assert(false,"error : Set");
						#endif
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif
				}
			}
			#pragma warning restore

			return false;
		}

		/** [System.Threading.SendOrPostCallback]Inner_AfterContextExecute
		*/
		private void Inner_AfterContextExecute(object a_userdata)
		{
			this.execute.AfterContextExecute((REQUESTITEM)a_userdata);
		}

		/** ThreadMain
		*/
		private void ThreadMain()
		{
			#pragma warning disable 0168
			do{
				try{
					if(this.manualresetevent.WaitOne() == true){
					}else{
						#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
						DebugTool.Assert(false,"error : WaitOne");
						#endif
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif

					//スレッド終了。
					break;
				}

				//ThreadExecute
				try{
					if(this.requestlist.Dequeue(out REQUESTITEM t_requestitem) == true){
						if(this.execute != null){
							this.execute.ThreadExecute(t_requestitem,ref this.cancel);
						}

						System.Threading.Thread.MemoryBarrier();
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif
				}

				//AfterContextExecute
				try{
					if(this.requestlist.Dequeue(out REQUESTITEM t_requestitem) == true){
						if(this.context != null){
							this.context.Post(this.Inner_AfterContextExecute,t_requestitem);
						}
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif
				}

				try{
					lock(this.lockobject){
						if(this.requestlist.list.Count == 0){
							this.manualresetevent.Reset();
						}
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_THREADREQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif

					//スレッド終了。
					break;
				}
			}while(System.Threading.Interlocked.Read(ref this.cancel) == 0);
			#pragma warning restore
		}
	}
}

