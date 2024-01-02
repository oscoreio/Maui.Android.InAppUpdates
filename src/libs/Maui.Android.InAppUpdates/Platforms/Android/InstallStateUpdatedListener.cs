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
            var installStatus = state.InstallStatus();
            switch (installStatus)
            {
                case InstallStatus.Unknown:
                case InstallStatus.Pending:
                case InstallStatus.Installing:
                case InstallStatus.Installed:
                case InstallStatus.Canceled:
                    break;
                
                case InstallStatus.Downloading
                    when Handler.Options.ShowDownload:
                {
                    var bytesDownloaded = state.BytesDownloaded();
                    var totalBytesToDownload = state.TotalBytesToDownload() + 1;
                    var percents = Math.Round(
                        100.0 * bytesDownloaded / totalBytesToDownload);
                    
                    Handler.Options.DownloadingAction(context, percents);
                    break;
                }
            
                case InstallStatus.Downloaded:
                    Handler.Options.CompleteUpdateAction(context, appUpdateManager);
                    break;
                
                case InstallStatus.Failed:
                    Handler.Options.DownloadFailedAction(context);
                    break;
            }
        }
        catch (Exception e)
        {
            Handler.Options.DebugAction($"Error occurred during in app update status change: {e}");
        }
    }
}