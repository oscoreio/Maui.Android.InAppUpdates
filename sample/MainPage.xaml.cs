using CommunityToolkit.Mvvm.Input;
using Maui.Android.InAppUpdates.Internal;

namespace Maui.Android.InAppUpdates;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	[RelayCommand]
	private void SetUpdateAvailableWithPriorityOf5()
	{
		DebugHelpers.SetUpdateAvailable(availableVersionCode: 2, priority: 5);
	}

	[RelayCommand]
	private void SetUpdateAvailableWithPriorityOf3()
	{
		DebugHelpers.SetUpdateAvailable(availableVersionCode: 2, priority: 3);
	}
}

