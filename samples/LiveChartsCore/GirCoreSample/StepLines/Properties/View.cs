using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.StepLines.Properties;

namespace GirCoreSample.StepLines.Properties;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly WrapPreferencesGroup wrapGroup;
	private readonly CartesianChart cartesianChart;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		var viewModel = new ViewModel();
		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
		};
		wrapGroup = new WrapPreferencesGroup(
		[
			new Button("New values").OnClick(() => viewModel.ChangeValuesInstance()),
			new Button("New fill").OnClick(() => viewModel.NewFill()),
			new Button("New stroke").OnClick(() => viewModel.NewStroke()),
			new Button("New geometry fill").OnClick(() => viewModel.NewGeometryFill()),
			new Button("New geometry stroke").OnClick(() => viewModel.NewGeometryStroke()),
			new Button("+ size").OnClick(() => viewModel.IncreaseGeometrySize()),
			new Button("- size").OnClick(() => viewModel.DecreaseGeometrySize()),
			new Button("New series").OnClick(() =>
			{
				viewModel.ChangeSeriesInstance();
				cartesianChart.Series = viewModel.Series;
			})
		]);

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
