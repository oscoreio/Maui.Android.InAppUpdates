#if ANDROID
using Android.Content;
using Xamarin.Google.Android.Play.Core.AppUpdate;
#endif

namespace Maui.Android.InAppUpdates;

/// <summary>
/// Represents options for in-app updates on Android.
/// </summary>
public class AndroidInAppUpdatesOptions
{
    /// <summary>
    /// Show the download progress. <br/>
    /// Default is true. <br/>
    /// </summary>
    public bool ShowDownload { get; set; } = true;
    
    /// <summary>
    /// Sets the priority of the update for immediate updates. <br/>
    /// Default is 5. <br/>
    /// </summary>
    public int ImmediateUpdatePriority { get; set; } = 5;
    
    /// <summary>
    /// Set this to true to use the fake app update manager. <br/>
    /// </summary>
    public bool UseFakeAppUpdateManager { get; set; }
    
#if ANDROID
    /// <summary>
    /// This action will be triggered when the app is updated. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action<Context> AppUpdatedAction { get; set; } = static context =>
        Internal.DefaultUserInterface.ShowShortToast(
            context: context,
            text: "App updated");
    
    /// <summary>
    /// This action will be triggered when the update is cancelled. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action<Context> UpdateCancelledAction { get; set; } = static context =>
        Internal.DefaultUserInterface.ShowShortToast(
            context: context,
            text: "In app update cancelled");
    
    /// <summary>
    /// This action will be triggered when the update is failed. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action<Context> UpdateFailedAction { get; set; } = static context =>
        Internal.DefaultUserInterface.ShowShortToast(
            context: context,
            text: "In app update failed");
    
    /// <summary>
    /// This action will be triggered when the download is failed. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action<Context> DownloadFailedAction { get; set; } = static context =>
        Internal.DefaultUserInterface.ShowShortToast(
            context: context,
            text: "Update download failed.");
    
    /// <summary>
    /// This action will be triggered when downloading the update. <br/>
    /// Second value is the percentage of the download. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action<Context, double> DownloadingAction { get; set; } = static (context, percents) =>
        Internal.DefaultUserInterface.ShowShortToast(
            context: context,
            text: $"Downloaded {percents}%");
    
    /// <summary>
    /// This action will be triggered when the update is completed. <br/>
    /// Default action will show the alert dialog to complete the update. <br/>
    /// </summary>
    public Action<Context, IAppUpdateManager> CompleteUpdateAction { get; set; } = static (context, appUpdateManager) =>
        Internal.DefaultUserInterface.ShowAlertToCompleteUpdate(
            context: context,
            appUpdateManager: appUpdateManager,
            title: "Download completed",
            message: "Update is ready to be installed.", // "Application successfully updated! You need to restart the app in order to use this new features"
            positiveButton: "Perform update"); // "Restart"
#endif
    
    /// <summary>
    /// This action will be triggered when debug event occurs. <br/>
    /// Default action will write the text to the debug output. <br/>
    /// </summary>
    public Action<string> DebugAction { get; set; } = static text =>
        System.Diagnostics.Debug.WriteLine(text);
}