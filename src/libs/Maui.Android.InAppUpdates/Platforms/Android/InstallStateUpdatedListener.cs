using Android.App;
using Android.Content;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.Install;
using Xamarin.Google.Android.Play.Core.Install.Model;
using Object = Java.Lang.Object;
using Toast = Android.Widget.Toast;
using ToastLength = Android.Widget.ToastLength;

namespace Maui.Android.InAppUpdates.Internal;

/// <summary>
/// Listener to track request state updates.
/// </summary>
public class InstallStateUpdatedListener(
    Context context,
    IAppUpdateManager appUpdateManager)
    : Object, IInstallStateUpdatedListener
{
    /// <summary>
    /// This method will be triggered when the app update status is changed.
    /// </summary>
    /// <param name="state"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void OnStateUpdate(InstallState state)
    {
        state = state ?? throw new ArgumentNullException(nameof(state));
        
        try
        {
            switch (state.InstallStatus())
            {
                case InstallStatus.Downloading:
                {
                    var bytesDownloaded = state.BytesDownloaded();
                    var totalBytesToDownload = state.TotalBytesToDownload();
            
                    var completed = Math.Round((double) bytesDownloaded / totalBytesToDownload * 100);
                    Toast.MakeText(context, $"Downloaded {completed}%", ToastLength.Short)?.Show();
                    break;
                }
            
                case InstallStatus.Downloaded:
                    ShowAlertToCompleteUpdate(context, appUpdateManager);
                    break;
                
                case InstallStatus.Failed:
                    Toast.MakeText(context, "Update download failed.", ToastLength.Short)?.Show();
                    break;
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Error occurred during in app update status change: {e}");
        }
    }
    
    /// <summary>
    /// This method will show the alert dialog to complete the update.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="appUpdateManager"></param>
    public static void ShowAlertToCompleteUpdate(
        Context context,
        IAppUpdateManager appUpdateManager)
    {
        var alert = new AlertDialog.Builder(context).Create();
        alert?.SetTitle("Download completed");
        alert?.SetMessage("Update is ready to be installed.");
        alert?.SetButton((int) DialogButtonType.Positive, "Perform update", (_, _) =>
        {
            appUpdateManager.CompleteUpdate();
        });
        alert?.SetCancelable(false);
        alert?.Show();
    }
}