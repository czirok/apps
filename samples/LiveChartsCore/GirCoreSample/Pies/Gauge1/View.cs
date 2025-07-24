using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.Gauge1;

namespace GirCoreSample.Pies.Gauge1;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly PieChart pieChart;

	public View()
	{
		var viewModel = new ViewModel();

		pieChart = new PieChart
		{
			Series = viewModel.Series,
			InitialRotation = -90,
			MinValue = 0,
			MaxValue = 100,
		};

		Child = pieChart;
	}

	public override void Dispose()
	{
		pieChart?.Dispose();
		base.Dispose();
	}
}
