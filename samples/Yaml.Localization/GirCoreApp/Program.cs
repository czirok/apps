using GirCoreApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

internal class Program
{
	internal const string ApplicationId = "gir.core.localizationtest";
	internal static Gio.Cancellable Cancellable = Gio.Cancellable.New();

	private static void Main(string[] _)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		foreach (var resource in typeof(CultureSample).Assembly.GetManifestResourceNames())
		{
			Console.WriteLine(resource);
		}

		Adw.Module.Initialize();
		Gio.Module.Initialize();

		var services = new ServiceCollection()
			.AddLogging(config =>
			{
				config.AddSimpleConsole(options =>
				{
					options.SingleLine = true;
					options.TimestampFormat = "HH:mm:ss ";
				});
			})
			.AddYamlEmbeddedResourceLocalization(typeof(CultureSample).Assembly)
			.AddSingleton<CultureSample>()
			.AddSingleton<CultureSampleWindow>()
			.BuildServiceProvider();

		var application = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);
		application.OnActivate += (sender, args) =>
		{
			var window = services.GetRequiredService<CultureSampleWindow>();
			window.SetApplication((Adw.Application)sender);
			window.Present();
		};
		application.Run(0, null);
	}
}
