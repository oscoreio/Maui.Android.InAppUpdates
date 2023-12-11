using Android.Content;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.Install.Model;
using Xamarin.Google.Android.Play.Core.Tasks;

namespace Maui.Android.InAppUpdates;

/// <summary>
/// Whenever the user brings your app to the foreground,
/// check whether your app has an update waiting to be installed.
/// If your app has an update in the DOWNLOADED state, prompt the user to install the update.
/// Otherwise, the update data continues to occupy the user's device storage. <br/>
/// According: https://developer.android.com/guide/playcore/in-app-updates/kotlin-java#install-flexible
/// </summary>
/// <param name="context"></param>
/// <param name="appUpdateManager"></param>
public class ResumeSuccessListener(
    Context context,
    IAppUpdateManager appUpdateManager)
    : Java.Lang.Object, IOnSuccessListener
{
    public void OnSuccess(Java.Lang.Object p0)
    {
        if (p0 is not AppUpdateInfo info)
        {
            return;
        }

        if (info.InstallStatus() == InstallStatus.Downloaded)
        {
            InstallStateUpdatedListener.ShowAlertToCompleteUpdate(context, appUpdateManager);
        }
    }
}