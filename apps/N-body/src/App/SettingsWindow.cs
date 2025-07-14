using System.Runtime.Versioning;
using EasyUIBinding.GirCore;
using Microsoft.Extensions.Localization;

namespace NBody.App;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class SettingsWindow : Adw.ApplicationWindow
{
	private readonly Settings _settings;
	private readonly Gio.Menu _menu;

	private readonly IStringLocalizer<Settings> L;
	private readonly IStringLocalizer<About> AL;
	public Settings Settings => _settings;
	public Gio.Menu Menu => _menu;

	public SettingsWindow(Settings settings, IStringLocalizer<Settings> localizer, IStringLocalizer<About> aboutLocalizer)
	{
		ArgumentNullException.ThrowIfNull(settings);
		ArgumentNullException.ThrowIfNull(localizer);
		ArgumentNullException.ThrowIfNull(aboutLocalizer);

		_settings = settings;

		L = localizer;
		AL = aboutLocalizer;

		SetDefaultSize(600, 900);
		SetTitle(L["N-body simulation"]);

		var headerBar = Adw.HeaderBar.New();

		_menu = new Gio.Menu();
		_menu.Append(AL["About"], "app.about");

		var menuButton = Gtk.MenuButton.New();
		menuButton.IconName = "open-menu-symbolic";
		menuButton.SetMenuModel(_menu);
		headerBar.PackEnd(menuButton);

		var toolbarView = Adw.ToolbarView.New();
		toolbarView.AddTopBar(headerBar);
		var box = UI.Box(Gtk.Orientation.Vertical, 0);
		box.Append(toolbarView);
		box.Append(UI.Scroll(_settings));

		Content = box;
		var appSettings = Gio.Settings.New(Program.ApplicationId);
		appSettings.Bind($"settings-width", this, "default-width", Gio.SettingsBindFlags.Default);
		appSettings.Bind($"settings-height", this, "default-height", Gio.SettingsBindFlags.Default);
		appSettings.Bind($"settings-maximized", this, "maximized", Gio.SettingsBindFlags.Default);
	}

	public override void Dispose()
	{
		_settings.Dispose();
		base.Dispose();
	}
}