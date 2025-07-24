using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.Gauge2;

namespace GirCoreSample.Pies.Gauge2;

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
			InitialRotation = -225,
			MaxAngle = 270,
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
