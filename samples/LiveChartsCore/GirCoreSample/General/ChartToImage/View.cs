using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore.SkiaSharpView.SKCharts;
using System.Runtime.Versioning;
using ViewModelsSamples.General.ChartToImage;

namespace GirCoreSample.General.ChartToImage;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly CartesianChart _cartesian;
	private readonly PieChart _pie;
	private readonly GeoMap _map;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		var viewModel = new ViewModel();

		_cartesian = new CartesianChart
		{
			Series = viewModel.CatesianSeries,
		};

		_pie = new PieChart
		{
			Series = viewModel.PieSeries,
		};

		_map = new GeoMap
		{
			Series = viewModel.GeoSeries,
		};

		Append(_cartesian);
		Append(_pie);
		Append(_map);

		// now lets create the images // mark
		CreateImageFromCartesianControl(); // mark
		CreateImageFromPieControl(); // mark
		CreateImageFromGeoControl(); // mark
	}

	private void CreateImageFromCartesianControl()
	{
		// you can take any chart in the UI, and build an image from it // mark
		var chartControl = _cartesian;
		var skChart = new SKCartesianChart(chartControl) { Width = 900, Height = 600, };
		skChart.SaveImage("CartesianImageFromControl.png");
	}

	private void CreateImageFromPieControl()
	{
		var chartControl = _pie;
		var skChart = new SKPieChart(chartControl) { Width = 900, Height = 600, };
		skChart.SaveImage("PieImageFromControl.png");
	}

	private void CreateImageFromGeoControl()
	{
		var chartControl = _map;
		var skChart = new SKGeoMap(chartControl) { Width = 900, Height = 600, };
		skChart.SaveImage("MapImageFromControl.png");
	}

	public override void Dispose()
	{
		_cartesian?.Dispose();
		_pie?.Dispose();
		_map?.Dispose();
		base.Dispose();
	}
}
