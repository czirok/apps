using EasyUIBinding.GirCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Runtime.Versioning;

namespace Gomoku;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class GomokuWindow : Adw.ApplicationWindow
{
	private readonly GomokuControls _controls;
	private readonly Gtk.Button _settingsButton;
	private readonly Gtk.Button _aboutButton;
	private readonly IStringLocalizer<I18N> L;
	private readonly IServiceProvider _services;

	public GomokuControls GomokuControls => _controls;

	public GomokuWindow(IServiceProvider services, GomokuControls controls, IStringLocalizer<I18N> localizer)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(controls);
		ArgumentNullException.ThrowIfNull(localizer);
		L = localizer;

		_services = services;
		_controls = controls;
		_controls.LanguageChanged += () =>
		{
			SetTitle(L["Gomoku"]);
			if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
			{
				SetDefaultDirection(Gtk.TextDirection.Rtl);
			}
			else
			{
				SetDefaultDirection(Gtk.TextDirection.Ltr);
			}
		};

		SetDefaultSize(720, 840);
		SetTitle(L["Gomoku"]);

		var headerBar = Adw.HeaderBar.New();

		_settingsButton = Gtk.Button.NewFromIconName("preferences-system-symbolic");
		_settingsButton.OnClicked += OpenDialog;

		_aboutButton = Gtk.Button.NewFromIconName("help-about-symbolic");
		_aboutButton.OnClicked += OpenAbout;

		headerBar.PackEnd(_aboutButton);
		headerBar.PackEnd(Gtk.Separator.New(Gtk.Orientation.Vertical));
		headerBar.PackEnd(_settingsButton);

		headerBar.PackStart(_controls.UndoButton);
		headerBar.PackStart(_controls.RedoButton);
		headerBar.PackStart(Gtk.Separator.New(Gtk.Orientation.Vertical));
		headerBar.PackStart(_controls.RestartButton);


		var toolbarView = Adw.ToolbarView.New();
		toolbarView.AddTopBar(headerBar);
		var box = UI.Box(Gtk.Orientation.Vertical, 0);
		box.Append(toolbarView);
		box.Append(_controls);

		Content = box;

		var appSettings = Gio.Settings.New(Program.ApplicationId);
		appSettings.Bind($"app-width", this, "default-width", Gio.SettingsBindFlags.Default);
		appSettings.Bind($"app-height", this, "default-height", Gio.SettingsBindFlags.Default);
		appSettings.Bind($"app-maximized", this, "maximized", Gio.SettingsBindFlags.Default);
	}

	private void OpenAbout(Gtk.Button sender, EventArgs args)
	{
		_services.GetRequiredService<About>().Window.Present();
	}

	private void OpenDialog(Gtk.Button sender, EventArgs args)
	{
		_controls.SettingsDialog.Present(this);
	}

	public override void Dispose()
	{
		_settingsButton.OnClicked -= OpenDialog;
		_aboutButton.OnClicked -= OpenAbout;
		_controls.LanguageChanged -= null;
		_controls.Dispose();
		base.Dispose();
	}
}