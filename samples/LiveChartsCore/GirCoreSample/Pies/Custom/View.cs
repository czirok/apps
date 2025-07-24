using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.Custom;

namespace GirCoreSample.Pies.Custom;

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
		};

		Child = pieChart;
	}

	public override void Dispose()
	{
		pieChart?.Dispose();
		base.Dispose();
	}
}
