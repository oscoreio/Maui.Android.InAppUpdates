using Android.Widget;
using Google.Android.Material.Snackbar;

// ReSharper disable once CheckNamespace
namespace Maui.Android.InAppUpdates.Internal;

public static class DefaultUserInterface
{
    /// <summary>
    /// Displays a short duration toast message at the center of the screen.
    /// </summary>
    /// <param name="text">The text to be displayed in the toast message.</param>
    public static void ShowShortToast(
        string text)
    {
        Toast.MakeText(
            context: Platform.AppContext,
            text: text,
            duration: ToastLength.Short)?.Show();
    }

    /// <summary>
    /// Displays a snackbar with an action to complete the app update process.
    /// </summary>
    /// <param name="text">The text to display on the snackbar.</param>
    /// <param name="actionText">The text for the action button.</param>
    /// <param name="clickHandler">The handler for the action button.</param>
    public static void ShowSnackbar(
        string text,
        string actionText,
        Action<global::Android.Views.View> clickHandler)
    {
        if (Platform.CurrentActivity?.Window?.DecorView is not {} view ||
            Snackbar.Make(
                text: text,
                duration: BaseTransientBottomBar.LengthIndefinite,
                view: view) is not {} snackbar)
        {
            return;
        }
        
        snackbar.SetAction(
            text: actionText,
            clickHandler: clickHandler);
        snackbar.Show();
    }
}