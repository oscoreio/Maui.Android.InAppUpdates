using Android.App;
using Android.Content;
using Android.Widget;
using Xamarin.Google.Android.Play.Core.AppUpdate;

namespace Maui.Android.InAppUpdates.Internal;

public static class DefaultUserInterface
{
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
        // alert?.SetMessage("Application successfully updated! You need to restart the app in order to use this new features");
        // alert?.SetButton((int)DialogButtonType.Positive, "Restart",
        alert?.SetMessage("Update is ready to be installed.");
        alert?.SetButton((int) DialogButtonType.Positive, "Perform update", (_, _) =>
        {
            appUpdateManager.CompleteUpdate();
            // You can start your activityonresult method when update is not available when using immediate update when testing with fakeappupdate manager
            //activity.StartActivityForResult(intent, 400);
        });
        alert?.SetCancelable(false);
        alert?.Show();
    }

    public static void ShowShortToast(Context context, string text)
    {
        Toast.MakeText(
            context: context,
            text: text,
            duration: ToastLength.Short)?.Show();
    }
}