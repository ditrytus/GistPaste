@echo off
cls
::".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
echo Test test
"packages\FAKE.4.39.0\tools\Fake.exe" build.fsx