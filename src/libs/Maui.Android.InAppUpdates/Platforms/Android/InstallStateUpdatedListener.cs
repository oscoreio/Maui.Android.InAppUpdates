using Android.Content;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.Install;
using Xamarin.Google.Android.Play.Core.Install.Model;
using Object = Java.Lang.Object;

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
            
                    var completed = Math.Round(100.0 * bytesDownloaded / totalBytesToDownload);
                    DefaultUserInterface.ShowShortToast(context, $"Downloaded {completed}%");
                    break;
                }
            
                case InstallStatus.Downloaded:
                    DefaultUserInterface.ShowAlertToCompleteUpdate(context, appUpdateManager);
                    break;
                
                case InstallStatus.Failed:
                    DefaultUserInterface.ShowShortToast(context, "Update download failed.");
                    break;
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Error occurred during in app update status change: {e}");
        }
    }
}