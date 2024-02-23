using Maui.Android.InAppUpdates;
using Microsoft.Extensions.Logging;

namespace TrimmingHelperApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			//.UseMauiApp<App>()
			.UseAndroidInAppUpdates()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}
