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
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static MauiAppBuilder UseAndroidInAppUpdates(
        this MauiAppBuilder builder,
        Action<AndroidInAppUpdatesOptions>? setupAction = null) 
    {
        builder = builder ?? throw new ArgumentNullException(nameof(builder));
        
#if ANDROID
        setupAction?.Invoke(Internal.Handler.Options);
#endif
        
        return builder
            .ConfigureLifecycleEvents(static events =>
            {
#if ANDROID
                events.AddAndroid(static android =>
                {
                    android
                        .OnActivityResult(Internal.Handler.HandleActivityResult)
                        .OnCreate(Internal.Handler.HandleCreate)
                        .OnResume(Internal.Handler.HandleResume)
                        ;
                });
#endif
            });
    }
}
