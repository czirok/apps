using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.StepLines.AutoUpdate;

namespace GirCoreSample.StepLines.AutoUpdate;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly WrapPreferencesGroup wrapGroup;
	private readonly CartesianChart cartesianChart;
	private readonly ViewModel viewModel;
	private bool isStreaming = false;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		viewModel = new ViewModel();
		wrapGroup = new WrapPreferencesGroup(
		[
			new Button("Add item").OnClick(() => viewModel.AddItem()),
			new Button("Replace item").OnClick(() => viewModel.ReplaceItem()),
			new Button("Remove item").OnClick(() => viewModel.RemoveItem()),
			new Button("Add series").OnClick(() => viewModel.AddSeries()),
			new Button("Remove series").OnClick(() => viewModel.RemoveSeries()),
			new Switch("Constant changes", isStreaming).OnSwitchToggled(OnConstantChangesClick)
		]);
		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
		};

		Append(wrapGroup);
		Append(cartesianChart);
	}

	private async void OnConstantChangesClick(object sender, System.EventArgs e)
	{
		isStreaming = !isStreaming;
		while (isStreaming)
		{
			viewModel.RemoveItem();
			viewModel.AddItem();
			await Task.Delay(1000);
		}
	}

	public override void Dispose()
	{
		isStreaming = false;
		wrapGroup.Dispose();
		cartesianChart.Dispose();
		base.Dispose();
	}
}
