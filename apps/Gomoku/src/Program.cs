using Gomoku;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal class Program
{
	internal const string ApplicationId = "org.gnome.pelagomoku";
	internal const string ApplicationVersion = "1.0";

	private static void Main(string[] _)
	{
		// Ensure that the I18N class is initialized to prevent trimming issues
		// and to ensure that the necessary resources are available at runtime.
		RuntimeHelpers.RunClassConstructor(typeof(I18N).TypeHandle);

		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		Adw.Module.Initialize();
		Gtk.Module.Initialize();
		GdkPixbuf.Module.Initialize();
		Cairo.Module.Initialize();
		Graphene.Module.Initialize();

		var config = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.Build();
		ArgumentNullException.ThrowIfNull(config, "Configuration is required.");

		var provider = new ServiceCollection()
			.AddGomokuApp()
			.BuildServiceProvider();

		var application = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);
		application.OnActivate += (sender, args) =>
		{
			var window = provider.GetRequiredService<GomokuWindow>();
			window.SetApplication((Adw.Application)sender);
			window.Present();
		};
		application.Run(0, null);
	}
}
