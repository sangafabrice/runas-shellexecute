using System;
using System.Management;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Shell32;

static class Program
{
  [STAThread]
  static void Main(string[] args)
  {
    RunAsAdministrator(args[0], args[1], true);
    RunAsAdministrator(args[0], args[1]);
    RunAsAdministrator(args[0], args[1]);
  }

  /// <param name="programExe">The path to the program to elevate.</param>
  /// <param name="arguments">The arguments used to run the program.</param>
  /// <param name="wait">The execution should pause til the process exits.</param>
  static void RunAsAdministrator(string programExe, string arguments, bool wait = false)
  {
    Shell Shell32 = new();
    Shell32.ShellExecute(programExe, arguments, Missing.Value, "runas");
    Marshal.FinalReleaseComObject(Shell32);
    Shell32 = null;
    if (wait) WaitChildProcessExit();
  }

  static void WaitChildProcessExit()
  {
    var childEnum = new ManagementObjectSearcher(
      "SELECT * FROM Win32_Process " +
      "WHERE ParentProcessId=" + Environment.ProcessId
    ).Get().GetEnumerator();
    if (!childEnum.MoveNext()) return;
    try
    {
      using (Process childProcess = Process.GetProcessById(Convert.ToInt32(childEnum.Current["ProcessId"])))
        childProcess.WaitForExit();
    }
    catch (ArgumentException)
    { /* Do nothing when the process has already exited */ }
  }
}