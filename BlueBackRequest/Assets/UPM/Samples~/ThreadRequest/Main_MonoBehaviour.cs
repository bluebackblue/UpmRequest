

/** BlueBack.Request.Samples.ThreadRequest
*/
namespace BlueBack.Request.Samples.ThreadRequest
{
	/** Main_MonoBehaviour
	*/
    public class Main_MonoBehaviour : UnityEngine.MonoBehaviour , BlueBack.Request.ThreadRequest_Execute_Base<Main_MonoBehaviour.Item>
    {
		/** Item
		*/
		public class Item
		{
			/** log
			*/
			public string log;
		}

		/** threadrequest
		*/
		public BlueBack.Request.ThreadRequest<Main_MonoBehaviour.Item> threadrequest;

		/** Awake
		*/
		private void Awake()
		{
			//initparam
			BlueBack.Request.ThreadRequest_InitParam<Main_MonoBehaviour.Item> t_initparam = BlueBack.Request.ThreadRequest_InitParam<Main_MonoBehaviour.Item>.CreateDefault();
			{
				t_initparam.execute = this;
			}

			//threadrequest
			this.threadrequest = new BlueBack.Request.ThreadRequest<Main_MonoBehaviour.Item>(in t_initparam);

			//StartCoroutine
			this.StartCoroutine(this.CoroutineMain());
		}

		/** CoroutineMain
		*/
		public System.Collections.IEnumerator CoroutineMain()
		{
			//Request
			UnityEngine.Debug.Log("Request : start");
			this.threadrequest.Request(new Item(){
				log = "log1",
			});
			UnityEngine.Debug.Log("Request : end");

			//Request
			UnityEngine.Debug.Log("Request : start");
			this.threadrequest.Request(new Item(){
				log = "log2",
			});
			UnityEngine.Debug.Log("Request : end");

			yield return null;

			//Request
			UnityEngine.Debug.Log("Request : start");
			this.threadrequest.Request(new Item(){
				log = "log3",
			});
			UnityEngine.Debug.Log("Request : end");

			//Sleep
			System.Threading.Thread.Sleep(1000);

			//Request
			UnityEngine.Debug.Log("Request : start");
			this.threadrequest.Request(new Item(){
				log = "log4",
			});
			UnityEngine.Debug.Log("Request : end");

			yield break;
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			//threadrequest
			if(this.threadrequest != null){
				this.threadrequest.Dispose();
				this.threadrequest = null;
			}
		}

		/** [BlueBack.Request.ThreadRequest_Execute_Base<ITEM>]ThreadExecute

			System.Threading.Interlocked.Read(ref a_cancel) == 1 : キャンセルリクエストあり。

		*/
		public void ThreadExecute(Main_MonoBehaviour.Item a_item,ref long a_cancel)
		{
			UnityEngine.Debug.Log(string.Format("ThreadExecute : {0} : {1}",System.Threading.Thread.CurrentThread.ManagedThreadId,a_item.log));
		}

		/** [BlueBack.Request.ThreadRequest_Execute_Base<ITEM>]AfterContextExecute
		*/
		public void AfterContextExecute(Main_MonoBehaviour.Item a_item)
		{
			UnityEngine.Debug.Log(string.Format("AfterContextExecute : {0} : {1}",System.Threading.Thread.CurrentThread.ManagedThreadId,a_item.log));
		}
	}
}

