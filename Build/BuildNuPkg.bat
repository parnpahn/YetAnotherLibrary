@echo off
if "%1" == "" goto Help
goto CreateFolder

:Help
echo.
echo Usage: buildnupkg [version]
echo.
echo Example: buildnupkg 1.0.8
goto End

:CreateFolder
if exist nupkg goto Build
md nupkg

:Build
..\Source\.nuget\NuGet.exe pack Yalib.symbols.nuspec -OutputDirectory nupkg -Symbols -Version %1

:End