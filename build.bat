@echo off
cls
"tools\NuGet.exe" "restore"
"packages\FAKE.4.39.0\tools\Fake.exe" build.fsx %1