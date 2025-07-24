using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Events.Polar;

namespace GirCoreSample.Events.Polar;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly PolarChart polarChart;

	public View()
	{
		var viewModel = new ViewModel();

		polarChart = new PolarChart
		{
			Series = viewModel.Series,
		};

		Child = polarChart;
	}

	public override void Dispose()
	{
		polarChart?.Dispose();
		base.Dispose();
	}
}
