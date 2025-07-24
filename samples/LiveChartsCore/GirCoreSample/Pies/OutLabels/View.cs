using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.OutLabels;

namespace GirCoreSample.Pies.OutLabels;

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
			IsClockwise = false,
			InitialRotation = -90
		};

		Child = pieChart;
	}

	public override void Dispose()
	{
		pieChart?.Dispose();
		base.Dispose();
	}
}
