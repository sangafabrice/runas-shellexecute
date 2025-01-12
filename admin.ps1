using namespace System.Runtime.InteropServices
using namespace System.Reflection
using namespace Shell32
using assembly Interop.Shell32.dll

function Start-AdminProcess {
  [CmdletBinding()]
  param (
    # The path to the program to execute.
    [string] $programExe,
    # The arguments string used to run the program.
    [string] $arguments,
    # Specifies that the script pauses til the process exits.
    [switch] $wait
  )
  $Shell32 = [ShellClass]::new()
  $Shell32.ShellExecute($programExe, $arguments, [Missing]::Value, 'runas')
  [void][Marshal]::FinalReleaseComObject($Shell32)
  $Shell32 = $null
  if ($wait) {
    Wait-ChildProcess
  }
}

function Wait-ChildProcess {
  [CmdletBinding()]
  param ()
  $childProcess =
    Get-CimInstance Win32_Process -Filter "ParentProcessID='$PID'" |
    Select-Object @{ Label = 'Id'; Expression = { $_.ProcessId } } |
    Get-Process -ErrorAction SilentlyContinue
  if ($null -ne $childProcess) {
    $childProcess.WaitForExit()
    $childProcess.Dispose()
  }
}

Start-AdminProcess $args[0] $args[1] -wait
Start-AdminProcess $args[0] $args[1]
Start-AdminProcess $args[0] $args[1]