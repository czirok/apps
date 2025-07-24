using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Test.Dispose;

namespace GirCoreSample.Test.Dispose;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class Widget : Gtk.Box, IDisposable
{
	private readonly CartesianChart cartesianChart;
	private readonly PieChart pieChart;
	private readonly PolarChart polarChart;
	private readonly GeoMap geoMap;

	public Widget()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		var viewModel = new ViewModel();

		var chartBox = Gtk.Box.New(Gtk.Orientation.Vertical, 0);
		var leftBox = Gtk.Box.New(Gtk.Orientation.Horizontal, 0);
		cartesianChart = new CartesianChart()
			.BindTo(viewModel, nameof(ViewModel.CartesianSeries), nameof(CartesianChart.Series));
		pieChart = new PieChart()
			.BindTo(viewModel, nameof(ViewModel.PieSeries), nameof(PieChart.Series));
		leftBox.Append(cartesianChart);
		leftBox.Append(pieChart);

		var rightBox = Gtk.Box.New(Gtk.Orientation.Horizontal, 0);
		polarChart = new PolarChart()
			.BindTo(viewModel, nameof(ViewModel.PolarSeries), nameof(PolarChart.Series));
		geoMap = new GeoMap()
			.BindTo(viewModel, nameof(ViewModel.GeoSeries), nameof(GeoMap.Series));
		rightBox.Append(polarChart);
		rightBox.Append(geoMap);

		chartBox.Append(leftBox);
		chartBox.Append(rightBox);
		Append(chartBox);
	}

	public override void Dispose()
	{
		cartesianChart.Dispose();
		pieChart.Dispose();
		polarChart.Dispose();
		geoMap.Dispose();
		base.Dispose();
	}
}
