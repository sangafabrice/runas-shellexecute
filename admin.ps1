using namespace System.Runtime.InteropServices
using namespace System.Reflection

function Start-AdminProcess {
  [CmdletBinding()]
  param (
    # The path to the program to execute.
    [string] $programExe,
    # The arguments string used to run the program.
    [string] $arguments
  )
  $Shell32 = New-Object -ComObject 'Shell.Application' # Late-binding
  $Shell32.ShellExecute($programExe, $arguments, [Missing]::Value, 'runas')
  [void][Marshal]::FinalReleaseComObject($Shell32)
  $Shell32 = $null
}

Start-AdminProcess $args[0] $args[1]