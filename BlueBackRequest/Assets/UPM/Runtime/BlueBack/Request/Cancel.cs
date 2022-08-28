

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。コルーチン。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** Cancel
	*/
	public sealed class Cancel
	{
		/** value
		*/
		private long value;

		/** constructor
		*/
		public Cancel()
		{
			System.Threading.Interlocked.Exchange(ref this.value,0);
		}

		/** Set
		*/
		public void Set(long a_value)
		{
			System.Threading.Interlocked.Exchange(ref this.value,a_value);
		}

		/** Get
		*/
		public long Get()
		{
			return System.Threading.Interlocked.Read(ref this.value);
		}
	}
}

