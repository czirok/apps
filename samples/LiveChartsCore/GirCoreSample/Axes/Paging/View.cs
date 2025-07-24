using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.Paging;

namespace GirCoreSample.Axes.Paging;

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
			new Button("Go to page 1").OnClick(() => viewModel.GoToPage1()),
			new Button("Go to page 2").OnClick(() => viewModel.GoToPage2()),
			new Button("Go to page 3").OnClick(() => viewModel.GoToPage3()),
			new Button("Clear").OnClick(() => viewModel.SeeAll())
		]);
		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
			XAxes = viewModel.XAxes,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
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
