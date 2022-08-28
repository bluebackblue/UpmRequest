

/** BlueBack.Request.Samples.CoroutineRequest
*/
namespace BlueBack.Request.Samples.CoroutineRequest
{
	/** Main_MonoBehaviour
	*/
	public sealed class Main_MonoBehaviour : UnityEngine.MonoBehaviour , BlueBack.Request.CoroutineRequest_Execute_Base<Main_MonoBehaviour.Item>
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
		public BlueBack.Request.CoroutineRequest<Main_MonoBehaviour.Item> request;

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
			//initparam
			BlueBack.Request.CoroutineRequest_InitParam<Main_MonoBehaviour.Item> t_initparam = BlueBack.Request.CoroutineRequest_InitParam<Main_MonoBehaviour.Item>.CreateDefault();
			{
				t_initparam.execute = this;
				t_initparam.monobehaviour = this;
			}

			//request
			this.request = new BlueBack.Request.CoroutineRequest<Main_MonoBehaviour.Item>(in t_initparam);

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
					this.count++;
					this.request.Request(new Item(){text = this.count.ToString()});
				}
			}
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			//request
			if(this.request != null){
				this.request.Dispose();
				this.request = null;
			}
		}

		/** [BlueBack.Request.CoroutineRequest_Execute_Base<ITEM>]コルーチンから呼び出される。

			a_cancel.Get() != 0 : キャンセルリクエストあり。

		*/
		public System.Collections.IEnumerator CoroutineMain(Item a_item,BlueBack.Request.Cancel a_cancel)
		{
			if(a_cancel.Get() == 0){
				UnityEngine.Debug.Log(a_item.text);
			}

			for(int ii=0;ii<100;ii++){
				if(a_cancel.Get() == 0){
					yield return new UnityEngine.WaitForSeconds(0.01f);
				}else{
					UnityEngine.Debug.Log("CoroutineMain : Cancel");
					break;
				}
			}

			//busy
			this.busy = false;
			yield break;
		}
	}
}

