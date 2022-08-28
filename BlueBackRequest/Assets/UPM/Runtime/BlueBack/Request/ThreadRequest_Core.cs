

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。スレッド。
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

		/** cancel
		*/
		private Cancel cancel;

		/** execute
		*/
		private ThreadRequest_Execute_Base<ITEM> execute;

		/** context
		*/
		private System.Threading.SynchronizationContext context;

		/** wakeup_lockobject
		*/
		private object wakeup_lockobject;

		/** manualresetevent
		*/
		private System.Threading.ManualResetEvent manualresetevent;

		/** thread
		*/
		private System.Threading.Thread thread;
		private System.UInt64 thread_coremask;
		private ThreadPriority thread_priority;
		private long thread_end;

		/** constructor
		*/
		public ThreadRequest_Core(in ThreadRequest_InitParam<ITEM> a_initparam,ThreadRequest_List<ITEM> a_list)
		{
			//[cache]list
			this.list = a_list;

			//cancel
			this.cancel = new Cancel();

			//execute
			this.execute = a_initparam.execute;

			//context
			this.context = a_initparam.context;

			//wakeup_lockobject
			this.wakeup_lockobject = new object();

			//manualresetevent
			this.manualresetevent = new System.Threading.ManualResetEvent(false);

			//thread
			this.thread_coremask = a_initparam.coremask;
			this.thread_priority = a_initparam.threadpriority;
			System.Threading.Interlocked.Exchange(ref this.thread_end,0);
			this.thread = new System.Threading.Thread(this.Inner_ThreadMain);
			this.thread.Start();
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			this.cancel.Set(1);

			//thread_end
			System.Threading.Interlocked.Exchange(ref this.thread_end,1);

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
			this.cancel.Set(0);

			//manualresetevent
			if(this.manualresetevent != null){
				this.manualresetevent.Dispose();
				this.manualresetevent = null;
			}
		}

		/** SetCancelValue
		*/
		public void SetCancelValue(long a_value)
		{
			//cancel
			this.cancel.Set(a_value);
		}

		/** GetCancelValue
		*/
		public long GetCancelValue()
		{
			return this.cancel.Get();
		}

		/** スレッド。復帰。

			return == false : 失敗。

		*/
		public bool Wakeup()
		{
			lock(this.wakeup_lockobject){
				#pragma warning disable 0168
				try{
					if(this.manualresetevent.Set() == true){
						return true;
					}else{
						#if(DEF_BLUEBACK_DEBUG_ASSERT)
						DebugTool.Assert(false,"error : Set");
						#endif
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_DEBUG_ASSERT)
					DebugTool.Assert(false,t_exception);
					#endif
				}
				#pragma warning restore
			}

			return false;
		}

		/** [System.Threading.SendOrPostCallback]Inner_AfterContextMain
		*/
		private void Inner_AfterContextMain(object a_userdata)
		{
			if(this.execute != null){
				try{
					this.execute.AfterContextMain((ITEM)a_userdata);
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_DEBUG_ASSERT)
					DebugTool.Assert(false,t_exception);
					#endif
				}
			}
		}

		/** Inner_ThreadMain
		*/
		private void Inner_ThreadMain()
		{
			#if((UNITY_STANDALONE_WIN)||(UNITY_EDITOR_WIN))
			{
				//GetCurrentThreadId
				#if(DEF_BLUEBACK_DEBUG_LOG)
				DebugTool.Log(string.Format("id = {0} mask = {1} priority = {2}",WinKernel32.GetCurrentThreadId(),this.thread_coremask,this.thread_priority));
				#endif

				uint t_handle = WinKernel32.GetCurrentThread();

				if(this.thread_coremask != 0){
					WinKernel32.SetThreadAffinityMask(t_handle,(uint)this.thread_coremask);
				}

				switch(this.thread_priority){
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
						#if(DEF_BLUEBACK_DEBUG_ASSERT)
						DebugTool.Assert(false,"error : WaitOne");
						#endif

						//スレッド終了。
						break;
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_DEBUG_ASSERT)
					DebugTool.Assert(false,t_exception);
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
						#if(DEF_BLUEBACK_DEBUG_ASSERT)
						DebugTool.Assert(false,t_exception);
						#endif

						//スレッド終了。
						break;
					}
				}else{
					//スレッド終了。
					break;
				}

				if(t_item != null){
					//execute
					if(this.execute != null){
						try{
							this.execute.ThreadMain(t_item,this.cancel);
						}catch(System.Exception t_exception){
							#if(DEF_BLUEBACK_DEBUG_ASSERT)
							DebugTool.Assert(false,t_exception);
							#endif

							//スレッド終了。
							break;
						}
					}

					//MemoryBarrier
					System.Threading.Thread.MemoryBarrier();

					//context
					if(this.context != null){
						try{
							this.context.Post(this.Inner_AfterContextMain,t_item);
						}catch(System.Exception t_exception){
							#if(DEF_BLUEBACK_DEBUG_ASSERT)
							DebugTool.Assert(false,t_exception);
							#endif

							//スレッド終了。
							break;
						}
					}
				}

				//Wakeupを禁止してからチェックを行う。
				try{
					lock(this.wakeup_lockobject){
						if(this.list.GetCount() == 0){
							this.manualresetevent.Reset();
						}
					}
				}catch(System.Exception t_exception){
					#if(DEF_BLUEBACK_DEBUG_ASSERT)
					DebugTool.Assert(false,t_exception);
					#endif

					//スレッド終了。
					break;
				}
			}while(System.Threading.Interlocked.Read(ref this.thread_end) == 0);
			#pragma warning restore
		}
	}
}

