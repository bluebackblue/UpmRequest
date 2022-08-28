

/** BlueBack.Request.Samples.ThreadOnceRequest
*/
namespace BlueBack.Request.Samples.ThreadOnceRequest
{
	/** Main_MonoBehaviour
	*/
	public sealed class Main_MonoBehaviour : UnityEngine.MonoBehaviour , BlueBack.Request.ThreadOnceRequest_Execute_Base<Main_MonoBehaviour.Item>
	{
		/** Item
		*/
		public sealed class Item
		{
			/** text
			*/
			public string text;
		}

		/** request
		*/
		public BlueBack.Request.ThreadOnceRequest<Item> request;

		/** count
		*/
		public int count;

		/** busy
		*/
		public bool busy;

		/** cancel
		*/
		public bool cancel;
		private bool cancel_old;

		/** Awake
		*/
		private void Awake()
		{
			//request
			{
				BlueBack.Request.ThreadOnceRequest_InitParam<Item> t_initparam = BlueBack.Request.ThreadOnceRequest_InitParam<Item>.CreateDefault();
				{
					t_initparam.execute = this;
				}
				this.request = new ThreadOnceRequest<Item>(in t_initparam);
			}

			//count
			this.count = 0;

			//busy
			this.busy = false;

			//cancel
			this.cancel = false;
			this.cancel_old = false;
		}

		/** Update
		*/
		private void Update()
		{
			if(this.cancel != this.cancel_old){
				this.cancel_old = this.cancel;
				this.request.SetCancelValue(this.cancel == true ? (long)1 : (long)0);
			}

			if(this.busy == false){
				if(this.request.GetCancelValue() == 0){
					this.busy = true;
					if(this.request.Start(new Item(){text = this.count.ToString()}) == true){
						this.count++;
					}else{
						UnityEngine.Debug.LogError("error");
						this.busy = false;
					}
				}
			}
		}

		/** [BlueBack.Request.ThreadOnceRequest_Execute_Base<ITEM>]スレッドから呼び出される。

			a_cancel.Get() != 0 : キャンセルリクエストあり。

		*/
		public void ThreadMain(Item a_item,Cancel a_cancel)
		{
			if(a_cancel.Get() == 0){
				UnityEngine.Debug.Log(a_item.text);
			}

			for(int ii=0;ii<100;ii++){
				if(a_cancel.Get() == 0){
					System.Threading.Thread.Sleep(10);
				}else{
					UnityEngine.Debug.Log("ThreadMain : Cancel");
					break;
				}
			}

			//busy
			this.busy = false;
		}

		/** [BlueBack.Request.ThreadRequest_Execute_Base<ITEM>]コンテキストから呼び出される。
		*/
		public void AfterContextMain(Item a_item)
		{
			UnityEngine.Debug.Log(string.Format("AfterContextExecute : {0} : {1}",System.Threading.Thread.CurrentThread.ManagedThreadId,a_item.text));
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			if(this.request != null){
				this.request.Dispose();
				this.request = null;
			}
		}
	}
}

