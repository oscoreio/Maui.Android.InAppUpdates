using Android.App;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.Install.Model;
using Xamarin.Google.Android.Play.Core.Tasks;

// ReSharper disable once CheckNamespace
namespace Maui.Android.InAppUpdates.Internal;

/// <summary>
/// Whenever the user brings your app to the foreground,
/// check whether your app has an update waiting to be installed.
/// If your app has an update in the DOWNLOADED state, prompt the user to install the update.
/// Otherwise, the update data continues to occupy the user's device storage. <br/>
/// According: https://developer.android.com/guide/playcore/in-app-updates/kotlin-java#install-flexible
/// </summary>
/// <param name="appUpdateManager"></param>
/// <param name="activity"></param>
/// <param name="updateRequest"></param>
public class ResumeSuccessListener(
    IAppUpdateManager appUpdateManager,
    Activity activity,
    int updateRequest)
    : Java.Lang.Object, IOnSuccessListener
{
    public void OnSuccess(Java.Lang.Object result)
    {
        if (result is not AppUpdateInfo info)
        {
            return;
        }
        
        // https://developer.android.com/guide/playcore/in-app-updates/kotlin-java#install-flexible
        // If the update is downloaded but not installed,
        // notify the user to complete the update.
        if (info.InstallStatus() == InstallStatus.Downloaded)
        {
            Handler.Options.CompleteUpdateAction();
        }
        // https://developer.android.com/guide/playcore/in-app-updates/kotlin-java#immediate
        else if (info.UpdateAvailability() == UpdateAvailability.DeveloperTriggeredUpdateInProgress) {
            // If an in-app update is already running, resume the update.
            _ = appUpdateManager.StartUpdateFlowForResult(
                info,
                activity,
                AppUpdateOptions
                    .NewBuilder(AppUpdateType.Immediate)
                    .SetAllowAssetPackDeletion(Handler.Options.AllowAssetPackDeletion)
                    .Build(),
                updateRequest);
        }
    }
}