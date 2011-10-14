@echo off
set PACKAGE_DIR=lib\packages
set DEPS_CMD=deps.cmd

if not exist %PACKAGE_DIR% call %DEPS_CMD%

%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild build.proj /target:Package /Property:Configuration=Release
if errorlevel 1 pause else exit