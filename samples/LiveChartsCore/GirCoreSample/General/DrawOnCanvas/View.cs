using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.General.DrawOnCanvas;

namespace GirCoreSample.General.DrawOnCanvas;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly CartesianChart cartesianChart;

	public View()
	{
		var viewModel = new ViewModel();

		cartesianChart = new CartesianChart();
		cartesianChart.UpdateStartedCommand = new Command(() => viewModel.ChartUpdated(new(cartesianChart)));
		cartesianChart.PointerPressedCommand = new Command(() => viewModel.ChartUpdated(new(cartesianChart)));

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart?.Dispose();
		base.Dispose();
	}
}
