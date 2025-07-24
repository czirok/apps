using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.Multiple;

namespace GirCoreSample.Axes.Multiple;

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
			YAxes = viewModel.YAxes,
			LegendPosition = LiveChartsCore.Measure.LegendPosition.Left,
			LegendTextPaint = viewModel.LegendTextPaint,
			LegendBackgroundPaint = viewModel.LedgendBackgroundPaint,
			LegendTextSize = 16
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart.Dispose();
		base.Dispose();
	}
}
