using EasyUIBinding;
using EasyUIBinding.GirCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal class Program
{
	internal const string ApplicationId = "easyuibinding.gircore";
	private static void Main(string[] _)
	{
		// Initialize the required modules
		Adw.Module.Initialize();

		// Create a service collection and add the SamplePreferencesPage
		// This is where we can add any services we want to use in the application
		var provider = new ServiceCollection()
			.AddLogging(config =>
			{
				config.AddSimpleConsole(options =>
				{
					options.SingleLine = true;
					options.TimestampFormat = "HH:mm:ss ";
				});
			})
			.AddSingleton<SamplePreferencesPage>()
			.BuildServiceProvider();

		// Create the Adw application
		var app = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);
		SamplePreferencesPage? samplePreferencesPage = null;

		app.OnActivate += (sender, args) =>
		{
			// Create header bar and toolbar view
			var headerBar = Adw.HeaderBar.New();
			var toolbarView = Adw.ToolbarView.New();
			toolbarView.AddTopBar(headerBar);
			var box = Gtk.Box.New(Gtk.Orientation.Vertical, 0);
			box.Append(toolbarView);

			samplePreferencesPage = provider.GetRequiredService<SamplePreferencesPage>();
			box.Append(UI.Scroll(samplePreferencesPage));

			// Create window and show
			var window = Adw.ApplicationWindow.New((Adw.Application)sender);
			window.Title = "Easy UI Binding Sample";
			window.Content = box;
			window.SetDefaultSize(600, 900);
			window.Show();
		};

		app.OnShutdown += (sender, args) =>
		{
			samplePreferencesPage?.Dispose();
			provider?.Dispose();
		};

		app.Run(0, null);
	}
}