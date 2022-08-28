

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。スレッド。リスト。
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
		private ThreadRequest_Core<ITEM> core;

		/** lockobject
		*/
		private object lockobject;

		/** list
		*/
		private System.Collections.Generic.Queue<ITEM> list;

		/** constructor
		*/
		public ThreadRequest_List()
		{
			//[cache]core
			this.core = null;

			//lockobject
			this.lockobject = new object();

			//list
			this.list = new System.Collections.Generic.Queue<ITEM>();
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//lockobject
			this.lockobject = null;

			//list
			if(this.list != null){
				this.list.Clear();
				this.list = null;
			}

			//[cache]core
			this.core = null;
		}

		/** SetCore
		*/
		public void SetCore(ThreadRequest_Core<ITEM> a_core)
		{
			//[cache]core
			this.core = a_core;
		}

		/** 設定。
		*/
		public void Enqueue(ITEM a_item)
		{
			//Enqueue
			lock(this.lockobject){
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
			lock(this.lockobject){
				if(this.list.Count > 0){
					return this.list.Dequeue();
				}
			}

			return null;
		}

		/** GetCount
		*/
		public int GetCount()
		{
			lock(this.lockobject){
				return this.list.Count;
			}
		}
	}
}

