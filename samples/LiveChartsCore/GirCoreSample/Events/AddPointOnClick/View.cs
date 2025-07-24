using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Measure;
using System.Runtime.Versioning;
using ViewModelsSamples.Events.AddPointOnClick;

namespace GirCoreSample.Events.AddPointOnClick;

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
			Series = viewModel.SeriesCollection,
			PointerPressedCommand = new Command<PointerCommandArgs>(viewModel.PointerDown),
			TooltipPosition = TooltipPosition.Hidden,
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart?.Dispose();
		base.Dispose();
	}
}
