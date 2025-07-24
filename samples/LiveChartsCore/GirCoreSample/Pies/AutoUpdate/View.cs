using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.AutoUpdate;

namespace GirCoreSample.Pies.AutoUpdate;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly WrapPreferencesGroup wrapGroup;
	private readonly PieChart pieChart;
	private readonly ViewModel viewModel;
	private bool isStreaming = false;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		viewModel = new ViewModel();
		wrapGroup = new WrapPreferencesGroup(
		[
			new Button("Add series").OnClick(() => viewModel.AddSeries()),
			new Button("Remove series").OnClick(() => viewModel.RemoveSeries()),
			new Button("Update all").OnClick(() => viewModel.UpdateAll()),
			new Switch("Constant changes", isStreaming).OnSwitchToggled(OnConstantChangesClick)
		]);
		pieChart = new PieChart
		{
			Series = viewModel.Series,
		};

		Append(wrapGroup);
		Append(pieChart);
	}

	private async void OnConstantChangesClick(object sender, System.EventArgs e)
	{
		isStreaming = !isStreaming;
		while (isStreaming)
		{
			viewModel.RemoveSeries();
			viewModel.AddSeries();
			await Task.Delay(1000);
		}
	}

	public override void Dispose()
	{
		isStreaming = false;
		wrapGroup.Dispose();
		pieChart.Dispose();
		base.Dispose();
	}
}
