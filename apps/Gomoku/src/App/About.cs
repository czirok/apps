using Microsoft.Extensions.Localization;
using System.Runtime.Versioning;

namespace Gomoku;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class About
{
	public Adw.AboutWindow Window { get; }

	private readonly GomokuWindow _window;
	private readonly IStringLocalizer<I18N> L;

	public About(GomokuWindow window, IStringLocalizer<I18N> localizer)
	{
		ArgumentNullException.ThrowIfNull(window);
		ArgumentNullException.ThrowIfNull(localizer);

		_window = window;
		L = localizer;
		Window = Adw.AboutWindow.New();
		Window.SetTransientFor(_window);
		Window.SetApplicationName(L["Gomoku"]);
		Window.SetApplicationIcon("org.gnome.pelagomoku");
		Window.SetVersion(Program.ApplicationVersion);
		Window.SetDeveloperName("Ferenc Czirok");
		Window.SetComments(L["About Comment"]);
		Window.SetWebsite("https://github.com/czirok/apps/apps/Gomoku");
		Window.SetDevelopers(["Petr Laštovička https://github.com/plastovicka/Piskvork", "Ferenc Czirok https://github.com/czirok/apps/apps/Gomoku"]);
		Window.SetLicenseType(Gtk.License.Gpl30);
	}
}