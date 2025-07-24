using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.NamedLabels;

namespace GirCoreSample.Axes.NamedLabels;

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
			TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Left, // mark
			TooltipTextPaint = viewModel.TooltipTextPaint, // mark
			TooltipBackgroundPaint = viewModel.TooltipBackgroundPaint, // mark
			TooltipTextSize = 16 // mark
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart.Dispose();
		base.Dispose();
	}
}
