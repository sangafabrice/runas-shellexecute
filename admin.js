import System;
import System.Reflection;
import System.Management;
import System.Diagnostics;
import System.Runtime.InteropServices;
import Shell32;

var args = Environment.GetCommandLineArgs();
RunAsAdministrator(args[1], args[2], true);
RunAsAdministrator(args[1], args[2]);
RunAsAdministrator(args[1], args[2]);

/**
 * @param {string} programExe - the path to the program to elevate
 * @param {string} command - the arguments string used to run the program
 * @param {bool} wait - the execution should pause til the process exits
 */
function RunAsAdministrator(programExe, command, wait) {
  var Shell32 = new ShellClass();
  Shell32.ShellExecute(programExe, command, Missing.Value, 'runas');
  Marshal.FinalReleaseComObject(Shell32);
  Shell32 = null;
  if (wait) WaitChildProcessExit();
}

function WaitChildProcessExit() {
  var currentProcess = Process.GetCurrentProcess();
  var currentProcessId = currentProcess.Id;
  currentProcess.Dispose();
  var childProcessQuery =
    'SELECT * FROM Win32_Process ' +
    'WHERE ParentProcessId=' + currentProcessId;
  var searcher = new ManagementObjectSearcher(childProcessQuery);
  var childEnum = searcher.Get().GetEnumerator();
  if (!childEnum.MoveNext()) return;
  var childProcessID = childEnum.Current["ProcessId"];
  try {
    var childProcess = Process.GetProcessById(childProcessID);
    childProcess.WaitForExit();
    childProcess.Dispose();
  } catch (error: ArgumentException) { }
}