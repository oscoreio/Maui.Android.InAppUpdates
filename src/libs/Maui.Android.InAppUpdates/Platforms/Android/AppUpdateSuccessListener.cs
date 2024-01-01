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

        System.Diagnostics.Debug.WriteLine($"AVAILABLE VERSION CODE {info.AvailableVersionCode()}");

        switch (info.UpdateAvailability())
        {
            case UpdateAvailability.UpdateAvailable or UpdateAvailability.DeveloperTriggeredUpdateInProgress when
                info.UpdatePriority() > 4 &&
                info.IsUpdateTypeAllowed(AppUpdateType.Immediate):
            {
                _ = appUpdateManager.StartUpdateFlowForResult(
                    info,
                    AppUpdateType.Immediate,
                    activity,
                    updateRequest);
                
                if (appUpdateManager is FakeAppUpdateManager { IsImmediateFlowVisible: true } fakeAppUpdate)
                {
                    fakeAppUpdate.UserAcceptsUpdate();
                    fakeAppUpdate.DownloadStarts();
                    fakeAppUpdate.DownloadCompletes();
                    
                    DefaultUserInterface.ShowAlertToCompleteUpdate(activity, appUpdateManager);
                }
                break;
            }

            case UpdateAvailability.UpdateAvailable or UpdateAvailability.DeveloperTriggeredUpdateInProgress when
                info.IsUpdateTypeAllowed(AppUpdateType.Flexible):
            {
                appUpdateManager.RegisterListener(new InstallStateUpdatedListener(
                    context: activity,
                    appUpdateManager: appUpdateManager));
                
                _ = appUpdateManager.StartUpdateFlowForResult(
                    info,
                    AppUpdateType.Flexible,
                    activity,
                    updateRequest);

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
                // You can start your activityonresult method when update is not available
                // when using immediate update
                activity.StartActivityForResult(
                    intent,
                    requestCode: 400); // You can use any random result code
                break;
        }
    }
}