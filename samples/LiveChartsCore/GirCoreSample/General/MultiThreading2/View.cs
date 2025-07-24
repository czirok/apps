using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.General.MultiThreading2;

namespace GirCoreSample.General.MultiThreading2;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly CartesianChart cartesianChart;

	public View()
	{
		cartesianChart = new CartesianChart
		{
			Series = [],
		};

		OnMap += (sender, e) =>
		{
			var viewModel = new ViewModel(InvokeOnUIThread);
			cartesianChart.Series = viewModel.Series;
		};

		Child = cartesianChart;
	}

	public void InvokeOnUIThread(Action action)
	{
		GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_DEFAULT, new GLib.SourceFunc(() =>
		{
			action();
			return GLib.Constants.SOURCE_REMOVE;
		}));
	}

	public override void Dispose()
	{
		cartesianChart?.Dispose();
		base.Dispose();
	}
}
