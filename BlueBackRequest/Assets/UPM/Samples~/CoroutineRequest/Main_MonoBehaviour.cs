

/** BlueBack.Request.Samples.CoroutineRequest
*/
namespace BlueBack.Request.Samples.CoroutineRequest
{
	/** Main_MonoBehaviour
	*/
    public class Main_MonoBehaviour : UnityEngine.MonoBehaviour , BlueBack.Request.CoroutineRequest_Execute_Base<Main_MonoBehaviour.Item>
    {
		/** Item
		*/
		public class Item
		{
			/** log
			*/
			public string log;
		}

		/** coroutinerequest
		*/
		public BlueBack.Request.CoroutineRequest<Main_MonoBehaviour.Item> coroutinerequest;

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

			//coroutinerequest
			this.coroutinerequest = new BlueBack.Request.CoroutineRequest<Main_MonoBehaviour.Item>(in t_initparam);

			//StartCoroutine
			this.StartCoroutine(this.CoroutineMain());
		}

		/** CoroutineMain
		*/
		public System.Collections.IEnumerator CoroutineMain()
		{
			//Request
			UnityEngine.Debug.Log("Request : start");
			this.coroutinerequest.Request(new Item(){
				log = "log1",
			});
			UnityEngine.Debug.Log("Request : end");

			//Request
			UnityEngine.Debug.Log("Request : start");
			this.coroutinerequest.Request(new Item(){
				log = "log2",
			});
			UnityEngine.Debug.Log("Request : end");

			yield return null;
			yield return null;
			yield return null;

			//Request
			UnityEngine.Debug.Log("Request : start");
			this.coroutinerequest.Request(new Item(){
				log = "log3",
			});
			UnityEngine.Debug.Log("Request : end");

			//Request
			UnityEngine.Debug.Log("Request : start");
			this.coroutinerequest.Request(new Item(){
				log = "log4",
			});
			UnityEngine.Debug.Log("Request : end");

			yield break;
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			//coroutinerequest
			if(this.coroutinerequest != null){
				this.coroutinerequest.Dispose();
				this.coroutinerequest = null;
			}
		}

		/** [BlueBack.Request.CoroutineRequest_Execute_Base<ITEM>]CoroutineExecute

			a_cancel.value == 1 : キャンセルリクエストあり。

		*/
		public System.Collections.IEnumerator CoroutineExecute(Main_MonoBehaviour.Item a_item,CoroutineRequest_Cancel a_cancel)
		{
			yield return null;

			UnityEngine.Debug.Log(string.Format("CoroutineExecute : {0} : {1}",System.Threading.Thread.CurrentThread.ManagedThreadId,a_item.log));

			yield break;
		}
	}
}

