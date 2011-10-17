@echo off
set DEPS_DIR=lib\packages
set NUGET_EXE=bin\nuget\nuget.exe
set NUSPEC=netzmq.nuspec
set VERSION_INFO_CS=src\proj\VersionAssemblyInfo.cs
set VERSION_INFO_CPP=src\proj\ZeroMQ.Proxy\VersionAssemblyInfo.cpp
set INTERNALS_INFO_CS=src\proj\InternalsAssemblyInfo.cs

set /p VERSION=Enter version (e.g. 1.0): 
set /p BUILD=Enter a build (e.g. 11234): 
set /p REVISION=Enter a revision (e.g. 7): 
set /p MATURITY=Enter maturity (e.g. Alpha, Beta, RC, Release, etc.): 

:: C# shared version info
move %VERSION_INFO_CS% %VERSION_INFO_CS%.bak
echo using System.Reflection; > %VERSION_INFO_CS%
echo. >> %VERSION_INFO_CS%
echo [assembly: AssemblyVersion("%VERSION%.0.0")] >> %VERSION_INFO_CS%
echo [assembly: AssemblyFileVersion("%VERSION%.%BUILD%.%REVISION%")] >> %VERSION_INFO_CS%
echo [assembly: AssemblyInformationalVersion("%VERSION%.%BUILD%.%REVISION% %MATURITY%")] >> %VERSION_INFO_CS%

:: VC++ proxy version info
move %VERSION_INFO_CPP% %VERSION_INFO_CPP%.bak
echo #include "stdafx.h"; > %VERSION_INFO_CPP%
echo. >> %VERSION_INFO_CPP%
echo using namespace System::Reflection; >> %VERSION_INFO_CPP%
echo. >> %VERSION_INFO_CPP%
echo [assembly:AssemblyVersionAttribute("%VERSION%.0.0")]; >> %VERSION_INFO_CPP%
echo [assembly:AssemblyFileVersionAttribute("%VERSION%.%BUILD%.%REVISION%")]; >> %VERSION_INFO_CPP%
echo [assembly:AssemblyInformationalVersionAttribute("%VERSION%.%BUILD%.%REVISION% %MATURITY%")]; >> %VERSION_INFO_CPP%

:: InternalsVisibleTo assembly info
move %INTERNALS_INFO_CS% %INTERNALS_INFO_CS%.bak
echo using System.Runtime.CompilerServices; > %INTERNALS_INFO_CS%
echo. >> %INTERNALS_INFO_CS%
echo [assembly: InternalsVisibleTo("ZeroMQ.Proxy, PublicKeyToken=9078bbfa744a021a")] >> %INTERNALS_INFO_CS%

if not exist %DEPS_DIR% call deps.cmd

%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild build.proj /target:Package /Property:Configuration=Release

%NUGET_EXE% Pack %NUSPEC% -Version %VERSION%.%REVISION% -OutputDirectory package

:: Clean up
move %VERSION_INFO_CS%.bak %VERSION_INFO_CS%
move %VERSION_INFO_CPP%.bak %VERSION_INFO_CPP%
move %INTERNALS_INFO_CS%.bak %INTERNALS_INFO_CS%

rd /S /Q out

if errorlevel 1 pause else exit