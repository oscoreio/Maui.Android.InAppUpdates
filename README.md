# Maui.Android.InAppUpdates
An example of implementing Android In-App Updates within a MAUI application with debugging capabilities.  
Also available as a NuGet package if you don't need customization.

# Usage
- Add NuGet package to your project:
```
<PackageReference Include="Maui.Android.InAppUpdates" Version="1.0.0" />
```
- Add the following to your `MauiProgram.cs` `CreateMauiApp` method:
```csharp
builder
   .UseAndroidInAppUpdates()
```

# Links
- https://developer.android.com/guide/playcore/in-app-updates/kotlin-java
- https://github.com/PatGet/XamarinPlayCoreUpdater
- https://github.com/xamarin/GooglePlayServicesComponents/issues/796
- https://github.com/PatGet/XamarinPlayCoreUpdater/issues/22
- https://github.com/PatGet/XamarinPlayCoreUpdater/issues/17
- https://github.com/PatGet/XamarinPlayCoreUpdater/pull/20#issuecomment-1273774958
- https://stackoverflow.com/questions/56218160/how-to-implement-google-play-in-app-update-and-use-play-core-library-with-xamari