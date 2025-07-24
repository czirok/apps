using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Test.ChangeSeriesInstance;

namespace GirCoreSample.Test.ChangeSeriesInstance;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly WrapPreferencesGroup wrapGroup;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		var viewModel = new ViewModel();

		var chartBox = Gtk.Box.New(Gtk.Orientation.Vertical, 0);
		var leftBox = Gtk.Box.New(Gtk.Orientation.Horizontal, 0);
		var cartesianChart = new CartesianChart()
			.BindTo(viewModel, nameof(ViewModel.CartesianSeries), nameof(CartesianChart.Series));
		var pieChart = new PieChart()
			.BindTo(viewModel, nameof(ViewModel.PieSeries), nameof(PieChart.Series));
		leftBox.Append(cartesianChart);
		leftBox.Append(pieChart);

		var rightBox = Gtk.Box.New(Gtk.Orientation.Horizontal, 0);
		var polarChart = new PolarChart()
			.BindTo(viewModel, nameof(ViewModel.PolarSeries), nameof(PolarChart.Series));
		var geoMap = new GeoMap()
			.BindTo(viewModel, nameof(ViewModel.GeoSeries), nameof(GeoMap.Series));
		rightBox.Append(polarChart);
		rightBox.Append(geoMap);

		chartBox.Append(leftBox);
		chartBox.Append(rightBox);

		wrapGroup = new WrapPreferencesGroup(
			[
				new Button("Change Data")
			.OnClick((s, e) => viewModel.GenerateData())

			]);

		Append(wrapGroup);
		Append(chartBox);
	}

	public override void Dispose()
	{
		wrapGroup.Dispose();
		base.Dispose();
	}
}
