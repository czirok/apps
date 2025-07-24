using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.ColorsAndPosition;

namespace GirCoreSample.Axes.ColorsAndPosition;

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
			new Button("Toggle position").OnClick(() => viewModel.TogglePosition()),
			new Button("New color").OnClick(() => viewModel.SetNewColor())
		]);
		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
			XAxes = viewModel.XAxes,
			YAxes = viewModel.YAxes,
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

