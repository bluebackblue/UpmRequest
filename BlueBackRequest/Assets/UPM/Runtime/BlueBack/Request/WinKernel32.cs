

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。
*/


/** BlueBack.Request
*/
#if((UNITY_STANDALONE_WIN)||(UNITY_EDITOR_WIN))
namespace BlueBack.Request
{
	/** using
	*/
	using DWORD_PTR = System.UInt32; 
	using DWORD = System.UInt32; 
	using HANDLE = System.UInt32; 
	using BOOL = System.Int32; 

	/** WinKernel32
	*/
	public static class WinKernel32
	{
		/** SetThreadAffinityMask

			https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setthreadaffinitymask

		*/
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern DWORD_PTR SetThreadAffinityMask(HANDLE hThread,DWORD_PTR dwThreadAffinityMask);

		/** GetCurrentThread

			https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getcurrentthread

		*/
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern HANDLE GetCurrentThread();

		/** GetCurrentThreadId

			https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getcurrentthreadid

		*/
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern DWORD GetCurrentThreadId();

		/** GetCurrentThreadId

			https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-setthreadpriority

		*/
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern BOOL SetThreadPriority(HANDLE hThread,int nPriority);

		/** THREAD
		*/
		public const int THREAD_MODE_BACKGROUND_BEGIN	= 0x00010000;
		public const int THREAD_MODE_BACKGROUND_END		= 0x00020000;
		public const int THREAD_PRIORITY_ABOVE_NORMAL	= 1;
		public const int THREAD_PRIORITY_BELOW_NORMAL	= -1;
		public const int THREAD_PRIORITY_HIGHEST		= 2;
		public const int THREAD_PRIORITY_IDLE			= -15;
		public const int THREAD_PRIORITY_LOWEST			= -2;
		public const int THREAD_PRIORITY_NORMAL			= 0;
		public const int THREAD_PRIORITY_TIME_CRITICAL	= 15;
	}
}
#endif

