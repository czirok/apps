using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.Gauge5;

namespace GirCoreSample.Pies.Gauge5;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly WrapPreferencesGroup wrapGroup;
	private readonly PieChart pieChart;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);

		var viewModel = new ViewModel();
		wrapGroup = new WrapPreferencesGroup(
		[
			new Button("Update").OnClick(() => viewModel.DoRandomChange()),
		]);
		pieChart = new PieChart
		{
			Series = viewModel.Series,
			InitialRotation = -90,
			MaxAngle = 270,
			MinValue = 0,
			MaxValue = 100,
			LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom,
		};

		Append(wrapGroup);
		Append(pieChart);
	}

	public override void Dispose()
	{
		wrapGroup.Dispose();
		pieChart.Dispose();
		base.Dispose();
	}
}
