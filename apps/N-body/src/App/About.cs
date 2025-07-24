using Microsoft.Extensions.Localization;
using System.Runtime.Versioning;

namespace NBody.App;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class About
{
	public Adw.AboutWindow Window { get; }

	private readonly SettingsWindow _settingsWindow;
	private readonly IStringLocalizer<About> L;
	private readonly IStringLocalizer<Settings> SL;

	public About(SettingsWindow settings, IStringLocalizer<About> localizer, IStringLocalizer<Settings> settingsLocalizer)
	{
		ArgumentNullException.ThrowIfNull(settings);
		ArgumentNullException.ThrowIfNull(localizer);
		ArgumentNullException.ThrowIfNull(settingsLocalizer);

		_settingsWindow = settings;
		L = localizer;
		SL = settingsLocalizer;
		Window = Adw.AboutWindow.New();
		Window.SetTransientFor(_settingsWindow);
		Window.SetApplicationName(SL["N-body simulation"]);
		Window.SetApplicationIcon("org.gnome.nbody");
		Window.SetVersion(Program.ApplicationVersion);
		Window.SetDeveloperName("Ferenc Czirok");
		Window.SetComments(L["Comments"]);
		Window.SetWebsite("https://github.com/czirok/apps");
		Window.SetDevelopers(["Zong Zheng Li https://zongzhengli.github.io/nbody.html", "Ferenc Czirok https://github.com/czirok/apps"]);
		Window.SetLicense("""
<b>Creative Commons Attribution 4.0 International</b>
<small><a href="https://creativecommons.org/licenses/by/4.0/">CC BY 4.0</a></small>

<big>You are free to:</big>
• <b>Share</b> — copy and redistribute the material in any medium or format for any purpose, even commercially.

• <b>Adapt</b> — — remix, transform, and build upon the material for any purpose, even commercially.

• The licensor cannot revoke these freedoms as long as you follow the license terms.

<big>Under the following terms:</big>

• <b>Attribution</b> — You must give <a href="https://creativecommons.org/licenses/by/4.0/#ref-appropriate-credit">appropriate credit</a>, provide a link to the license, and <a href="https://creativecommons.org/licenses/by/4.0/#ref-indicate-changes">indicate if changes were made </a>. You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you or your use.

• <b>No additional restrictions</b> — You may not apply legal terms or <a href="https://creativecommons.org/licenses/by/4.0/#ref-technological-measures">technological measures </a>that legally restrict others from doing anything the license permits.

<big>Notices:</big>

You do not have to comply with the license for elements of the material in the public domain or where your use is permitted by an applicable <a href="https://creativecommons.org/licenses/by/4.0/#ref-exception-or-limitation">exception or limitation </a>.

No warranties are given. The license may not give you all of the permissions necessary for your intended use. For example, other rights such as <a href="https://creativecommons.org/licenses/by/4.0/#ref-publicity-privacy-or-moral-rights">publicity, privacy, or moral rights</a> may limit how you use the material.
""");
	}
}