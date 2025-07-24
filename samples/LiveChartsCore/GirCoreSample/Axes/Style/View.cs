using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.Style;

namespace GirCoreSample.Axes.Style;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly CartesianChart cartesianChart;

	public View()
	{
		// Gray background
		var provider = Gtk.CssProvider.New();
		var css = ".gray-canvas { background-color: #3C3C3C; }";
		provider.LoadFromData(css, css.Length);
		Gtk.StyleContext.AddProviderForDisplay(Gdk.Display.GetDefault()!, provider, Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);
		SetCssClasses(["gray-canvas"]);

		var viewModel = new ViewModel();

		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
			XAxes = viewModel.XAxes,
			YAxes = viewModel.YAxes,
			DrawMarginFrame = viewModel.Frame,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.Both,
			TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Hidden
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart.Dispose();
		base.Dispose();
	}
}
