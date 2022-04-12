

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
		public struct Item
		{
			public string log;
		}

		/** log_prefix
		*/
		public string log_prefix;

		/** threadrequest
		*/
		public BlueBack.ThreadRequest.ThreadRequest<Main_MonoBehaviour.Item> threadrequest;

		/** Awake
		*/
		private void Awake()
		{
			this.log_prefix = "xxx";

			//threadrequest
			this.threadrequest = new ThreadRequest.ThreadRequest<Main_MonoBehaviour.Item>(new ThreadRequest.InitParam<Main_MonoBehaviour.Item>(){
				execute = this,
			});

			//StartCoroutine
			this.StartCoroutine(this.CoroutineMain());
		}

		/** CoroutineMain
		*/
		public System.Collections.IEnumerator CoroutineMain()
		{
			//Request
			UnityEngine.Debug.Log("Request");
			this.threadrequest.Request(new Item(){
				log = "1",
			});

			//Request
			UnityEngine.Debug.Log("Request");
			this.threadrequest.Request(new Item(){
				log = "2",
			});

			yield return null;

			//Request
			UnityEngine.Debug.Log("Request");
			this.threadrequest.Request(new Item(){
				log = "3",
			});

			//Sleep
			System.Threading.Thread.Sleep(1000);

			//Request
			UnityEngine.Debug.Log("Request");
			this.threadrequest.Request(new Item(){
				log = "4",
			});

			yield break;
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			//threadrequest
			this.threadrequest.Dispose();
		}

		/** [BlueBack.ThreadRequest.Execute_Base<REQUESTITEM>]Load

			System.Threading.Interlocked.Read(ref a_cancel) == 1 : キャンセルリクエストあり。

		*/
		public void Load(in Main_MonoBehaviour.Item a_requestitem,ref long a_cancel)
		{
			UnityEngine.Debug.Log(this.log_prefix + " : " + a_requestitem.log);
		}
	}
}

