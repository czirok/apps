using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Runtime.Versioning;

namespace NBody.App;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class Manager
{
	private readonly IServiceProvider _services;
	private readonly CanvasWindow _canvasWindow;
	private readonly SettingsWindow _settingsWindow;
	private readonly IStringLocalizer<Settings> L;
	private readonly IStringLocalizer<About> AL;

	private Gio.SimpleAction? _aboutAction;

	public Manager(IServiceProvider services, CanvasWindow canvas, SettingsWindow settings, IStringLocalizer<Settings> localizer, IStringLocalizer<About> aboutLocalizer)
	{
		ArgumentNullException.ThrowIfNull(canvas);
		ArgumentNullException.ThrowIfNull(settings);
		ArgumentNullException.ThrowIfNull(localizer);

		_services = services;
		_canvasWindow = canvas;
		_settingsWindow = settings;

		L = localizer;
		AL = aboutLocalizer;
	}

	public void Run(Adw.Application application)
	{
		ArgumentNullException.ThrowIfNull(application);
		CreateAbout(application);
		CreateSettings(application);
		CreateCanvas(application);
	}

	private void CreateAbout(Adw.Application application)
	{
		_aboutAction = Gio.SimpleAction.New("about", null);
		_aboutAction.OnActivate += ShowAbout;
		application?.AddAction(_aboutAction);
	}

	private void ShowAbout(Gio.SimpleAction sender, Gio.SimpleAction.ActivateSignalArgs args)
		=> _services.GetRequiredService<About>().Window.Present();

	private void CreateCanvas(Adw.Application application)
	{
		_canvasWindow.SetApplication(application!);
		_canvasWindow.OnCloseRequest += (sender, args) =>
		{
			(sender as CanvasWindow)?.Close();
			_settingsWindow?.Close();
			return false;
		};
		_canvasWindow.Present();
	}

	private void CreateSettings(Adw.Application application)
	{
		_settingsWindow.SetApplication(application);
		_settingsWindow.Settings.LanguageChanged += () =>
		{
			_settingsWindow.SetTitle(L["N-body simulation"]);
			_settingsWindow.Menu.RemoveAll();
			_settingsWindow.Menu.Append(AL["About"], "app.about");
		};
		_settingsWindow.OnCloseRequest += (sender, args) =>
		{
			(sender as SettingsWindow)?.Close();
			_canvasWindow?.Close();
			return false;
		};
		_settingsWindow.Present();
	}

	public void Dispose()
	{
		_aboutAction?.OnActivate -= ShowAbout;
		_aboutAction?.Dispose();
		_canvasWindow?.Dispose();
		_settingsWindow?.Dispose();
	}
}