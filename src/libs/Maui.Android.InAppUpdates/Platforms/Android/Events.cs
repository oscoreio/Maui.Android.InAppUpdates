using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.AppUpdate.Testing;
using Xamarin.Google.Android.Play.Core.Install.Model;
using Toast = Android.Widget.Toast;
using Bundle = Android.OS.Bundle;

namespace Maui.Android.InAppUpdates;

public static class UpdateEvents
{
    private const int RequestUpdate = 4711;
    
    private static IAppUpdateManager? _appUpdateManager;

    public static void HandleCreate(Activity activity, Bundle? savedInstanceState)
    {
#if DEBUG
        var updateManager = new FakeAppUpdateManager(activity);
        /* The below line of code will trigger the fake app update manager which it will display the alert dialog
        Let say if we comment this line of code to simulate update is not available then the play core update not available flag
        will be captured on the appupdatesuccess listener.
        If comment this line it will simulate if the app update is not available. Then you can add logic when update is not available using immeidate update*/
        updateManager.SetUpdateAvailable(3); // your higher app version code that can be used to test fakeappupdate manager
        updateManager.SetUpdatePriority(4);
#else // The below line of code will execute in release configuration
        var updateManager = AppUpdateManagerFactory.Create(this);
#endif
        _appUpdateManager = updateManager;
        
        updateManager.AppUpdateInfo.AddOnSuccessListener(new AppUpdateSuccessListener(
            appUpdateManager: updateManager,
            activity: activity,
            updateRequest: RequestUpdate,
            intent: activity.Intent));
    }
    
    public static void HandleResume(Activity activity)
    {
        _appUpdateManager?.AppUpdateInfo.AddOnSuccessListener(new ResumeSuccessListener(
            context: activity,
            appUpdateManager: _appUpdateManager));
    }
    
    public static void HandleActivityResult(Activity activity, int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
    {
        if (requestCode == RequestUpdate)
        {
            switch (resultCode) // The switch block will be triggered only with flexible update since it returns the install result codes
            {
                case Result.Ok:
                    // In app update success
                    //if (AppUpdateTypeSupported == AppUpdateType.Immediate)
                    {
                        Toast.MakeText(activity, "App updated", ToastLength.Short)?.Show();
                    }

                    break;
                case Result.Canceled:
                    Toast.MakeText(activity, "In app update cancelled", ToastLength.Short)?.Show();
                    break;
                case (Result)ActivityResult.ResultInAppUpdateFailed:
                    Toast.MakeText(activity, "In app update failed", ToastLength.Short)?.Show();
                    break;
            }
        }
        else // Here we add our custom code since immediate update will not return a callback result code
        {
            var dialog = new AlertDialog.Builder(activity);
            var alert = dialog.Create();
            alert?.SetMessage("Hello Xamarin. Additional instructions");
            alert?.SetCancelable(false);
            alert?.SetButton((int)DialogButtonType.Positive, "Ok", (_, _) =>
            {
                alert.Dismiss();
            });
            alert?.Show();
        }
    }
}