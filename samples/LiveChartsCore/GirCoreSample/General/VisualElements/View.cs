using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.General.VisualElements;

namespace GirCoreSample.General.VisualElements;

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
			VisualElements = viewModel.VisualElements,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart?.Dispose();
		base.Dispose();
	}
}
