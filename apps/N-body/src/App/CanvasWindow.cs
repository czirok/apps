using System.Runtime.Versioning;

namespace NBody.App;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class CanvasWindow : Adw.ApplicationWindow
{
	private readonly Canvas _canvas;

	public CanvasWindow(Canvas canvas)
	{
		_canvas = canvas;
		SetDefaultSize(900, 700);
		SetTitle(string.Empty);

		// Black background
		var provider = Gtk.CssProvider.New();
		var css = ".black-canvas { background-color: black; }";
		provider.LoadFromData(css, css.Length);
		Gtk.StyleContext.AddProviderForDisplay(Gdk.Display.GetDefault()!, provider, Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);

		var overlay = Gtk.Overlay.New();
		overlay.SetCssClasses(["black-canvas"]);
		overlay.SetHexpand(true);
		overlay.SetVexpand(true);

		overlay.SetChild(_canvas);

		var toolbarView = Adw.ToolbarView.New();
		toolbarView.AddTopBar(Adw.HeaderBar.New());
		toolbarView.SetHalign(Gtk.Align.Fill);
		toolbarView.SetValign(Gtk.Align.Start);
		toolbarView.SetVexpand(false);
		toolbarView.SetHexpand(true);

		overlay.AddOverlay(toolbarView);

		Content = overlay;

		var appSettings = Gio.Settings.New(Program.ApplicationId);
		appSettings.Bind($"canvas-width", this, "default-width", Gio.SettingsBindFlags.Default);
		appSettings.Bind($"canvas-height", this, "default-height", Gio.SettingsBindFlags.Default);
		appSettings.Bind($"canvas-maximized", this, "maximized", Gio.SettingsBindFlags.Default);
	}

	public override void Dispose()
	{
		_canvas.Dispose();
		base.Dispose();
	}
}