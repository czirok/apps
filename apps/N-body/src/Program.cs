using Microsoft.Extensions.DependencyInjection;
using NBody.App;
using System.Runtime.Versioning;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal class Program
{
	internal const string ApplicationId = "org.gnome.nbody";
	internal const string ApplicationVersion = "1.2.1";

	private static void Main(string[] _)
	{
		Adw.Module.Initialize();
		Gtk.Module.Initialize();
		GdkPixbuf.Module.Initialize();
		Cairo.Module.Initialize();
		Graphene.Module.Initialize();

		var provider = new ServiceCollection()
			.AddNBodyApp()
			.BuildServiceProvider();
		var application = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);
		Manager? manager = null;

		application.OnActivate += (sender, args) =>
		{
			manager = provider.GetRequiredService<Manager>();
			manager.Run((Adw.Application)sender);
		};

		application.OnShutdown += (sender, args) =>
		{
			manager?.Dispose();
			provider?.Dispose();
		};

		application.Run(0, null);
	}
}
