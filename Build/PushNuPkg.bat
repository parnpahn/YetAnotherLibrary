@echo off
if "%1" == "" goto Help
goto Push

:Help
echo.
echo Usage: pushnupkg [version]
echo.
echo Example: pushnupkg 1.0.8
goto End

:Push
cd nupkg
..\..\Source\.nuget\nuget push Yalib.%1.nupkg
cd..

:End