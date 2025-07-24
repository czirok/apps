using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Axes.LabelsRotation;

namespace GirCoreSample.Axes.LabelsRotation;

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
			new SpinInteger("Labels rotation", "Labels rotation", 0, new IntRange(-360, 720, 5))
				.BindTo(viewModel, nameof(viewModel.SliderValue))
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
