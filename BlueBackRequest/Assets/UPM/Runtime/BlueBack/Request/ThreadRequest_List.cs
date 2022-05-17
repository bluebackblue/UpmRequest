

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。リスト。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadRequest_List
	*/
	public sealed class ThreadRequest_List<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** [cache]core
		*/
		public ThreadRequest_Core<ITEM> core;

		/** list
		*/
		public System.Collections.Generic.Queue<ITEM> list;

		/** constructor
		*/
		public ThreadRequest_List(ThreadRequest_Core<ITEM> a_core)
		{
			//core
			this.core = a_core;

			//list
			this.list = new System.Collections.Generic.Queue<ITEM>();
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//core
			this.core = null;

			//list
			if(this.list != null){
				this.list.Clear();
				this.list = null;
			}
		}

		/** 設定。
		*/
		public void Enqueue(ITEM a_item)
		{
			//Enqueue
			lock(this.core.lockobject){
				this.list.Enqueue(a_item);
			}

			//Wakeup
			this.core.Wakeup();
		}

		/** 取得。

			return == null : データなし。

		*/
		public ITEM Dequeue()
		{
			lock(this.core.lockobject){
				if(this.list.Count > 0){
					return this.list.Dequeue();
				}
			}

			return null;
		}
	}
}

