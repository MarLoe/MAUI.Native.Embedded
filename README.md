# MAUI.Native.Embedded

[![NuGet version (MarLoe.MAUI.Native.Embedded)](https://img.shields.io/nuget/v/MarLoe.MAUI.Native.Embedded.svg?style=flat-square)](https://www.nuget.org/packages/MarLoe.MAUI.Native.Embedded/)

Embed native iOS AppClips into you MAUI app

MAUI currently does not support AppClips for iOS. This NuGet will allow you to embed a natively build AppClips into you MAUI app.

This NuGet will build you native added projects for you. You do not need to do anything but adding it.

## iOS AppClips
In you main MAUI project (.csproj) file you must add the following line(s);
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" />
</ItemGroup>
```

The `Native.xcodeproj` can be named anything and placed anywhere as long as it can be referenced from you MAUI project. But I suggest putting it under `Platforms/iOS`.

### Scheme (optional)
Normally in Xcode projects, the main scheme is named the same as the Xcode project (.xcodeproj) file. Should this not be the case, it is possible to specify the exact scheme to use.

When building your embedded Xcode project the scheme used will be the same name as the Xcode project (.xcodeproj) file. In the example above, the scheme used if not specified will be `Native`

You can override schme using `Scheme`:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" Scheme="AnotherNative" />
</ItemGroup>
```

### Configuration (optional)
Normally in Xcode projects, the configurations are called `Debug` and `Release`. This usually corrosponds to the configurations used in .NET and MAUI projects.

When building your embedded Xcode project the configuration names from your MAUI project will be used.

You can override this by using `Configuraion`:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" Configuration="AnotherConfiguration" />
</ItemGroup>
```
If the configuration names does not match, you e.g. do this
```
<ItemGroup>
  <AppClips Condition="'$(Configuration)' == 'Debug'" Include="./Platforms/iOS/Native/Native.xcodeproj" Configuration="NativeDebug" />
  <AppClips Condition="'$(Configuration)' == 'Release'" Include="./Platforms/iOS/Native/Native.xcodeproj" Configuration="NativeRelease" />
</ItemGroup>
```

### SkipValidation (optional)
For AppClips the application identifier and the com.apple.developer.parent-application-identifiers in the AppClips entitlement must match the `ApplicationId` of the MAUI app.

After the MAUI app has been build, it is verified that there is a match. If not the build will fail with an error 1013 ( I had to chose a number ;).
The validation process relies on a couple of command line tools to be present on the build system. If not, the validation and thus the build will fail.

You can skip validation by providing `SkipValidation`:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" SkipValidation="true" />
</ItemGroup>
```

### NoSigning (optional)
Especially for CI builds, it can be difficult to get provisionings working.

You can skip signing by providing `NoSigning`:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" NoSigning="true" />
</ItemGroup>
```

## Hints
Here are a few hints that might make your life easier.

### Hint: Only add for release builds
When e.g. adding AppClips, you might want to only build and embed the native project when making release builds. Do this by adding a condtion:
```
<ItemGroup>
  <AppClips Condition="'$(Configuration)' == 'Release'" Include="./Platforms/iOS/Native/Native.xcodeproj" />
</ItemGroup>
```

## TLDR;
Support AppClipp in your MAUI project by adding this to your project (.csproj) file:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" />
</ItemGroup>
```
