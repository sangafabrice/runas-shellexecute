Imports System.Management
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports Shell32

Module Program
  Sub Main(args As String())
    RunAsAdministrator(args(0), args(1), True)
    RunAsAdministrator(args(0), args(1))
    RunAsAdministrator(args(0), args(1))
  End Sub

  ''' <param name="programExe">The path to the program to elevate.</param>
  ''' <param name="arguments">The arguments used to run the program.</param>
  ''' <param name="wait">The execution should pause til the process exits.</param>
  Sub RunAsAdministrator(programExe As String, arguments As String, Optional wait As Boolean = False)
    Dim Shell32 As New Shell   ' ShellClass
    Shell32.ShellExecute(programExe, arguments,, "runas")
    Marshal.FinalReleaseComObject(Shell32)
    Shell32 = Nothing
    If wait Then WaitChildProcessExit()
  End Sub

  Sub WaitChildProcessExit()
    Dim childEnum = (New ManagementObjectSearcher(
      "SELECT * FROM Win32_Process " &
      "WHERE ParentProcessId=" & Environment.ProcessId
    )).Get().GetEnumerator()
    If Not childEnum.MoveNext() Then Exit Sub
    Try
      Using childProcess = Process.GetProcessById(childEnum.Current("ProcessId"))
        childProcess.WaitForExit()
      End Using
    Catch ex As ArgumentException
    End Try
  End Sub
End Module