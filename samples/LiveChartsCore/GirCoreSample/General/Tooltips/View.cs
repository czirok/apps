using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore.Measure;
using System.Runtime.Versioning;
using ViewModelsSamples.General.Tooltips;

namespace GirCoreSample.General.Tooltips;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly ViewModel viewModel;
	private readonly CartesianChart cartesianChart;
	private readonly WrapToggle<TooltipPosition> wrapToggle;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);

		viewModel = new ViewModel();

		cartesianChart = new CartesianChart()
		{
			Series = viewModel.Series,
			LegendPosition = LegendPosition.Left,
			TooltipPosition = viewModel.Position
		}.BindTo(viewModel, nameof(ViewModel.Position), nameof(CartesianChart.TooltipPosition));

		wrapToggle = new WrapToggle<TooltipPosition>("TooltipPosition", string.Empty, new Dictionary<TooltipPosition, string>
			{
				{ TooltipPosition.Hidden, "Hidden" },
				{ TooltipPosition.Top, "Top" },
				{ TooltipPosition.Bottom, "Bottom" },
				{ TooltipPosition.Left, "Left" },
				{ TooltipPosition.Right, "Right" },
				{ TooltipPosition.Center, "Center" }
			}, TooltipPosition.Left, TooltipPosition.Top)
			.BindTo(viewModel, nameof(ViewModel.Position));

		Append(wrapToggle.Row);
		Append(cartesianChart);
	}

	public override void Dispose()
	{
		wrapToggle.Dispose();
		cartesianChart.Dispose();
		base.Dispose();
	}
}
