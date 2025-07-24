using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Polar.Basic;

namespace GirCoreSample.Polar.Basic;

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
			Title = viewModel.Title,
			AngleAxes = viewModel.AngleAxes,
			RadiusAxes = viewModel.RadialAxes,
		};

		Child = polarChart;
	}

	public override void Dispose()
	{
		polarChart?.Dispose();
		base.Dispose();
	}
}
