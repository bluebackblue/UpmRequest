

/** BlueBack.Request.Samples.ThreadCall
*/
namespace BlueBack.Request.Samples.ThreadCall
{
	/** Main_MonoBehaviour
	*/
	public sealed class Main_MonoBehaviour : UnityEngine.MonoBehaviour , BlueBack.Request.ThreadCall_Execute_Base<Main_MonoBehaviour.Item>
	{
		/** threadcall
		*/
		BlueBack.Request.ThreadCall<Item> threadcall;

		/** count
		*/
		public int count;

		/** Item
		*/
		public class Item
		{
			/** text
			*/
			public string text;

			/** cancel
			*/
			public bool cancel;
		}
		public Item item;

		/** Awake
		*/
		private void Awake()
		{
			//threadcall
			this.threadcall = new ThreadCall<Item>();

			//item
			this.item = new Item();

			//count
			this.count = 0;
		}

		/** Update
		*/
		private void Update()
		{
			if(this.threadcall.TryEnd() == true){
				this.count++;
				this.item.text = this.count.ToString();
				this.item.cancel = false;
				this.threadcall.Start(this,this.item);
			}
		}

		/** [BlueBack.Request.ThreadCall_Execute_Base<PARAM>]スレッドから呼び出される。
		*/
		public void ThreadMain(Item a_param)
		{
			UnityEngine.Debug.Log(a_param.text);

			if(a_param.cancel == false){
				System.Threading.Thread.Sleep(1000);
			}
		}

		/** [BlueBack.Request.ThreadCall_Execute_Base<PARAM>]スレッドキャンセル。
		*/
		public void ThreadCancel(Item a_param)
		{
			a_param.cancel = true;
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			if(this.threadcall != null){
				this.threadcall.Dispose();
				this.threadcall = null;
			}
		}
	}
}

