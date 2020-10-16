using System;
using System.Runtime.InteropServices;

namespace WinApi
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Startupinfo
    {
        public Int32 cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public Int32 dwX;
        public Int32 dwY;
        public Int32 dwXSize;
        public Int32 dwYSize;
        public Int32 dwXCountChars;
        public Int32 dwYCountChars;
        public Int32 dwFillAttribute;
        public Int32 dwFlags;
        public Int16 wShowWindow;
        public Int16 cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessInformation
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }
    public class WinApiFuncs
    {
        public const uint INFINITE = 0xFFFFFFFF;
        public const uint WAIT_ABANDONED = 0x00000080;
        public const uint WAIT_OBJECT_0 = 0x00000000;
        public const uint WAIT_TIMEOUT = 0x00000102;
        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        public const uint CREATE_SUSPENDED = 4;
        public const uint CREATE_NEW_CONSOLE = 0x00000010;
        public const uint CREATE_NO_WINDOW = 0x08000000;

        public const int STARTF_USECOUNTCHARS = 0x00000008;
        public const int STARTF_USEFILLATTRIBUTE = 0x00000010;
        public const int STARTF_USEPOSITION = 0x00000004;
        public const int STARTF_USESIZE = 0x00000002;
        public const int STARTF_USESTDHANDLES = 0x00000100;


        public const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const uint SYNCHRONIZE = 0x00100000;
        public const uint EVENT_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x3);
        public const uint EVENT_MODIFY_STATE = 0x0002;
        public const uint ERROR_ALREADY_EXISTS = 183;
        
        public const UInt32 MUTEX_ALL_ACCESS = 0x1F0001;
        public const UInt32 MUTEX_MODIFY_STATE = 0x0001;

        public const uint TIMER_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x3);
        public const uint TIMER_MODIFY_STATE = 0x0002;


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, 
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles,
            uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory,
            [In] ref Startupinfo lpStartupInfo, out ProcessInformation lpProcessInformation);
        public static ProcessInformation CreateProcess(string lpApplicationName, string lpCommandLine, uint dwCreationFlags = 0)
        {
            ProcessInformation pi;
            var si = new Startupinfo();
            si.cb = Marshal.SizeOf(si);
            var ret = CreateProcess(lpApplicationName, lpCommandLine, IntPtr.Zero, IntPtr.Zero, false, dwCreationFlags,
                                    IntPtr.Zero, null, ref si, out pi);
            return ret ? pi : new ProcessInformation { hProcess = INVALID_HANDLE_VALUE, hThread = INVALID_HANDLE_VALUE };

        }
        public static ProcessInformation CreateProcess(string lpCommandLine, uint dwCreationFlags = 0)
        {
            ProcessInformation pi;
            var si = new Startupinfo();
            si.cb = Marshal.SizeOf(si);
            var ret = CreateProcess(null, lpCommandLine, IntPtr.Zero, IntPtr.Zero, false, dwCreationFlags,
                                    IntPtr.Zero, null, ref si, out pi);
            return ret ? pi : new ProcessInformation { hProcess = INVALID_HANDLE_VALUE, hThread = INVALID_HANDLE_VALUE };

        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern void ExitProcess(uint uExitCode);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
        public static int GetLastError()
        {
            return Marshal.GetLastWin32Error();
        }
        // Wait functions
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
        [DllImport("kernel32.dll")]
        public static extern uint WaitForMultipleObjects(uint nCount, IntPtr[] lpHandles, bool bWaitAll, uint dwMilliseconds);

        //  Threads
        public delegate void ThreadMethod(IntPtr data);
        //System.Threading.ThreadStart

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateThread(IntPtr securityAttributes, uint stackSize, 
            ThreadMethod startFunction, IntPtr threadParameter, uint creationFlags, out uint threadId);
        public static IntPtr CreateThread(ThreadMethod startFunction, IntPtr threadParameter, uint creationFlags = 0)
        {
            uint dw;
            return CreateThread(IntPtr.Zero, 0, startFunction, threadParameter, creationFlags, out dw);
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        public static extern void ExitThread(uint dwExitCode);
        [DllImport("kernel32.dll")]
        public static extern bool TerminateThread(IntPtr hThread, uint dwExitCode);
        // EVENTS
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);
        [DllImport("kernel32.dll")]
        public static extern bool SetEvent(IntPtr hEvent);
        [DllImport("kernel32.dll")]
        public static extern bool ResetEvent(IntPtr hEvent);
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenEvent(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);
        [DllImport("kernel32.dll")]
        public static extern bool ReleaseMutex(IntPtr hMutex);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenMutex(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        // Waitable Timers
        
        public const long NanoToSec = -10000000;

        public delegate void TimerApcProc(IntPtr arg, uint lowTime, uint highTime);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long ft, int lPeriod, 
            TimerApcProc pfnCompletionRoutine, IntPtr pArgToCompletionRoutine, bool fResume);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CancelWaitableTimer(IntPtr hTimer);
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenWaitableTimer(uint dwDesiredAccess, bool bInheritHandle, string lpName);
    }
}
