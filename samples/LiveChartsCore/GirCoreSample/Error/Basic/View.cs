using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Error.Basic;

namespace GirCoreSample.Error.Basic;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly CartesianChart cartesianChart1;
	private readonly CartesianChart cartesianChart2;
	private readonly CartesianChart cartesianChart3;

	public View()
	{
		var viewModel = new ViewModel();
		cartesianChart1 = new CartesianChart
		{
			Series = viewModel.Series0,
		};
		cartesianChart2 = new CartesianChart
		{
			Series = viewModel.Series1,
		};
		cartesianChart3 = new CartesianChart
		{
			Series = viewModel.Series2,
			XAxes = viewModel.DateTimeAxis
		};

		var box = Gtk.Box.New(Gtk.Orientation.Vertical, 10);
		box.Append(cartesianChart1);
		box.Append(cartesianChart2);
		box.Append(cartesianChart3);

		Child = box;
	}

	public override void Dispose()
	{
		cartesianChart1?.Dispose();
		cartesianChart2?.Dispose();
		cartesianChart3?.Dispose();
		base.Dispose();
	}
}
