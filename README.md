# Maui.Android.InAppUpdates

[![Nuget package](https://img.shields.io/nuget/vpre/Oscore.Maui.Android.InAppUpdates)](https://www.nuget.org/packages/Oscore.Maui.Android.InAppUpdates/)
[![CI/CD](https://github.com/oscoreio/Maui.Android.InAppUpdates/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/oscoreio/Maui.Android.InAppUpdates/actions/workflows/dotnet.yml)
[![License: MIT](https://img.shields.io/github/license/oscoreio/Maui.Android.InAppUpdates)](https://github.com/oscoreio/Maui.Android.InAppUpdates/blob/main/LICENSE)

NuGet package that implementing Android In-App Updates for MAUI with debugging capabilities.
![Flexible](https://developer.android.com/static/images/app-bundle/flexible_flow.png)

# Usage
- Add NuGet package to your project:
```xml
<PackageReference Include="Oscore.Maui.Android.InAppUpdates" Version="0.9.1" />
```
- Add the following to your `MauiProgram.cs` `CreateMauiApp` method:
```diff
builder
    .UseMauiApp<App>()
+   .UseAndroidInAppUpdates()
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    });
```

# Notes
- Right now the package uses the `Xamarin.Google.Android.Play.Core` dependency for implementation. There is a plan to move to `Xamarin.Google.Android.Play.App.Update` in the future [when the problem with flexible updates is resolved](https://github.com/PatGet/XamarinPlayCoreUpdater/issues/17)

# Links
- https://developer.android.com/guide/playcore/in-app-updates/kotlin-java
- https://github.com/PatGet/XamarinPlayCoreUpdater
- https://github.com/xamarin/GooglePlayServicesComponents/issues/796
- https://github.com/PatGet/XamarinPlayCoreUpdater/issues/22
- https://github.com/PatGet/XamarinPlayCoreUpdater/issues/17
- https://github.com/PatGet/XamarinPlayCoreUpdater/pull/20#issuecomment-1273774958
- https://stackoverflow.com/questions/56218160/how-to-implement-google-play-in-app-update-and-use-play-core-library-with-xamari