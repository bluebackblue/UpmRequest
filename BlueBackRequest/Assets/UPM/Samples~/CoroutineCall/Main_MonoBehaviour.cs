

/** BlueBack.Request.Samples.CoroutineCall
*/
namespace BlueBack.Request.Samples.CoroutineCall
{
	/** Main_MonoBehaviour
	*/
	public sealed class Main_MonoBehaviour : UnityEngine.MonoBehaviour , BlueBack.Request.CoroutineCall_Execute_Base<Main_MonoBehaviour.Item>
	{
		/** coroutinecall
		*/
		BlueBack.Request.CoroutineCall<Item> coroutinecall;

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
			//coroutinecall
			this.coroutinecall = new CoroutineCall<Item>();

			//item
			this.item = new Item();

			//count
			this.count = 0;
		}

		/** Update
		*/
		private void Update()
		{
			if(this.coroutinecall.TryEnd() == true){
				this.count++;
				this.item.text = this.count.ToString();
				this.item.cancel = false;
				this.coroutinecall.Start(this,this.item,this);
			}
		}

		/** [BlueBack.Request.CoroutineCall_Execute_Base<PARAM>]コルーチンから呼び出される。
		*/
		public System.Collections.IEnumerator CoroutineMain(Item a_param)
		{
			UnityEngine.Debug.Log(a_param.text);

			if(a_param.cancel == false){
				yield return new UnityEngine.WaitForSeconds(1.0f);
			}

			yield break;
		}

		/** [BlueBack.Request.CoroutineCall_Execute_Base<PARAM>]コルーチンキャンセル。
		*/
		public void CoroutineCancel(Item a_param)
		{
			a_param.cancel = true;
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			if(this.coroutinecall != null){
				this.coroutinecall.Dispose();
				this.coroutinecall = null;
			}
		}
	}
}

