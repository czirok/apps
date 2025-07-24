using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.MatchScale;

namespace GirCoreSample.Axes.MatchScale;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly CartesianChart cartesianChart;

	public View()
	{
		var viewModel = new ViewModel();

		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
			XAxes = viewModel.XAxes,
			YAxes = viewModel.YAxes,
			DrawMarginFrame = viewModel.Frame,
			MatchAxesScreenDataRatio = true,
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
