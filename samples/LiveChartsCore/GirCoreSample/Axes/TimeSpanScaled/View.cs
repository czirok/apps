using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.TimeSpanScaled;

namespace GirCoreSample.Axes.TimeSpanScaled;

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
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart.Dispose();
		base.Dispose();
	}
}
