using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore.Kernel.Events;
using System.Runtime.Versioning;
using ViewModelsSamples.Events.Tutorial;

namespace GirCoreSample.Events.Tutorial;

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
			FindingStrategy = viewModel.Strategy,
			PointerPressedCommand = new Command<PointerCommandArgs>(viewModel.OnPressed),
			HoveredPointsChangedCommand = new Command<HoverCommandArgs>(viewModel.OnHoveredPointsChanged)
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart?.Dispose();
		base.Dispose();
	}
}
