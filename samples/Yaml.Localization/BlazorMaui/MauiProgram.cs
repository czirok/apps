// BlazorMaui
using BlazorShared.Layout;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Yaml.Localization;

namespace BlazorMaui
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});
			builder.Services
				.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance)
#if IOS || ANDROID
				.AddSingleton<IPlatformService, MauiTouchPlatformService>()
#elif WINDOWS || MACCATALYST || TIZEN
				.AddSingleton<IPlatformService, MauiPointerPlatformService>()
#endif
				.AddSingleton<ChangeThemeStore>()
				.AddSingleton<CultureSelectorStore>()
				.AddYamlEmbeddedResourceLocalization(typeof(BlazorShared.Lang).Assembly);

			builder.Services.AddSingleton<MauiThemeService>();

			builder.Services.AddSingleton<MainPage>();

			builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
