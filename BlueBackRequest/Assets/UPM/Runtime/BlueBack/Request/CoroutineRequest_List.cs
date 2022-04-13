

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
		/** list
		*/
		public System.Collections.Generic.Queue<ITEM> list;

		/** constructor
		*/
		public CoroutineRequest_List()
		{
			//list
			this.list = new System.Collections.Generic.Queue<ITEM>();
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
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
			this.list.Enqueue(a_item);
		}

		/** 取得。

			return == null : データなし。

		*/
		public ITEM Dequeue()
		{
			if(this.list.Count > 0){
				return this.list.Dequeue();
			}

			return null;
		}
	}
}

