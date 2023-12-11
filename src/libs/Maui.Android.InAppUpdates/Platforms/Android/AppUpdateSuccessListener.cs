using Android.App;
using Android.Content;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.AppUpdate.Testing;
using Xamarin.Google.Android.Play.Core.Install.Model;
using Xamarin.Google.Android.Play.Core.Tasks;
using Activity = Android.App.Activity;

namespace Maui.Android.InAppUpdates.Internal;

public class AppUpdateSuccessListener(
    IAppUpdateManager appUpdateManager,
    Activity activity,
    int updateRequest,
    Intent? intent)
    : Java.Lang.Object, IOnSuccessListener
{
    public void OnSuccess(Java.Lang.Object p0)
    {
        if (p0 is not AppUpdateInfo info)
        {
            return;
        }

        System.Diagnostics.Debug.WriteLine("AVAILABLE VERSION CODE {VersionCode}", $"{info.AvailableVersionCode()}");

        switch (info.UpdateAvailability())
        {
            case UpdateAvailability.UpdateAvailable or UpdateAvailability.DeveloperTriggeredUpdateInProgress when
                info.UpdatePriority() > 4 &&
                info.IsUpdateTypeAllowed(AppUpdateType.Immediate):
            {
                // Start an update
                _ = appUpdateManager.StartUpdateFlowForResult(
                    info, activity, AppUpdateOptions.DefaultOptions(AppUpdateType.Immediate), updateRequest);

                if (appUpdateManager is FakeAppUpdateManager { IsImmediateFlowVisible: true } fakeAppUpdate)
                {
                    fakeAppUpdate.UserAcceptsUpdate();
                    fakeAppUpdate.DownloadStarts();
                    fakeAppUpdate.DownloadCompletes();
                    
                    var dialog = new AlertDialog.Builder(activity);
                    var alert = dialog.Create();
                    alert?.SetMessage("Application successfully updated! You need to restart the app in order to use this new features");
                    alert?.SetCancelable(false);
                    alert?.SetButton((int)DialogButtonType.Positive, "Restart", (_, _) =>
                    {
                        appUpdateManager.CompleteUpdate();
                        // You can start your activityonresult method when update is not available when using immediate update when testing with fakeappupdate manager
                        //activity.StartActivityForResult(intent, 400);
                    });
                    alert?.Show();
                }
                break;
            }

            case UpdateAvailability.UpdateAvailable or UpdateAvailability.DeveloperTriggeredUpdateInProgress when
                info.IsUpdateTypeAllowed(AppUpdateType.Flexible):
            {
                appUpdateManager.RegisterListener(new InstallStateUpdatedListener(activity, appUpdateManager));
                // Start an update
                //_ = appUpdateManager.StartUpdateFlowForResult(
                //    info, activity, AppUpdateOptions.DefaultOptions(AppUpdateType.Flexible), updateRequest);
                _ = appUpdateManager.StartUpdateFlowForResult(info, AppUpdateType.Flexible, activity, updateRequest);

                if (appUpdateManager is FakeAppUpdateManager fakeAppUpdate)
                {
                    fakeAppUpdate.UserAcceptsUpdate();
                    fakeAppUpdate.DownloadStarts();
                    fakeAppUpdate.DownloadCompletes();
                }
                break;
            }
                
            case UpdateAvailability.UpdateNotAvailable:
            case UpdateAvailability.Unknown:
                System.Diagnostics.Debug.WriteLine("UPDATE NOT AVAILABLE {VersionCode}", $"{info.AvailableVersionCode()}");
                // You can start your activityonresult method when update is not available when using immediate update
                activity.StartActivityForResult(intent, 400); // You can use any random result code
                break;
        }
    }
}