# MAUI.Native.Embedded Cheat Sheet
My personal cheat sheet for things to remember

## Build With Full Log
Make a build and write the full log to `publish.log`. Good for debugging MSBuild.
```
dotnet publish -f:net9.0-ios --verbosity diagnostic --tl:off MAUI.Native.Embedded.Sample/MAUI.Native.Embedded.Sample.csproj > publish.log
```

## Build For iOS Device
In order to build for device (and not simulator) you must provide the `RuntimeIdentifier`.
This makes a debug build for iOS device.
```
dotnet build -f:net9.0-ios -p:RuntimeIdentifier=ios-arm64 MAUI.Native.Embedded.Sample/MAUI.Native.Embedded.Sample.csproj
```

## See Xcode project settings
```
xcodebuild -showBuildSettings -scheme MAUI.Native.Embedded.ios -project MAUI.Native.Embedded.Sample/Platforms/iOS/MAUI.Native.Embedded.ios/MAUI.Native.Embedded.ios.xcodeproj
```
