

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadCall
	*/
	public sealed class ThreadCall<PARAM> : System.IDisposable
		where PARAM : class
	{
		/** busy
		*/
		public bool busy;

		/** execute
		*/
		public ThreadCall_Execute_Base<PARAM> execute;

		/** thread
		*/
		public System.Threading.Thread thread;

		/** param
		*/
		public PARAM param;

		/** constructor
		*/
		public ThreadCall()
		{
			//busy
			this.busy = false;

			//execute
			this.execute = null;

			//thread
			this.thread = null;

			//param
			this.param = null;
		}

		/** [System.IDisposable]Dispose
		*/
		public void Dispose()
		{
			if(this.thread != null){
				if(this.busy == true){
					if(this.execute != null){
						this.execute.ThreadCancel(this.param);
					}
				}
			}

			if(this.thread != null){
				this.thread.Join();
				this.thread.Abort();
				this.thread = null;
			}
		}

		/** Start
		*/
		public bool Start(ThreadCall_Execute_Base<PARAM> a_execute,PARAM a_param)
		{
			if(this.thread == null){
				//busy
				this.busy = true;

				//callback
				this.execute = a_execute;

				//param
				this.param = a_param;

				//thread
				this.thread = new System.Threading.Thread(this.Inner_Main);
				this.thread.Start(a_param);

				return true;
			}

			return false;
		}

		/** TryEnd
		*/
		public bool TryEnd()
		{
			if(this.thread != null){
				if(this.busy == true){
					if(this.execute != null){
						this.execute.ThreadCancel((PARAM)this.param);
					}
				}
			}

			if(this.thread != null){
				if(this.busy == false){
					this.thread.Join();
					this.thread.Abort();
					this.thread = null;
				}
			}

			return (this.thread == null);
		}

		/** Inner_Main
		*/
		private void Inner_Main(System.Object a_object)
		{
			if(this.execute != null){
				this.execute.ThreadMain((PARAM)a_object);
			}else{
				#if(DEF_BLUEBACK_DEBUG_ASSERT)
				DebugTool.Assert(false,"execute == null");
				#endif
			}
			this.busy = false;
		}
	}
}

