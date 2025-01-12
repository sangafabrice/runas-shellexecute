''' admin.vbs

RunAsAdministrator WScript.Arguments(0), WScript.Arguments(1)

''' <param name="strProgramExe">The path to the program to elevate.</param>
''' <param name="strArguments">The arguments used to run the program.</param>
Sub RunAsAdministrator(ByVal strProgramExe, ByVal strArguments)
  Set Shell32 = CreateObject("Shell.Application")
  Shell32.ShellExecute strProgramExe, strArguments,, "runas"
  Set Shell32 = Nothing
End Sub