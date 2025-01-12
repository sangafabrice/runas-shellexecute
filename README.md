## **Examples**

```powershell
PS> dotnet build admin.csproj
PS> bin\Release\net9.0\admin.exe "powershell" "-NoExit"

PS> dotnet build admin.fsproj
PS> bin\Release\net9.0\admin.exe "powershell" "-NoExit"

PS> jsc -r:Interop.Shell32 admin.js
PS> .\admin.exe "powershell" "-NoExit"

PS> dotnet build admin.vbproj
PS> bin\Release\net9.0\admin.exe "powershell" "-NoExit"

PS> pwsh -NoProfile admin.ps1 "powershell" "-NoExit"
```