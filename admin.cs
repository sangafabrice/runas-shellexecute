using System;
using System.Management;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

static class Program
{
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
    Type ShellType = Type.GetTypeFromProgID("Shell.Application");
    dynamic Shell32 = Activator.CreateInstance(ShellType);
    Shell32.ShellExecute(programExe, arguments, Missing.Value, "runas");
    Marshal.FinalReleaseComObject(Shell32);
    Shell32 = null;
    if (wait) WaitChildProcessExit();
  }

  static void WaitChildProcessExit()
  {
    int currentProcessId;
    using (Process currentProcess = Process.GetCurrentProcess())
      currentProcessId = currentProcess.Id;
    string childProcessQuery =
      "SELECT * FROM Win32_Process " +
      "WHERE ParentProcessId=" + currentProcessId;
    var searcher = new ManagementObjectSearcher(childProcessQuery);
    var childEnum = searcher.Get().GetEnumerator();
    if (!childEnum.MoveNext()) return;
    int childProcessID = Convert.ToInt32(childEnum.Current["ProcessId"]);
    try
    {
      using (Process childProcess = Process.GetProcessById(childProcessID))
        childProcess.WaitForExit();
    }
    catch (ArgumentException)
    { /* Do nothing when the process has already exited */ }
  }
}