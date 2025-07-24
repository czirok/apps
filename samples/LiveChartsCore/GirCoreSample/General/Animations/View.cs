using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore;
using System.Runtime.Versioning;
using ViewModelsSamples.General.Animations;

namespace GirCoreSample.General.Animations;

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
			new ComboTuple<Func<float, float>>("curve", "Select Curve", viewModel.AvalaibaleCurves, EasingFunctions.BackOut, EasingFunctions.BackOut)
				.Bind(viewModel, nameof(viewModel.SelectedCurve)),
			new ComboTuple<TimeSpan>("speed", "Select Speed", viewModel.AvailableSpeeds, TimeSpan.FromMilliseconds(1300), TimeSpan.FromMilliseconds(1300))
				.Bind(viewModel, nameof(viewModel.SelectedSpeed))
		]);
		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
			AnimationsSpeed = viewModel.SelectedSpeed.Item2,
			EasingFunction = viewModel.SelectedCurve.Item2,
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
