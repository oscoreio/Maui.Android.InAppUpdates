using Android.App;
using Android.Gms.Tasks;
using Xamarin.Google.Android.Play.Core.AppUpdate;
using Xamarin.Google.Android.Play.Core.AppUpdate.Install.Model;

// ReSharper disable once CheckNamespace
namespace Maui.Android.InAppUpdates.Internal;

public class AppUpdateSuccessListener(
    IAppUpdateManager appUpdateManager,
    Activity activity,
    int updateRequest)
    : Java.Lang.Object, IOnSuccessListener
{
    public InstallStateUpdatedListener? InstallStateUpdatedListener { get; private set; }

    public void OnSuccess(Java.Lang.Object? result)
    {
        if (result is not AppUpdateInfo info)
        {
            return;
        }

        Handler.Options.DebugAction($"AVAILABLE VERSION CODE {info.AvailableVersionCode()}");

        var updateAvailability = info.UpdateAvailability();
        var updatePriority = info.UpdatePriority();
        var isImmediateUpdatesAllowed = info.IsUpdateTypeAllowed(AppUpdateType.Immediate);
        var isFlexibleUpdatesAllowed = info.IsUpdateTypeAllowed(AppUpdateType.Flexible);
        switch (updateAvailability)
        {
            case UpdateAvailability.UpdateAvailable or
                UpdateAvailability.DeveloperTriggeredUpdateInProgress
                when updatePriority >= Handler.Options.ImmediateUpdatePriority &&
                     isImmediateUpdatesAllowed:
            {
                _ = appUpdateManager.StartUpdateFlowForResult(
                    info,
                    activity,
                    AppUpdateOptions
                        .NewBuilder(AppUpdateType.Immediate)
                        .SetAllowAssetPackDeletion(Handler.Options.AllowAssetPackDeletion)
                        .Build(),
                    updateRequest);
                break;
            }

            case UpdateAvailability.UpdateAvailable or
                UpdateAvailability.DeveloperTriggeredUpdateInProgress
                when isFlexibleUpdatesAllowed:
            {
                InstallStateUpdatedListener ??= new InstallStateUpdatedListener();
                appUpdateManager.RegisterListener(InstallStateUpdatedListener);
            
                _ = appUpdateManager.StartUpdateFlowForResult(
                    info,
                    activity,
                    AppUpdateOptions
                        .NewBuilder(AppUpdateType.Flexible)
                        .SetAllowAssetPackDeletion(Handler.Options.AllowAssetPackDeletion)
                        .Build(),
                    updateRequest);
                break;
            }

            case UpdateAvailability.UpdateNotAvailable:
            case UpdateAvailability.Unknown:
                Handler.Options.DebugAction($"UPDATE NOT AVAILABLE {info.AvailableVersionCode()}");
                break;
        }
    }
}