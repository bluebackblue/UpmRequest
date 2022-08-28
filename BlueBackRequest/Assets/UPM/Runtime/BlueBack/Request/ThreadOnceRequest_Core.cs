

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。スレッド。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadOnceRequest_Core
	*/
	public sealed class ThreadOnceRequest_Core<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** item
		*/
		private ITEM item;

		/** cancel
		*/
		private Cancel cancel;

		/** execute
		*/
		private ThreadOnceRequest_Execute_Base<ITEM> execute;

		/** context
		*/
		private System.Threading.SynchronizationContext context;

		/** thread
		*/
		private System.Threading.Thread thread;
		private bool thread_busy;
		private System.UInt64 thread_coremask;
		private ThreadPriority thread_priority;

		/** constructor
		*/
		public ThreadOnceRequest_Core(in ThreadOnceRequest_InitParam<ITEM> a_initparam)
		{
			//item
			this.item = null;

			//cancel
			this.cancel = new Cancel();

			//execute
			this.execute = a_initparam.execute;

			//context
			this.context = a_initparam.context;

			//thread
			this.thread = null;
			this.thread_busy = false;
			this.thread_coremask = a_initparam.coremask;
			this.thread_priority = a_initparam.threadpriority;
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//cancel
			this.cancel.Set(1);

			//thread
			{
				if(this.thread != null){
					this.thread.Join();
					this.thread.Abort();
					this.thread = null;
				}
				this.thread_busy = false;
			}

			//execute
			this.execute = null;

			//this.context
			this.context = null;

			//item
			this.item = null;

			//cancel
			this.cancel.Set(0);
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

		/** 開始。
		*/
		public bool Start(ITEM a_item)
		{
			if(a_item == null){
				return false;
			}

			if(this.thread_busy == true){
				return false;
			}

			//item
			this.item = a_item;

			//Start
			this.thread_busy = true;
			this.thread = new System.Threading.Thread(this.Inner_ThreadMain);
			this.thread.Start();

			return true;
		}

		/** 終了。
		*/
		public void End()
		{
			//thread
			if(this.thread != null){
				this.thread.Join();
				this.thread.Abort();
				this.thread = null;
			}

			//item
			this.item = null;
		}

		/** TryEnd
		*/
		public bool TryEnd()
		{
			if(this.thread_busy == false){
				//thread
				if(this.thread != null){
					this.thread.Join();
					this.thread.Abort();
					this.thread = null;
				}

				//item
				this.item = null;

				return true;
			}else{
				return false;
			}
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

			do{
				//execute
				if(this.execute != null){
					try{
						this.execute.ThreadMain(this.item,this.cancel);
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
						this.context.Post(this.Inner_AfterContextMain,this.item);
					}catch(System.Exception t_exception){
						#if(DEF_BLUEBACK_DEBUG_ASSERT)
						DebugTool.Assert(false,t_exception);
						#endif

						//スレッド終了。
						break;
					}
				}

				//スレッド終了。
				break;
			}while(false);

			//thread_busy
			this.thread_busy = false;
		}
	}
}

