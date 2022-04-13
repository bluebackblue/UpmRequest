

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
		public ThreadRequest_List<ITEM> list;

		/** execute
		*/
		public ThreadRequest_Execute_Base<ITEM> execute;

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

		/** constructor
		*/
		public ThreadRequest_Core()
		{
			//list
			this.list = null;

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
			this.raw = new System.Threading.Thread(this.Inner_ThreadMain);
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

			//list
			this.list = null;

			//execute
			this.execute = null;
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
			#pragma warning restore

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

				//Inner_AfterContextExecute
				if(t_item != null){
					if(this.context != null){
						try{
							System.Threading.Thread.MemoryBarrier();
							this.context.Post(this.Inner_AfterContextExecute,t_item);
						}catch(System.Exception t_exception){
							#if(DEF_BLUEBACK_REQUEST_ASSERT)
							DebugTool.Assert(false,t_exception.Message);
							#endif

							//スレッド終了。
							break;
						}
					}
				}

				//Reset
				try{
					lock(this.lockobject){
						if(this.list.list.Count == 0){
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

