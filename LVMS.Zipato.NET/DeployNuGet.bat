@echo off
del *.nupkg
@echo on
".nuget/nuget.exe" pack ZipatoNet.Signed.nuspec -symbols

".nuget/nuget.exe" push LVMS.ZipatoNet.Signed.*.nupkg

pause