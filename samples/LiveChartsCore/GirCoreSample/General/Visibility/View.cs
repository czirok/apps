using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.General.Visibility;

namespace GirCoreSample.General.Visibility;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly CartesianChart cartesianChart;
	private readonly WrapPreferencesGroup wrapGroup;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		var viewModel = new ViewModel();
		wrapGroup = new WrapPreferencesGroup(
		[
			new Switch("Toggle Series 1", true).OnSwitchToggled(() => viewModel.ToggleSeries0()),
			new Switch("Toggle Series 2", true).OnSwitchToggled(() => viewModel.ToggleSeries1()),
			new Switch("Toggle Series 3", true).OnSwitchToggled(() => viewModel.ToggleSeries2())
		]);
		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
		};

		Append(wrapGroup);
		Append(cartesianChart);
	}

	public override void Dispose()
	{
		wrapGroup.Dispose();
		cartesianChart.Dispose();
		base.Dispose();
	}
}
