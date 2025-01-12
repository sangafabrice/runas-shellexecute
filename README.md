## **Examples**

```powershell
PS> wscript admin.js "powershell" "-NoExit"

PS> wscript admin.vbs "powershell" "-NoExit"

PS> powershell -NoProfile -File admin.ps1 "powershell" "-NoExit"

PS> pwsh -NoProfile admin.ps1 "powershell" "-NoExit"
```

## **Test Limited Access**

It is preferable to start the next PowerShell with the argument `-NoProfile`.

```powershell
PS> Set-PSBreakpoint -Script admin.ps1 -Line 33 -Action {
  Wait-Process -Id $childProcess.Id 2>&1 | Out-Host
}

PS> .\admin.ps1 "powershell" "-NoExit"
```