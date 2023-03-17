using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace testBreak
{
	class Program
	{
		[DllImport("libc", SetLastError = true, EntryPoint = "kill")]
		private static extern int sys_kill(int pid, int sig);
  private static readonly int SIGINT = 2;

		static void Main(string[] args)
		{
			bool isWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
			Debug.WriteLine("OS  : " + (isWin ? "Windows" : "Linux"));
			Debug.WriteLine("Args: " + (args.Length > 0 ? String.Join(' ', args) : ""));

			try
			{
				using Process proc = new Process();
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.FileName = "/code/signaler/signaler";
				proc.StartInfo.Arguments = String.Join(' ', args);

				proc.Start();
				Console.WriteLine();
    Console.WriteLine($"PID={proc.Id} started. Press Enter to exit");
				Console.ReadLine();

				sys_kill(proc.Id, SIGINT);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
