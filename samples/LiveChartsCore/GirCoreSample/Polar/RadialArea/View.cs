using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Polar.RadialArea;

namespace GirCoreSample.Polar.RadialArea;

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
			AngleAxes = viewModel.AngleAxes,
			InitialRotation = -45,
		};

		Child = polarChart;
	}

	public override void Dispose()
	{
		polarChart?.Dispose();
		base.Dispose();
	}
}
