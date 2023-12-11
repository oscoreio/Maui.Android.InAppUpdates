using Microsoft.Maui.LifecycleEvents;

namespace Maui.Android.InAppUpdates;

/// <summary>
/// This class contains the extension method to enable the in-app updates for android.
/// </summary>
public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// This method will enable the in-app updates for android.
    /// Set debugMode to true to enable the fake app update manager.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="debugMode"></param>
    /// <returns></returns>
    public static MauiAppBuilder UseAndroidInAppUpdates(
        this MauiAppBuilder builder,
        bool debugMode = false)
    {
#if ANDROID
        Internal.UpdateEvents.DebugMode = debugMode;
#endif
        
        return builder
            .ConfigureLifecycleEvents(static events =>
            {
#if ANDROID
                events.AddAndroid(static android =>
                {
                    android
                        .OnActivityResult(Internal.UpdateEvents.HandleActivityResult)
                        .OnCreate(Internal.UpdateEvents.HandleCreate)
                        .OnResume(Internal.UpdateEvents.HandleResume)
                        ;
                });
#endif
            });
    }
}
