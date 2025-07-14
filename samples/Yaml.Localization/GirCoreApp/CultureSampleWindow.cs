using System.Globalization;
using EasyUIBinding.GirCore;
using Microsoft.Extensions.Localization;

namespace GirCoreApp;

public class CultureSampleWindow : Adw.ApplicationWindow
{
	private readonly CultureSample _sample;
	private readonly IStringLocalizer<CultureSample> L;
	public CultureSample CultureSample => _sample;

	public CultureSampleWindow(CultureSample sample, IStringLocalizer<CultureSample> localizer)
	{
		ArgumentNullException.ThrowIfNull(sample);
		ArgumentNullException.ThrowIfNull(localizer);
		L = localizer;

		_sample = sample;
		_sample.LanguageChanged += () =>
		{
			SetTitle(L["Yaml Localization"]);
			if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
			{
				SetDefaultDirection(Gtk.TextDirection.Rtl);
			}
			else
			{
				SetDefaultDirection(Gtk.TextDirection.Ltr);
			}
		};

		SetDefaultSize(600, 800);
		SetTitle(L["Yaml Localization"]);

		var headerBar = Adw.HeaderBar.New();

		var toolbarView = Adw.ToolbarView.New();
		toolbarView.AddTopBar(headerBar);
		var box = UI.Box(Gtk.Orientation.Vertical, 0);
		box.Append(toolbarView);
		box.Append(UI.Scroll(_sample));

		Content = box;
	}

	public override void Dispose()
	{
		_sample.Dispose();
		base.Dispose();
	}
}