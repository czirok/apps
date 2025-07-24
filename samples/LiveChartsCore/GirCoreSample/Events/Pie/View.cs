using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using System.Runtime.Versioning;
using ViewModelsSamples.Events.Pie;

namespace GirCoreSample.Events.Pie;

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
		};
		pieChart.DataPointerDown += Chart_DataPointerDown;

		Child = pieChart;
	}

	private void Chart_DataPointerDown(IChartView chart, IEnumerable<ChartPoint> points)
	{
		// notice in the chart event we are not able to use strongly typed points
		// but we can cast the point.Context.DataSource to the actual type.

		foreach (var point in points)
		{
			if (point.Context.DataSource is City city)
			{
				Console.WriteLine($"[chart.dataPointerDownEvent] clicked on {city.Name}");
				continue;
			}

			if (point.Context.DataSource is int integer)
			{
				Console.WriteLine($"[chart.dataPointerDownEvent] clicked on number {integer}");
				continue;
			}

			// handle more possible types here...
			// if (point.Context.DataSource is Foo foo)
			// {
			//     ...
			// }
		}
	}

	public override void Dispose()
	{
		pieChart?.Dispose();
		base.Dispose();
	}
}
