using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore.Measure;
using System.Runtime.Versioning;
using ViewModelsSamples.General.Legends;

namespace GirCoreSample.General.Legends;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly CartesianChart cartesianChart;
	private readonly WrapToggle<LegendPosition>? wrapToggle;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);

		var viewModel = new ViewModel();
		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
			LegendPosition = LegendPosition.Right,
		}.BindTo(viewModel, nameof(viewModel.Position), nameof(cartesianChart.LegendPosition));

		wrapToggle = new WrapToggle<LegendPosition>("LegendPosition", string.Empty, new Dictionary<LegendPosition, string>
			{
				{ LegendPosition.Hidden, "Hidden" },
				{ LegendPosition.Top, "Top" },
				{ LegendPosition.Bottom, "Bottom" },
				{ LegendPosition.Left, "Left" },
				{ LegendPosition.Right, "Right" }
			}, LegendPosition.Left, LegendPosition.Top)
				.BindTo(viewModel, nameof(viewModel.Position));

		Append(wrapToggle.Row);
		Append(cartesianChart);
	}

	public override void Dispose()
	{
		cartesianChart?.Dispose();
		wrapToggle?.Dispose();
		base.Dispose();
	}
}
