

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadRequest_Core
	*/
	public sealed class ThreadRequest_Core<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** [cache]list
		*/
		private ThreadRequest_List<ITEM> list;

		/** execute
		*/
		private ThreadRequest_Execute_Base<ITEM> execute;

		/** context
		*/
		private System.Threading.SynchronizationContext context;

		/** wakeup_lockobject
		*/
		private object wakeup_lockobject;

		/** cancel
		*/
		private long cancel;

		/** manualresetevent
		*/
		private System.Threading.ManualResetEvent manualresetevent;

		/** thread
		*/
		private System.Threading.Thread thread;

		/** coremask
		*/
		private System.UInt64 coremask;

		/** threadpriority
		*/
		private ThreadPriority threadpriority;

		/** constructor
		*/
		public ThreadRequest_Core(in ThreadRequest_InitParam<ITEM> a_initparam)
		{
			//list
			this.list = null;

			//execute
			this.execute = null;

			//context
			this.context = null;

			//wakeup_lockobject
			this.wakeup_lockobject = new object();

			//cancel
			this.cancel = 0;

			//manualresetevent
			this.manualresetevent = new System.Threading.ManualResetEvent(false);

			//thread
			this.thread = null;

			//coremask
			this.coremask = a_initparam.coremask;

			//threadpriority
			this.threadpriority = a_initparam.threadpriority;
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			System.Threading.Interlocked.Exchange(ref this.cancel,1);

			//Wakeup
			this.Wakeup();

			//thread
			this.thread.Join();
			this.thread.Abort();
			this.thread = null;

			//[cache]list
			this.list = null;

			//execute
			this.execute = null;

			//this.context
			this.context = null;

			//wakeup_lockobject
			this.wakeup_lockobject = null;

			//cancel
			this.cancel = 0;

			//manualresetevent
			if(this.manualresetevent != null){
				this.manualresetevent.Dispose();
				this.manualresetevent = null;
			}
		}

		/** スレッド。開始。
		*/
		public void Start(ThreadRequest_List<ITEM> a_list,in ThreadRequest_InitParam<ITEM> a_initparam)
		{
			//list
			this.list = a_list;

			//execute
			this.execute = a_initparam.execute;

			//context
			this.context = a_initparam.context;

			//Start
			this.thread = new System.Threading.Thread(this.Inner_ThreadMain);
			this.thread.Start();
		}

		/** スレッド。復帰。

			return == false : 失敗。

		*/
		public bool Wakeup()
		{
			lock(this.wakeup_lockobject){
				try{
					if(this.manualresetevent.Set() == true){
						return true;
					}else{
						#if(DEF_BLUEBACK_REQUEST_ASSERT)
						DebugTool.Assert(false,"error : Set");
						#endif
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_REQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif
				}
			}

			return false;
		}

		/** [System.Threading.SendOrPostCallback]Inner_AfterContextExecute
		*/
		private void Inner_AfterContextExecute(object a_userdata)
		{
			this.execute.AfterContextExecute((ITEM)a_userdata);
		}

		/** Inner_ThreadMain
		*/
		private void Inner_ThreadMain()
		{
			#if((UNITY_STANDALONE_WIN)||(UNITY_EDITOR_WIN))
			{
				//GetCurrentThreadId
				#if(DEF_BLUEBACK_REQUEST_LOG)
				DebugTool.Log(string.Format("id = {0} mask = {1} priority = {2}",WinKernel32.GetCurrentThreadId(),this.coremask,this.threadpriority));
				#endif

				uint t_handle = WinKernel32.GetCurrentThread();

				if(this.coremask != 0){
					WinKernel32.SetThreadAffinityMask(t_handle,(uint)this.coremask);
				}

				switch(this.threadpriority){
				case ThreadPriority.Low:
					{
						WinKernel32.SetThreadPriority(t_handle,WinKernel32.THREAD_PRIORITY_BELOW_NORMAL);
					}break;
				case ThreadPriority.High:
					{
						WinKernel32.SetThreadPriority(t_handle,WinKernel32.THREAD_PRIORITY_ABOVE_NORMAL);
					}break;
				case ThreadPriority.Middle:
				default:
					{
						WinKernel32.SetThreadPriority(t_handle,WinKernel32.THREAD_PRIORITY_NORMAL);
					}break;
				}
			}
			#endif

			#pragma warning disable 0168
			do{
				try{
					if(this.manualresetevent.WaitOne() == true){
					}else{
						#if(DEF_BLUEBACK_REQUEST_ASSERT)
						DebugTool.Assert(false,"error : WaitOne");
						#endif

						//スレッド終了。
						break;
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_REQUEST_ASSERT)
					DebugTool.Assert(false,t_exception.Message);
					#endif

					//スレッド終了。
					break;
				}

				ITEM t_item;

				//Dequeue
				if(this.list != null){
					try{
						t_item = this.list.Dequeue();
					}catch(System.Exception t_exception){
						#if(DEF_BLUEBACK_REQUEST_ASSERT)
						DebugTool.Assert(false,t_exception.Message);
						#endif

						//スレッド終了。
						break;
					}
				}else{
					//スレッド終了。
					break;
				}

				//ThreadExecute
				if(t_item != null){
					if(this.execute != null){
						try{
							this.execute.ThreadExecute(t_item,ref this.cancel);
						}catch(System.Exception t_exception){
							#if(DEF_BLUEBACK_REQUEST_ASSERT)
							DebugTool.Assert(false,t_exception.Message);
							#endif

							//スレッド終了。
							break;
						}
					}
				}

				//応答待ちしない。
				if(t_item != null){
					try{
						if(this.context != null){
							this.context.Post(this.Inner_AfterContextExecute,t_item);
						}else{
							System.Threading.Thread.MemoryBarrier();
						}
					}catch(System.Exception t_exception){
						#if(DEF_BLUEBACK_REQUEST_ASSERT)
						DebugTool.Assert(false,t_exception.Message);
						#endif

						//スレッド終了。
						break;
					}
				}

				//Wakeupを禁止してからリセットチェックを行う。
				try{
					lock(this.wakeup_lockobject){
						if(this.list.GetCount() == 0){
							this.manualresetevent.Reset();
						}
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_REQUEST_ASSERT)
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

