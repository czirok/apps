using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using LiveChartsCore.Kernel.Events;
using System.Runtime.Versioning;
using ViewModelsSamples.General.Scrollable;

namespace GirCoreSample.General.Scrollable;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly CartesianChart cartesianChart1;
	private readonly CartesianChart cartesianChart2;

	private readonly ViewModel _viewModel = new();
	public View()
	{
		var viewModel = _viewModel;

		cartesianChart1 = new CartesianChart
		{
			Series = viewModel.Series,
			XAxes = viewModel.ScrollableAxes,
			ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
			DrawMargin = viewModel.Margin,
			UpdateStartedCommand = new Command<ChartCommandArgs>(viewModel.ChartUpdated),
		};

		cartesianChart2 = new CartesianChart
		{
			Series = viewModel.ScrollbarSeries,
			DrawMargin = viewModel.Margin,
			Sections = viewModel.Thumbs,
			XAxes = viewModel.InvisibleX,
			YAxes = viewModel.InvisibleY,
			TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Hidden,
			PointerPressedCommand = new Command<PointerCommandArgs>(viewModel.PointerDown),
			PointerMoveCommand = new Command<PointerCommandArgs>(viewModel.PointerMove),
			PointerReleasedCommand = new Command<PointerCommandArgs>(viewModel.PointerUp),
		};

		var layout = Gtk.ConstraintLayout.New();
		SetLayoutManager(layout);

		// Add the charts to the layout
		cartesianChart1.SetParent(this);
		cartesianChart2.SetParent(this);

		// Chart1 position - full width, at the top
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart1, Gtk.ConstraintAttribute.Start, Gtk.ConstraintRelation.Eq, this, Gtk.ConstraintAttribute.Start, 1.0, 0.0, (int)Gtk.ConstraintStrength.Required));
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart1, Gtk.ConstraintAttribute.End, Gtk.ConstraintRelation.Eq, this, Gtk.ConstraintAttribute.End, 1.0, 0.0, (int)Gtk.ConstraintStrength.Required));
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart1, Gtk.ConstraintAttribute.Top, Gtk.ConstraintRelation.Eq, this, Gtk.ConstraintAttribute.Top, 1.0, 0.0, (int)Gtk.ConstraintStrength.Required));

		// Chart2 position - full width, at the bottom
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart2, Gtk.ConstraintAttribute.Start, Gtk.ConstraintRelation.Eq, this, Gtk.ConstraintAttribute.Start, 1.0, 0.0, (int)Gtk.ConstraintStrength.Required));
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart2, Gtk.ConstraintAttribute.End, Gtk.ConstraintRelation.Eq, this, Gtk.ConstraintAttribute.End, 1.0, 0.0, (int)Gtk.ConstraintStrength.Required));
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart2, Gtk.ConstraintAttribute.Bottom, Gtk.ConstraintRelation.Eq, this, Gtk.ConstraintAttribute.Bottom, 1.0, 0.0, (int)Gtk.ConstraintStrength.Required));

		// Vertical spacing - chart1 height = chart2 height
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart1, Gtk.ConstraintAttribute.Bottom, Gtk.ConstraintRelation.Eq, cartesianChart2, Gtk.ConstraintAttribute.Top, 1.0, 0.0, (int)Gtk.ConstraintStrength.Required));

		// 80%-20% height distribution
		layout.AddConstraint(Gtk.Constraint.New(cartesianChart1, Gtk.ConstraintAttribute.Height, Gtk.ConstraintRelation.Eq, cartesianChart2, Gtk.ConstraintAttribute.Height, 5.0, 0.0, (int)Gtk.ConstraintStrength.Required));
	}

	public override void Dispose()
	{
		cartesianChart1?.Dispose();
		cartesianChart2?.Dispose();
		base.Dispose();
	}
}
