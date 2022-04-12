

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief ThreadRequest。リスト。
*/


/** BlueBack.ThreadRequest
*/
namespace BlueBack.ThreadRequest
{
	/** RequestList
	*/
	public sealed class RequestList<REQUESTITEM> : System.IDisposable
		where REQUESTITEM : struct
	{
		/** [cache]thread
		*/
		public Thread<REQUESTITEM> thread;

		/** list
		*/
		public System.Collections.Generic.Queue<REQUESTITEM> list;

		/** constructor
		*/
		public RequestList(Thread<REQUESTITEM> a_thread)
		{
			//thread
			this.thread = a_thread;

			//list
			this.list = new System.Collections.Generic.Queue<REQUESTITEM>();
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
		}

		/** Dequeue
		*/
		public bool Dequeue(out REQUESTITEM a_requestitem)
		{
			lock(this.thread.lockobject){
				if(this.list.Count > 0){
					a_requestitem = this.list.Dequeue();
					return true;
				}
			}

			a_requestitem = default;
			return false;
		}

		/** Enqueue
		*/
		public void Enqueue(in REQUESTITEM a_requestitem)
		{
			//Enqueue
			lock(this.thread.lockobject){
				this.list.Enqueue(a_requestitem);
			}

			//Wakeup
			this.thread.Wakeup();
		}
	}
}

