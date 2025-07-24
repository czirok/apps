using System.Runtime.Versioning;

namespace GirCoreSample;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal static class AOT
{
	internal static readonly Dictionary<string, Func<Gtk.Widget>> ViewFactories = new()
	{
		["Design.LinearGradients"] = () => new Design.LinearGradients.View(),
		["Design.RadialGradients"] = () => new Design.RadialGradients.View(),
		["Design.StrokeDashArray"] = () => new Design.StrokeDashArray.View(),

		["Lines.Basic"] = () => new Lines.Basic.View(),
		["Lines.AutoUpdate"] = () => new Lines.AutoUpdate.View(),
		["Lines.Straight"] = () => new Lines.Straight.View(),
		["Lines.Properties"] = () => new Lines.Properties.View(),
		["Lines.Area"] = () => new Lines.Area.View(),
		["Lines.Custom"] = () => new Lines.Custom.View(),
		["Lines.CustomPoints"] = () => new Lines.CustomPoints.View(),
		["Lines.Padding"] = () => new Lines.Padding.View(),
		["Lines.XY"] = () => new Lines.XY.View(),
		["Lines.Zoom"] = () => new Lines.Zoom.View(),

		["Bars.Basic"] = () => new Bars.Basic.View(),
		["Bars.AutoUpdate"] = () => new Bars.AutoUpdate.View(),
		["Bars.Custom"] = () => new Bars.Custom.View(),
		["Bars.WithBackground"] = () => new Bars.WithBackground.View(),
		["Bars.Spacing"] = () => new Bars.Spacing.View(),
		["Bars.DelayedAnimation"] = () => new Bars.DelayedAnimation.View(),
		["Bars.Race"] = () => new Bars.Race.View(),
		["Bars.RowsWithLabels"] = () => new Bars.RowsWithLabels.View(),
		["Bars.Layered"] = () => new Bars.Layered.View(),

		["Pies.Basic"] = () => new Pies.Basic.View(),
		["Pies.AutoUpdate"] = () => new Pies.AutoUpdate.View(),
		["Pies.Doughnut"] = () => new Pies.Doughnut.View(),
		["Pies.Pushout"] = () => new Pies.Pushout.View(),
		["Pies.Custom"] = () => new Pies.Custom.View(),
		["Pies.Icons"] = () => new Pies.Icons.View(),
		["Pies.OutLabels"] = () => new Pies.OutLabels.View(),
		["Pies.NightingaleRose"] = () => new Pies.NightingaleRose.View(),
		["Pies.Gauges"] = () => new Pies.Gauges.View(),
		["Pies.Gauge1"] = () => new Pies.Gauge1.View(),
		["Pies.Gauge2"] = () => new Pies.Gauge2.View(),
		["Pies.Gauge3"] = () => new Pies.Gauge3.View(),
		["Pies.Gauge4"] = () => new Pies.Gauge4.View(),
		["Pies.Gauge5"] = () => new Pies.Gauge5.View(),
		["Pies.AngularGauge"] = () => new Pies.AngularGauge.View(),

		["Scatter.Basic"] = () => new Scatter.Basic.View(),
		["Scatter.Bubbles"] = () => new Scatter.Bubbles.View(),
		["Scatter.AutoUpdate"] = () => new Scatter.AutoUpdate.View(),
		["Scatter.Custom"] = () => new Scatter.Custom.View(),

		["StackedArea.Basic"] = () => new StackedArea.Basic.View(),
		["StackedArea.StepArea"] = () => new StackedArea.StepArea.View(),
		["StackedBars.Basic"] = () => new StackedBars.Basic.View(),
		["StackedBars.Groups"] = () => new StackedBars.Groups.View(),

		["Financial.BasicCandlesticks"] = () => new Financial.BasicCandlesticks.View(),

		["Error.Basic"] = () => new Error.Basic.View(),

		["Box.Basic"] = () => new Box.Basic.View(),

		["Heat.Basic"] = () => new Heat.Basic.View(),

		["StepLines.Basic"] = () => new StepLines.Basic.View(),
		["StepLines.AutoUpdate"] = () => new StepLines.AutoUpdate.View(),
		["StepLines.Properties"] = () => new StepLines.Properties.View(),
		["StepLines.Area"] = () => new StepLines.Area.View(),
		["StepLines.Custom"] = () => new StepLines.Custom.View(),
		["StepLines.Zoom"] = () => new StepLines.Zoom.View(),

		["Polar.Basic"] = () => new Polar.Basic.View(),
		["Polar.RadialArea"] = () => new Polar.RadialArea.View(),
		["Polar.Coordinates"] = () => new Polar.Coordinates.View(),

		["Axes.LabelsFormat"] = () => new Axes.LabelsFormat.View(),
		["Axes.LabelsFormat2"] = () => new Axes.LabelsFormat2.View(),
		["Axes.NamedLabels"] = () => new Axes.NamedLabels.View(),
		["Axes.LabelsRotation"] = () => new Axes.LabelsRotation.View(),
		["Axes.Multiple"] = () => new Axes.Multiple.View(),
		["Axes.Shared"] = () => new Axes.Shared.View(),
		["Axes.ColorsAndPosition"] = () => new Axes.ColorsAndPosition.View(),
		["Axes.Crosshairs"] = () => new Axes.Crosshairs.View(),
		["Axes.CustomSeparatorsInterval"] = () => new Axes.CustomSeparatorsInterval.View(),
		["Axes.DateTimeScaled"] = () => new Axes.DateTimeScaled.View(),
		["Axes.TimeSpanScaled"] = () => new Axes.TimeSpanScaled.View(),
		["Axes.Logarithmic"] = () => new Axes.Logarithmic.View(),
		["Axes.Style"] = () => new Axes.Style.View(),
		["Axes.MatchScale"] = () => new Axes.MatchScale.View(),
		["Axes.Paging"] = () => new Axes.Paging.View(),

		["Events.Tutorial"] = () => new Events.Tutorial.View(),
		["Events.AddPointOnClick"] = () => new Events.AddPointOnClick.View(),
		["Events.Cartesian"] = () => new Events.Cartesian.View(),
		["Events.Pie"] = () => new Events.Pie.View(),
		["Events.Polar"] = () => new Events.Polar.View(),

		["General.MapPoints"] = () => new General.MapPoints.View(),
		["General.RealTime"] = () => new General.RealTime.View(),
		["General.Scrollable"] = () => new General.Scrollable.View(),
		["General.Sections"] = () => new General.Sections.View(),
		["General.Sections2"] = () => new General.Sections2.View(),
		["General.ConditionalDraw"] = () => new General.ConditionalDraw.View(),
		["General.VisualElements"] = () => new General.VisualElements.View(),
		["General.ChartToImage"] = () => new General.ChartToImage.View(),
		["General.Tooltips"] = () => new General.Tooltips.View(),
		["General.Legends"] = () => new General.Legends.View(),
		["General.Animations"] = () => new General.Animations.View(),
		["General.Visibility"] = () => new General.Visibility.View(),
		["General.TemplatedTooltips"] = () => new General.TemplatedTooltips.View(),
		["General.TemplatedLegends"] = () => new General.TemplatedLegends.View(),
		["General.UserDefinedTypes"] = () => new General.UserDefinedTypes.View(),
		["General.NullPoints"] = () => new General.NullPoints.View(),
		["General.MultiThreading"] = () => new General.MultiThreading.View(),
		["General.MultiThreading2"] = () => new General.MultiThreading2.View(),
		["General.DrawOnCanvas"] = () => new General.DrawOnCanvas.View(),

		["VisualTest.TwoChartsOneSeries"] = () => new VisualTest.TwoChartsOneSeries.View(),
		["VisualTest.ReattachVisual"] = () => new VisualTest.ReattachVisual.View(),
		["VisualTest.Tabs"] = () => new VisualTest.Tabs.View(),

		["Test.ChangeSeriesInstance"] = () => new Test.ChangeSeriesInstance.View(),
		["Test.Dispose"] = () => new Test.Dispose.View(),
		["Test.MotionCanvasDispose"] = () => new Test.MotionCanvasDispose.View(),

		["Maps.World"] = () => new Maps.World.View(),
	};

}