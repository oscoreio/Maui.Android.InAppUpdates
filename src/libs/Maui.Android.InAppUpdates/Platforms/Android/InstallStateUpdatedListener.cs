using Xamarin.Google.Android.Play.Core.AppUpdate.Install.Model;
using Xamarin.Google.Android.Play.Core.AppUpdate.Install;

// ReSharper disable once CheckNamespace
namespace Maui.Android.InAppUpdates.Internal;

/// <summary>
/// Listener to track request state updates.
/// </summary>
public class InstallStateUpdatedListener
    : Java.Lang.Object, IInstallStateUpdatedListener
{
    /// <summary>
    /// This method will be triggered when the app update status is changed.
    /// </summary>
    /// <param name="state"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void OnStateUpdate(InstallState? state)
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
                    
                    Handler.Options.DownloadingAction(percents);
                    break;
                }
            
                case InstallStatus.Downloaded:
                    Handler.Options.CompleteUpdateAction();
                    break;
                
                case InstallStatus.Failed:
                    Handler.Options.DownloadFailedAction();
                    break;
            }
        }
        catch (Exception e)
        {
            Handler.Options.DebugAction($"Error occurred during in app update status change: {e}");
        }
    }
}
