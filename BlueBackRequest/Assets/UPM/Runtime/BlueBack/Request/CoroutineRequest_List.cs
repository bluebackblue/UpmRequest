

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。リスト。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineRequest_List
	*/
	public sealed class CoroutineRequest_List<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** lockobject
		*/
		private object lockobject;

		/** list
		*/
		private System.Collections.Generic.Queue<ITEM> list;

		/** constructor
		*/
		public CoroutineRequest_List()
		{
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
		}

		/** 設定。
		*/
		public void Enqueue(ITEM a_item)
		{
			//Enqueue
			lock(this.lockobject){
				this.list.Enqueue(a_item);
			}
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
	}
}

