using Android.App;
using Android.Content;
using Android.Runtime;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.AppUpdate.Testing;
using Xamarin.Google.Android.Play.Core.Install.Model;
using Bundle = Android.OS.Bundle;

namespace Maui.Android.InAppUpdates.Internal;

/// <summary>
/// This class is responsible for handling the update events.
/// </summary>
public static class Handler
{
    public const int RequestUpdate = 4711;
    
    /// <summary>
    /// The app update manager.
    /// </summary>
    public static IAppUpdateManager? AppUpdateManager { get; private set; }
    
    public static AppUpdateSuccessListener? AppUpdateSuccessListener { get; private set; }
    public static ResumeSuccessListener? ResumeSuccessListener { get; private set; }
    
    /// <summary>
    /// Options for the in-app updates.
    /// </summary>
    public static AndroidInAppUpdatesOptions Options { get; set; } = new();

    /// <summary>
    /// This method will be triggered when the app is created.
    /// </summary>
    /// <param name="activity"></param>
    /// <param name="savedInstanceState"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void HandleCreate(Activity activity, Bundle? savedInstanceState)
    {
        activity = activity ?? throw new ArgumentNullException(nameof(activity));

        AppUpdateManager = Options.UseFakeAppUpdateManager
            ? new FakeAppUpdateManager(activity)
            : AppUpdateManagerFactory.Create(activity);
        AppUpdateSuccessListener ??= new AppUpdateSuccessListener(
            appUpdateManager: AppUpdateManager,
            activity: activity,
            updateRequest: RequestUpdate,
            intent: activity.Intent);
        AppUpdateManager.AppUpdateInfo.AddOnSuccessListener(AppUpdateSuccessListener);
    }
    
    /// <summary>
    /// This method will be triggered when the app is resumed.
    /// </summary>
    /// <param name="activity"></param>
    public static void HandleResume(Activity activity)
    {
        if (AppUpdateManager is null)
        {
            return;
        }
        
        ResumeSuccessListener ??= new ResumeSuccessListener(
            appUpdateManager: AppUpdateManager,
            activity: activity,
            updateRequest: RequestUpdate);
        AppUpdateManager.AppUpdateInfo.AddOnSuccessListener(ResumeSuccessListener);
    }
    
    /// <summary>
    /// This method will be triggered when the activity result is returned.
    /// </summary>
    /// <param name="activity"></param>
    /// <param name="requestCode"></param>
    /// <param name="resultCode"></param>
    /// <param name="data"></param>
    public static void HandleActivityResult(
        Activity activity,
        int requestCode,
        [GeneratedEnum] Result resultCode,
        Intent? data)
    {
        if (requestCode != RequestUpdate)
        {
            return;
        }
        
        // The switch block will be triggered only with flexible update since it returns the install result codes
        switch (resultCode)
        {
            case Result.Ok:
                Options.AppUpdatedAction(activity);
                break;
            
            case Result.Canceled:
                Options.UpdateCancelledAction(activity);
                break;
            
            case (Result)ActivityResult.ResultInAppUpdateFailed:
                Options.UpdateFailedAction(activity);
                break;
        }
    }
}