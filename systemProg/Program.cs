using WinApi;
using System;
namespace systemProg
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            var pr1 = WinApiFuncs.CreateProcess("notepad");
            var pr2 = WinApiFuncs.CreateProcess("notepad");
            var pr3 = WinApiFuncs.CreateProcess("notepad");
            var pr4 = WinApiFuncs.CreateProcess("notepad");
            
            Console.WriteLine("Process Started");
            WinApiFuncs.WaitForSingleObject(pr1.hProcess, 10 * 1000);
            WinApiFuncs.TerminateProcess(pr1.hProcess,0);
            Console.WriteLine("process 1 ended");
            
            WinApiFuncs.WaitForSingleObject(pr2.hProcess, 0);
            WinApiFuncs.TerminateProcess(pr2.hProcess,0);
            Console.WriteLine("process 2 ended");
            
            WinApiFuncs.WaitForSingleObject(pr3.hProcess, 10*1000);
            WinApiFuncs.TerminateProcess(pr3.hProcess,0);
            Console.WriteLine("process 3 ended");

            WinApiFuncs.WaitForSingleObject(pr4.hProcess, 0);
            WinApiFuncs.TerminateProcess(pr4.hProcess,0);
            Console.WriteLine("process 4 ended");

            Console.WriteLine("End");
        }
    }
}