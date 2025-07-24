using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.Shared;

namespace GirCoreSample.Axes.Shared;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly CartesianChart cartesianChart1;
	private readonly CartesianChart cartesianChart2;

	public View()
	{
		var viewModel = new ViewModel();

		cartesianChart1 = new CartesianChart
		{
			Series = viewModel.SeriesCollection1,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
			DrawMargin = viewModel.DrawMargin,
			XAxes = viewModel.X1
		};

		cartesianChart2 = new CartesianChart
		{
			Series = viewModel.SeriesCollection2,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
			DrawMargin = viewModel.DrawMargin,
			XAxes = viewModel.X2
		};

		var box = Gtk.Box.New(Gtk.Orientation.Vertical, 5);
		box.Append(cartesianChart1);
		box.Append(cartesianChart2);

		Child = box;
	}

	public override void Dispose()
	{
		cartesianChart1.Dispose();
		cartesianChart2.Dispose();
		base.Dispose();
	}
}
