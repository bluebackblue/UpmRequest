

/** BlueBack.ThreadRequest.Samples.Simple
*/
namespace BlueBack.PoolList.Samples.Simple
{
	/** Main_MonoBehaviour
	*/
    public class Main_MonoBehaviour : UnityEngine.MonoBehaviour , BlueBack.ThreadRequest.Execute_Base<Main_MonoBehaviour.Item>
    {
		/** Item
		*/
		public class Item
		{
			public string log;
		}

		/** threadrequest
		*/
		public BlueBack.ThreadRequest.ThreadRequest<Main_MonoBehaviour.Item> threadrequest;

		/** Awake
		*/
		private void Awake()
		{
			//initparam
			ThreadRequest.InitParam<Main_MonoBehaviour.Item> t_initparam = ThreadRequest.InitParam<Main_MonoBehaviour.Item>.CreateDefault();
			{
				t_initparam.execute = this;
			}

			//threadrequest
			this.threadrequest = new ThreadRequest.ThreadRequest<Main_MonoBehaviour.Item>(in t_initparam);

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
			this.threadrequest.Dispose();
		}

		/** [BlueBack.ThreadRequest.Execute_Base<REQUESTITEM>]ThreadExecute

			System.Threading.Interlocked.Read(ref a_cancel) == 1 : キャンセルリクエストあり。

		*/
		public void ThreadExecute(Main_MonoBehaviour.Item a_requestitem,ref long a_cancel)
		{
			UnityEngine.Debug.Log(string.Format("ThreadExecute : {0} : {1}",System.Threading.Thread.CurrentThread.ManagedThreadId,a_requestitem.log));
		}

		/** [BlueBack.ThreadRequest.Execute_Base<REQUESTITEM>]AfterContextExecute
		*/
		public void AfterContextExecute(Main_MonoBehaviour.Item a_requestitem)
		{
			UnityEngine.Debug.Log(string.Format("AfterContextExecute : {0} : {1}",System.Threading.Thread.CurrentThread.ManagedThreadId,a_requestitem.log));
		}
	}
}

