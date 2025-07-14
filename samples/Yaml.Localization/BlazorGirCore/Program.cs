// BlazorGirCore
using System.Runtime.Versioning;
using BlazorShared.Layout;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebKit.BlazorWebView.GirCore;
using Yaml.Localization;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal class Program
{
	internal const string ApplicationId = "gir.core.blazorsample";
	internal const string ApplicationVersion = "1.0.0.0";

	private static void Main(string[] _)
	{
		foreach (var resource in typeof(BlazorShared.Lang).Assembly.GetManifestResourceNames())
		{
			Console.WriteLine(resource);
		}

		Adw.Module.Initialize();
		Gtk.Module.Initialize();
		WebKit.Module.Initialize();
		var services = new ServiceCollection()
			.AddLogging(config =>
			{
				config.AddSimpleConsole(options =>
				{
					options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
					options.SingleLine = true;
					options.TimestampFormat = "HH:mm:ss ";
				}).SetMinimumLevel(LogLevel.Information);
			});
		services
			.AddSingleton<IPlatformService, GirCorePlatformService>()
			.AddSingleton<ChangeThemeStore>()
			.AddSingleton<CultureSelectorStore>()
			.AddYamlEmbeddedResourceLocalization(typeof(BlazorShared.Lang).Assembly)
			.AddBlazorWebView(
				new BlazorWebViewOptions()
				{
					RootComponent = typeof(BlazorGirCore.Components.App),
					HostPath = "wwwroot/index.html"
				});

		var provider = services.BuildServiceProvider();
		var application = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);

		application.OnActivate += (sender, args) =>
		{
			var window = Gtk.ApplicationWindow.New((Adw.Application)sender);
			window.Title = "Blazor GirCore";
			window.SetDefaultSize(800, 600);

			var webView = new BlazorWebView(provider);
			window.SetChild(webView);
			window.Show();

			// Allow opening developer tools
			webView.GetSettings().EnableDeveloperExtras = true;
		};

		application.OnShutdown += (sender, args) =>
		{
		};

		application.RunWithSynchronizationContext([]);
	}
}
