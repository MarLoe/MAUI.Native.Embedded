# MAUI.Native
Embed native iOS AppClips into you MAUI app

MAUI currently does not support AppClips for iOS. This NuGet will allow you to embed a natively build AppClips into you MAUI app.

## iOS AppClips
In you main MAUI project (.csproj) file you must add the following line(s);

```
<ItemGroup>
  <AppClips Include="./Platforms/iOS/Native/Native.xcodeproj">
</ItemGroup>
```

The `Native.xcodeproj` can be named anything and placed anywhere as long as it can be referenced from you MAUI project file. But I suggest putting it under `Platforms/iOS`.
