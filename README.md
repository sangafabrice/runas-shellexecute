## **Examples**

```powershell
PS> csc admin.cs
PS> .\admin.exe "powershell" "-NoExit"

PS> fsi -r:Interop.Shell32 -r:System.Management admin.fsx "powershell" "-NoExit"

PS> jsc -r:Interop.Shell32 admin.js
PS> .\admin.exe "powershell" "-NoExit"

PS> vbc -r:Interop.Shell32.dll admin.vb
PS> .\admin.exe "powershell" "-NoExit"

PS> powershell -NoProfile -File admin.ps1 powershell -NoExit

PS> pwsh -NoProfile admin.ps1 "powershell" "-NoExit"
```