@echo off
set PACKAGE_DIR=lib\packages

if not exist %PACKAGE_DIR% call deps.cmd

%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild build.proj /target:Build /Property:Configuration=Debug
if errorlevel 1 pause else exit