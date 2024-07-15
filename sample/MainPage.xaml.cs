using CommunityToolkit.Mvvm.Input;
#if ANDROID
using Xamarin.Google.Android.Play.Core.AppUpdate.Testing;
#endif

namespace Maui.Android.InAppUpdates;

public partial class MainPage : ContentPage
{
#if ANDROID
	private int _availableVersionCode = 2;
#endif
	
	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	[RelayCommand]
	private void SetUpdateAvailableWithPriorityOf5()
	{
#if ANDROID
		FakeAppUpdateManager.SetUpdatePriority(updatePriority: 5);
		FakeAppUpdateManager.SetUpdateAvailable(availableVersionCode: _availableVersionCode++);
		AddOnSuccessListener();
#endif
	}

	[RelayCommand]
	private void SetUpdateAvailableWithPriorityOf3()
	{
#if ANDROID
		FakeAppUpdateManager.SetUpdatePriority(updatePriority: 3);
		FakeAppUpdateManager.SetUpdateAvailable(availableVersionCode: _availableVersionCode++);
		AddOnSuccessListener();
#endif
	}
	
	[RelayCommand]
	private void UserAcceptsUpdate()
	{
#if ANDROID
		FakeAppUpdateManager.UserAcceptsUpdate();
#endif
	}
	
	[RelayCommand]
	private void DownloadStarts()
	{
#if ANDROID
		FakeAppUpdateManager.SetBytesDownloaded(0);
		FakeAppUpdateManager.SetTotalBytesToDownload(10_000_000);
		FakeAppUpdateManager.DownloadStarts();
#endif
	}
	
	[RelayCommand]
	private void DownloadCompletes()
	{
#if ANDROID
		FakeAppUpdateManager.SetBytesDownloaded(10_000_000);
		FakeAppUpdateManager.DownloadCompletes();
#endif
	}
	
	[RelayCommand]
	private void InstallCompletes()
	{
#if ANDROID
		FakeAppUpdateManager.InstallCompletes();
#endif
	}
	
	[RelayCommand]
	private void InstallFails()
	{
#if ANDROID
		FakeAppUpdateManager.InstallFails();
#endif
	}
	
	[RelayCommand]
	private void CompleteUpdate()
	{
#if ANDROID
		Internal.Handler.Options.CompleteUpdateAction();
#endif
	}
	
	[RelayCommand]
	private async Task Downloading()
	{
#if ANDROID
		for (var i = 0; i < 100; i += 10)
		{
			Internal.Handler.Options.DownloadingAction(i);
			await Task.Delay(TimeSpan.FromMilliseconds(250));
		}
#else
		await Task.Delay(TimeSpan.FromMilliseconds(250));
#endif
	}
	
#if ANDROID
	private static FakeAppUpdateManager FakeAppUpdateManager =>
		(Internal.Handler.AppUpdateManager as FakeAppUpdateManager)!;
	
	private static void AddOnSuccessListener()
	{
		FakeAppUpdateManager.AppUpdateInfo.AddOnSuccessListener(Internal.Handler.AppUpdateSuccessListener!);
	}
#endif
}

