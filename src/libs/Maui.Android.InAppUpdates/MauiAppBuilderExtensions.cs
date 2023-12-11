using Microsoft.Maui.LifecycleEvents;

namespace Maui.Android.InAppUpdates;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseAndroidInAppUpdates(this MauiAppBuilder builder)
    {
        return builder
            .ConfigureLifecycleEvents(static events =>
            {
#if ANDROID
                events.AddAndroid(static android =>
                {
                    android
                        .OnActivityResult(UpdateEvents.HandleActivityResult)
                        .OnCreate(UpdateEvents.HandleCreate)
                        .OnResume(UpdateEvents.HandleResume)
                        ;
                });
#endif
            });
    }
}
