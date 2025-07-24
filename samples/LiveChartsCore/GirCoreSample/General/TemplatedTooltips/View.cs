using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.General.TemplatedTooltips;

namespace GirCoreSample.General.TemplatedTooltips;

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
			Tooltip = new CustomTooltip()
		};

		Child = cartesianChart;
	}

	public override void Dispose()
	{
		cartesianChart?.Dispose();
		base.Dispose();
	}
}

