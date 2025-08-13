# MAUI.Native.Embedded

[![NuGet version (MarLoe.MAUI.Native.Embedded)](https://img.shields.io/nuget/v/MarLoe.MAUI.Native.Embedded.svg?style=flat-square)](https://www.nuget.org/packages/MarLoe.MAUI.Native.Embedded/)

Embed native iOS AppClips and Widgets into you MAUI app

MAUI currently does not support AppClips for iOS. This NuGet will allow you to embed a natively build AppClips into you MAUI app.

This NuGet will build you native added projects for you. You do not need to do anything but adding it.

## iOS AppClips & Widgets
In you main MAUI project (`.csproj`) file you can add one or more of the following line(s);
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" />
  <Widgets Include="./Platforms/iOS/Native/Native.xcodeproj" />
</ItemGroup>
```

The `Native.xcodeproj` can be named anything and placed anywhere as long as it can be referenced from you MAUI project. *But I suggest putting it under `Platforms/iOS`*.

### Scheme (optional)
Normally in Xcode projects, the main scheme is named the same as the Xcode project file (`.xcodeproj`) file. Should this not be the case, it is possible to specify the exact scheme to use.

When building your embedded Xcode project the scheme used will be the same name as the Xcode project (`.xcodeproj`) filename. In the example above, the scheme used if not specified will be `Native`

You can override scheme like this:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" Scheme="AnotherScheme" />
  <Widgets Include="./Platforms/iOS/Native/Native.xcodeproj" Scheme="ThirdScheme" />
</ItemGroup>
```

### Configuration (optional)
Normally in Xcode projects, the configurations are called `Debug` and `Release`. This usually corrosponds to the configurations used in .NET and MAUI projects.

When building your embedded Xcode project the configuration names from your MAUI project will be used.

You can override configuration like this:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" Configuration="AnotherConfiguration" />
  <Widgets Include="./Platforms/iOS/Native/Native.xcodeproj" Configuration="AnotherConfiguration" />
</ItemGroup>
```

If the configuration names does not match, you can e.g. do this (works for both `<AppClips>` and `<Widgets>`)
```
<ItemGroup>
  <AppClips 
    Condition="'$(Configuration)' == 'Debug'"
    Include="./Platforms/iOS/Native/Native.xcodeproj"
    Configuration="NativeDebug" />

  <AppClips 
    Condition="'$(Configuration)' == 'Release'"
    Include="./Platforms/iOS/Native/Native.xcodeproj"
    Configuration="NativeRelease" />
</ItemGroup>
```

### SkipValidation (optional)
For AppClips the application identifier and the com.apple.developer.parent-application-identifiers in the AppClips entitlement must match the `ApplicationId` of the MAUI app.

After the MAUI app has been build, it is verified that there is a match. If not the build will fail with an error 1013 (I had to chose a number ;).
The validation process relies on a couple of command line tools to be present on the build system. If not, the validation and thus the build will fail.

You can skip validation like this:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" SkipValidation="true" />
</ItemGroup>
```

### NoSigning (optional)
Especially for CI builds, it can be difficult to get provisionings working. In order to mitigate this, you can skip signing of the xcode project. I also am skipping signing in the sample app in order for it to build straight out of the box.

You can skip signing like this:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" NoSigning="true" />
  <Widgets Include="./Platforms/iOS/Native/Native.xcodeproj" NoSigning="true" />
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

### Hint: Custom Xcode project version
If your Xcode project uses a custom way of storing versions, you can make an custom update here.

All you need to do is to add this target to your project (.csproj)
```
<Target Name="CustomXcodeUpdateVersion" AfterTargets="_UpdateXcodeProjectVersion">
  <!-- Do your custom version update here -->
</Target>
```

## TLDR;
Support AppClip or Widget for iOS in your MAUI project by adding this to your project (.csproj) file:
```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj" />
  <Widgets Include="./Platforms/iOS/Native/Native.xcodeproj" />
</ItemGroup>
```
