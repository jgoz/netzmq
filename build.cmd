%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild build.proj /target:Build /Property:Configuration=Release
if errorlevel 1 pause else exit