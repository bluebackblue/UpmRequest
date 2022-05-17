

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** CoroutineRequest_Cancel
	*/
	public sealed class CoroutineRequest_Cancel
	{
		/** value
		*/
		private volatile int value;

		/** constructor
		*/
		public CoroutineRequest_Cancel(int a_value)
		{
			this.value = a_value;
			System.Threading.Thread.MemoryBarrier();
		}

		/** Set
		*/
		public void Set(int a_value)
		{
			this.value = a_value;
			System.Threading.Thread.MemoryBarrier();
		}

		/** Get
		*/
		public int Get()
		{
			int t_value = this.value;
			System.Threading.Thread.MemoryBarrier();
			return t_value;
		}
	}
}

