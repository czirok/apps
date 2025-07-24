using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.VisualTest.TwoChartsOneSeries;

namespace GirCoreSample.VisualTest.TwoChartsOneSeries;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box
{
	private readonly CartesianChart cartesianChart1;
	private readonly CartesianChart cartesianChart2;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		var viewModel = new ViewModel();

		cartesianChart1 = new CartesianChart
		{
			Series = viewModel.Series,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
		};

		cartesianChart2 = new CartesianChart
		{
			Series = viewModel.Series,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
		};

		Append(cartesianChart1);
		Append(cartesianChart2);
	}

	public override void Dispose()
	{
		cartesianChart1.Dispose();
		cartesianChart2.Dispose();
		base.Dispose();
	}
}

