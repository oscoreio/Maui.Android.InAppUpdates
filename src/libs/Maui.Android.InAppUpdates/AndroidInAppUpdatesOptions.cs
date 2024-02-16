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
    
    /// <summary>
    /// This action will be triggered when the app is updated. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action AppUpdatedAction { get; set; } = static () =>
#if ANDROID
        Internal.DefaultUserInterface.ShowShortToast(
            text: "App updated");
#else
        {};
#endif
    
    /// <summary>
    /// This action will be triggered when the update is cancelled. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action UpdateCancelledAction { get; set; } = static () =>
#if ANDROID
        Internal.DefaultUserInterface.ShowShortToast(
            text: "In app update cancelled");
#else
        {};
#endif
    
    /// <summary>
    /// This action will be triggered when the update is failed. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action UpdateFailedAction { get; set; } = static () =>
#if ANDROID
        Internal.DefaultUserInterface.ShowShortToast(
            text: "In app update failed");
#else
        {};
#endif
    
    /// <summary>
    /// This action will be triggered when the download is failed. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action DownloadFailedAction { get; set; } = static () =>
#if ANDROID
        Internal.DefaultUserInterface.ShowShortToast(
            text: "Update download failed.");
#else
        {};
#endif
    
    /// <summary>
    /// This action will be triggered when downloading the update. <br/>
    /// Second value is the percentage of the download. <br/>
    /// Default action will show the toast with english text. <br/>
    /// </summary>
    public Action<double> DownloadingAction { get; set; } = static percents =>
#if ANDROID
        Internal.DefaultUserInterface.ShowShortToast(
            text: $"Downloaded {percents}%");
#else
        {};
#endif
    
    /// <summary>
    /// This action will be triggered when the update is completed. <br/>
    /// Default action will show the alert dialog to complete the update. <br/>
    /// </summary>
    public Action CompleteUpdateAction { get; set; } = static () =>
#if ANDROID
        Internal.DefaultUserInterface.ShowSnackbar(
            text: "An update has just been downloaded.",
            actionText: "RESTART",
            clickHandler: static _ => Internal.Handler.AppUpdateManager?.CompleteUpdate());
#else
        {};
#endif
    
    /// <summary>
    /// This action will be triggered when debug event occurs. <br/>
    /// Default action will write the text to the debug output. <br/>
    /// </summary>
    public Action<string> DebugAction { get; set; } = static text =>
        System.Diagnostics.Debug.WriteLine(text);
}