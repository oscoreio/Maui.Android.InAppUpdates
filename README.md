# Maui.Android.InAppUpdates

[![Nuget package](https://img.shields.io/nuget/vpre/Oscore.Maui.Android.InAppUpdates)](https://www.nuget.org/packages/Oscore.Maui.Android.InAppUpdates/)
[![CI/CD](https://github.com/oscoreio/Maui.Android.InAppUpdates/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/oscoreio/Maui.Android.InAppUpdates/actions/workflows/dotnet.yml)
[![License: MIT](https://img.shields.io/github/license/oscoreio/Maui.Android.InAppUpdates)](https://github.com/oscoreio/Maui.Android.InAppUpdates/blob/main/LICENSE)

NuGet package that implementing Android In-App Updates for MAUI with debugging capabilities.
![Flexible](https://developer.android.com/static/images/app-bundle/flexible_flow.png)

# Usage
- Add NuGet package to your project:
```xml
<PackageReference Include="Oscore.Maui.Android.InAppUpdates" Version="1.0.0" />
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
It will display a window when starting an application or resume according to the official guides.  
You cannot see the popup dialog while developing or if you distribute it manually. 
As you can [see here](https://developer.android.com/guide/playcore/in-app-review/test), 
you have to download the app from the Play Store to see the popup. 
I recommend using Android Play Store's [“Internal App Sharing”](https://play.google.com/console/about/internalappsharing/) feature to test.  
When flexible updates were available in version 1.0.5, the default behavior was:
- If priority 1-3 is specified, flexible update will be offered
- If priority 4-5 is specified, immediate update will be offered
  
Currently for version 1.1.0 only immediate update is offered, regardless of priority, but there are plans to return the old behavior when it becomes possible

# Notes
- Right now the package uses the `Xamarin.Google.Android.Play.App.Update` package, but it's not possible to do flexible updates with it.

# Links
- https://developer.android.com/guide/playcore/in-app-updates/kotlin-java
- https://github.com/PatGet/XamarinPlayCoreUpdater
- https://github.com/xamarin/GooglePlayServicesComponents/issues/796
- https://github.com/PatGet/XamarinPlayCoreUpdater/issues/22
- https://github.com/PatGet/XamarinPlayCoreUpdater/issues/17
- https://github.com/PatGet/XamarinPlayCoreUpdater/pull/20#issuecomment-1273774958
- https://stackoverflow.com/questions/56218160/how-to-implement-google-play-in-app-update-and-use-play-core-library-with-xamari