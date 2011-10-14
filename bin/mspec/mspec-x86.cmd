@echo off
set PACKAGE_DIR=lib\packages
set MSPEC_EXE=mspec-x86-clr4.exe
set ARGS=%*

for /F %%M in ('dir /b /s %MSPEC_EXE%') do (
  %%M %ARGS%
  goto exit
)

:exit

if errorlevel 1 pause else exit