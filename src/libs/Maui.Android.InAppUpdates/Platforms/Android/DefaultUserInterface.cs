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
    public static void ShowShortToast(string text)
    {
        try
        {
            if (Platform.AppContext is null)
            {
                Handler.Options.DebugAction($"Cannot show toast - Platform.AppContext is null: {text}");
                return;
            }

            Toast.MakeText(
                context: Platform.AppContext,
                text: text,
                duration: ToastLength.Short)?.Show();
        }
        catch (Exception ex)
        {
            Handler.Options.DebugAction($"Failed to show toast '{text}': {ex}");
        }
    }

    /// <summary>
    /// Displays a snackbar with an action to complete the app update process.
    /// If snackbar fails due to theme incompatibility, falls back to toast message.
    /// </summary>
    /// <param name="text">The text to display on the snackbar.</param>
    /// <param name="actionText">The text for the action button.</param>
    /// <param name="clickHandler">The handler for the action button. Can be null if no action is needed.</param>
    public static void ShowSnackbar(
        string text,
        string actionText,
        Action<global::Android.Views.View> clickHandler)
    {
        var fallbackMessage = $"{text} - {actionText}";

        try
        {
            var view = Platform.CurrentActivity?.Window?.DecorView;
            if (view is null)
            {
                FallbackToToastWithAction("Cannot show snackbar - no active view", fallbackMessage, clickHandler, null);
                return;
            }

            var snackbar = Snackbar.Make(view, text, BaseTransientBottomBar.LengthIndefinite);
            if (snackbar is null)
            {
                FallbackToToastWithAction("Cannot create snackbar", fallbackMessage, clickHandler, view);
                return;
            }

            snackbar.SetAction(text: actionText, clickHandler: clickHandler);
            snackbar.Show();
        }
        catch (Exception ex) when (IsThemeRelated(ex))
        {
            HandleSnackbarFailure(ex, "theme related", fallbackMessage, clickHandler, Platform.CurrentActivity?.Window?.DecorView);
        }
        catch (Exception ex)
        {
            HandleSnackbarFailure(ex, "general exception", fallbackMessage, null, null);
        }
    }

    private static void FallbackToToastWithAction(
        string debugMessage,
        string fallbackMessage,
        Action<global::Android.Views.View>? clickHandler,
        global::Android.Views.View? fallbackView)
    {
        Handler.Options.DebugAction($"{debugMessage}, falling back to toast and auto-triggering action");
        ShowShortToast(fallbackMessage);

        // Auto-trigger the action since user can't click the snackbar
        if (clickHandler is not null && fallbackView is not null)
        {
            try
            {
                clickHandler(fallbackView);
            }
            catch (Exception actionEx)
            {
                Handler.Options.DebugAction($"Error auto-executing snackbar action: {actionEx}");
            }
        }
    }

    private static void HandleSnackbarFailure(
        Exception ex,
        string errorType,
        string fallbackMessage,
        Action<global::Android.Views.View>? clickHandler,
        global::Android.Views.View? view)
    {
        Handler.Options.DebugAction($"Snackbar {errorType}: {ex}");
        ShowShortToast(fallbackMessage);

        // Only auto-trigger for theme-related exceptions where user interaction was expected
        if (clickHandler is not null && view is not null)
        {
            try
            {
                clickHandler(view);
            }
            catch (Exception actionEx)
            {
                Handler.Options.DebugAction($"Error executing snackbar action: {actionEx}");
            }
        }
    }

    private static bool IsThemeRelated(Exception ex) =>
        ex is global::Android.Views.InflateException ||
        (ex is Java.Lang.UnsupportedOperationException &&
         ex.Message?.Contains("Failed to resolve attribute", StringComparison.Ordinal) == true);
}