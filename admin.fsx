open System
open System.Reflection
open System.Management
open System.Diagnostics
open System.Runtime.InteropServices
open Shell32

let WaitChildProcessExit () =
  use currentProcess = Process.GetCurrentProcess()
  let currentProcessId = currentProcess.Id |> string
  let childProcessQuery =
    "SELECT * FROM Win32_Process " +
    "WHERE ParentProcessId=" + currentProcessId
  let searcher = new ManagementObjectSearcher(childProcessQuery)
  let childEnum = searcher.Get().GetEnumerator()
  if childEnum.MoveNext() then
    try
      use childProcess =
        childEnum.Current.["ProcessId"]
        |> Convert.ToInt32
        |> Process.GetProcessById
      childProcess.WaitForExit()
    with
      | :? ArgumentException -> ignore()

/// <param name="programExe">The path to the program to elevate.</param>
/// <param name="arguments">The arguments used to run the program.</param>
/// <param name="wait">The execution should pause til the process exits.</param>
let RunAsAdministrator (programExe: string) (arguments: string) (wait: bool) =
  let mutable Shell32 = new ShellClass()
  Shell32.ShellExecute(programExe, arguments, Missing.Value, "runas")
  Marshal.FinalReleaseComObject Shell32 |> ignore
  Shell32 <- null
  if wait then WaitChildProcessExit()

let args = Array.sub fsi.CommandLineArgs 1 2
RunAsAdministrator args.[0] args.[1] true
RunAsAdministrator args.[0] args.[1] false
RunAsAdministrator args.[0] args.[1] false