# MAUI.Native.Embedded Cheat Sheet
My personal cheat sheet for things to remember

## Build With Full Log
Make a build and write the full log to `publish.log`. Good for debugging MSBuild.
```
dotnet publish -f:net9.0-ios --verbosity diagnostic --tl:off MAUI.Native.Embedded.Sample/MAUI.Native.Embedded.Sample.csproj > publish.log
```
