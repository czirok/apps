using System.Diagnostics;
using System.Runtime.Versioning;
using BlazorGirCoreApp.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebKit.BlazorWebView.GirCore;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal class Program
{
	internal static long StartTime;
	internal const string ApplicationId = "gir.core.blazorsample";
	internal const string ApplicationVersion = "1.0.0.0";

	private static void Main(string[] _)
	{
		StartTime = Environment.TickCount64;
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
			})
			.AddSingleton<WeatherForecastService>()
			.AddBlazorWebView(
				new BlazorWebViewOptions()
				{
					RootComponent = typeof(BlazorGirCoreApp.App),
					HostPath = "wwwroot/index.html"
				}
			)
			.BuildServiceProvider();

		Console.WriteLine($"After ServiceProvider: {Environment.TickCount64 - StartTime} ms");

		var application = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);

		application.OnActivate += (sender, args) =>
		{
			Console.WriteLine($"OnActivate: {Environment.TickCount64 - StartTime} ms");
			var window = Gtk.ApplicationWindow.New((Adw.Application)sender);
			window.Title = "Blazor";
			window.SetDefaultSize(800, 600);

			Console.WriteLine($"Before BlazorWebView: {Environment.TickCount64 - StartTime} ms");

			var webView = new BlazorWebView(services);
			window.SetChild(webView);
			window.Show();
			Console.WriteLine($"After BlazorWebView: {Environment.TickCount64 - StartTime} ms");
			// Allow opening developer tools
			webView.GetSettings().EnableDeveloperExtras = true;
		};

		application.OnShutdown += (sender, args) =>
		{
		};

		Console.WriteLine($"Before Run: {Environment.TickCount64 - StartTime} ms");
		application.RunWithSynchronizationContext([]);
	}
}
