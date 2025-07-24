using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.Basic;

namespace GirCoreSample.Pies.Basic;

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
			Title = viewModel.Title
		};

		Child = pieChart;
	}

	public override void Dispose()
	{
		pieChart?.Dispose();
		base.Dispose();
	}
}
