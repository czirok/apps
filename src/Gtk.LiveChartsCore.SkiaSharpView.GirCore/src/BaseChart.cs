// Copyright (c) Ferenc Czirok
// Licensed under the MIT License.
// See the LICENSE file in the root for more information.

using LiveChartsCore;
using LiveChartsCore.Behaviours.Events;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Providers;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.Motion;
using LiveChartsCore.Painting;
using LiveChartsCore.VisualElements;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Input;
using EventArgs = LiveChartsCore.Behaviours.Events.EventArgs;

namespace Gtk.LiveChartsCore.SkiaSharpView.GirCore;

/// <summary>
/// Base class for views that display a chart.
/// </summary>
[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public abstract partial class BaseChart : MotionCanvas, IChartView
{
	#region IChartView fields

	private Margin? _drawMargin = null;
	private LegendPosition _legendPosition = LiveCharts.DefaultSettings.LegendPosition;
	private TooltipPosition _tooltipPosition = LiveCharts.DefaultSettings.TooltipPosition;
	private Paint? _legendTextPaint = (Paint?)LiveCharts.DefaultSettings.LegendTextPaint;
	private Paint? _legendBackgroundPaint = (Paint?)LiveCharts.DefaultSettings.LegendBackgroundPaint;
	private double _legendTextSize = LiveCharts.DefaultSettings.LegendTextSize;
	private Paint? _tooltipTextPaint = (Paint?)LiveCharts.DefaultSettings.TooltipTextPaint;
	private Paint? _tooltipBackgroundPaint = (Paint?)LiveCharts.DefaultSettings.TooltipBackgroundPaint;
	private double _tooltipTextSize = LiveCharts.DefaultSettings.TooltipTextSize;
	private VisualElement? _title;
	private IChartTooltip? _tooltip;
	private IEnumerable<ChartElement> _visuals = [];

	#endregion

	#region IChartView properties

	/// <inheritdoc cref="IChartView.CoreChart" />
	public Chart CoreChart { get; internal set; } = default!;

	/// <inheritdoc cref="IChartView.DesignerMode" />
	public bool DesignerMode => false;

	private Gtk.CssProvider? _backgroundCssProvider;
	private LvcColor _backColor = new();
	/// <inheritdoc cref="IChartView.BackColor" />private Gtk.CssProvider? _backgroundCssProvider;
	LvcColor IChartView.BackColor
	{
		get => _backColor;
		set
		{
			_backColor = value;

			if (_backgroundCssProvider != null)
			{
				GetStyleContext().RemoveProvider(_backgroundCssProvider);
			}

			var css = $".chart-background {{background-color: rgba({value.R}, {value.G}, {value.B}, {value.A / 255.0});}}";

			_backgroundCssProvider = Gtk.CssProvider.New();
			_backgroundCssProvider.LoadFromString(css);

			AddCssClass("chart-background");
			GetStyleContext().AddProvider(_backgroundCssProvider, Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);
		}
	}

	/// <inheritdoc cref="IChartView.ControlSize" />
	public LvcSize ControlSize => new LvcSize(GetWidth(), GetHeight());

	/// <inheritdoc cref="IChartView.DrawMargin" />            
	public Margin? DrawMargin { get => _drawMargin; set { _drawMargin = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.AnimationsSpeed" />
	public TimeSpan AnimationsSpeed { get; set; } = LiveCharts.DefaultSettings.AnimationsSpeed;

	/// <inheritdoc cref="IChartView.AnimationsSpeed" />
	public Func<float, float>? EasingFunction { get; set; } = LiveCharts.DefaultSettings.EasingFunction;

	/// <inheritdoc cref="IChartView.UpdaterThrottler" /> 
	public TimeSpan UpdaterThrottler { get; set; } = LiveCharts.DefaultSettings.UpdateThrottlingTimeout;

	/// <inheritdoc cref="IChartView.LegendPosition" />
	public LegendPosition LegendPosition { get => _legendPosition; set { _legendPosition = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.TooltipPosition" />
	public TooltipPosition TooltipPosition { get => _tooltipPosition; set { _tooltipPosition = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.LegendTextPaint" />
	public Paint? LegendTextPaint { get => _legendTextPaint; set { _legendTextPaint = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.LegendBackgroundPaint" />
	public Paint? LegendBackgroundPaint { get => _legendBackgroundPaint; set { _legendBackgroundPaint = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.LegendTextSize" />
	public double LegendTextSize { get => _legendTextSize; set { _legendTextSize = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.TooltipTextPaint" />
	public Paint? TooltipTextPaint { get => _tooltipTextPaint; set { _tooltipTextPaint = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.TooltipBackgroundPaint" />
	public Paint? TooltipBackgroundPaint { get => _tooltipBackgroundPaint; set { _tooltipBackgroundPaint = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.TooltipTextSize" />
	public double TooltipTextSize { get => _tooltipTextSize; set { _tooltipTextSize = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.AutoUpdateEnabled" />
	public bool AutoUpdateEnabled { get; set; } = true;

	/// <inheritdoc cref="IChartView.Title"/>          
	public VisualElement? Title { get => _title; set { _title = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.Legend" />
	public IChartLegend? Legend { get; set; }

	/// <inheritdoc cref="IChartView.Tooltip" />        
	public IChartTooltip? Tooltip { get => _tooltip; set { _tooltip = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IChartView.VisualElements" />
	public IEnumerable<ChartElement> VisualElements
	{
		get => _visuals;
		set
		{
			VisualsObserver?.Dispose(_visuals);
			VisualsObserver?.Initialize(value);
			if (CoreChart == null) return;
			_visuals = value;
			OnPropertyChanged();
		}
	}

	#endregion

	#region IChartView events

	/// <inheritdoc cref="IChartView.DataPointerDown" />
	public event ChartPointsHandler? DataPointerDown;

	/// <inheritdoc cref="IChartView.HoveredPointsChanged" />
	public event ChartPointHoverHandler? HoveredPointsChanged;

	/// <inheritdoc cref="IChartView.ChartPointPointerDown" />
	[Obsolete($"Use the {nameof(DataPointerDown)} event instead with a {nameof(FindingStrategy)} that used TakeClosest.")]
	public event ChartPointHandler? ChartPointPointerDown;

	/// <inheritdoc cref="IChartView.Measuring" />
	public event ChartEventHandler? Measuring;

	/// <inheritdoc cref="IChartView.UpdateStarted" />
	public event ChartEventHandler? UpdateStarted;

	/// <inheritdoc cref="IChartView.UpdateFinished" />
	public event ChartEventHandler? UpdateFinished;

	/// <inheritdoc cref="IChartView.VisualElementsPointerDown" />
	public event VisualElementsHandler? VisualElementsPointerDown;

	#endregion

	#region IChartView methods

	/// <inheritdoc cref="IChartView.GetPointsAt(LvcPointD, FindingStrategy, FindPointFor)"/>
	public abstract IEnumerable<ChartPoint> GetPointsAt(LvcPointD point, FindingStrategy strategy = FindingStrategy.Automatic, FindPointFor findPointFor = FindPointFor.HoverEvent);

	/// <inheritdoc cref="IChartView.GetVisualsAt(LvcPointD)"/>
	public abstract IEnumerable<IChartElement> GetVisualsAt(LvcPointD point);

	/// <inheritdoc cref="IChartView.OnDataPointerDown(IEnumerable{ChartPoint}, LvcPoint)"/>
	public void OnDataPointerDown(IEnumerable<ChartPoint> points, LvcPoint pointer)
	{
		DataPointerDown?.Invoke(this, points);
		if (DataPointerDownCommand?.CanExecute(points) is true)
			DataPointerDownCommand.Execute(points);

#pragma warning disable CS0618 // Type or member is obsolete
		ChartPointPointerDown?.Invoke(this, points.FindClosestTo(pointer));
		if (ChartPointPointerDownCommand?.CanExecute(points.FindClosestTo(pointer)) is true)
			ChartPointPointerDownCommand?.Execute(points.FindClosestTo(pointer));
#pragma warning restore CS0618 // Type or member is obsolete
	}

	/// <inheritdoc cref="IChartView.OnHoveredPointsChanged(IEnumerable{ChartPoint}?, IEnumerable{ChartPoint}?)"/>
	public void OnHoveredPointsChanged(IEnumerable<ChartPoint>? newPoints, IEnumerable<ChartPoint>? oldPoints)
	{
		HoveredPointsChanged?.Invoke(this, newPoints, oldPoints);

		var args = new HoverCommandArgs(this, newPoints, oldPoints);
		if (HoveredPointsChangedCommand?.CanExecute(args) is true)
			HoveredPointsChangedCommand.Execute(args);
	}

	/// <inheritdoc cref="IChartView.OnVisualElementPointerDown(IEnumerable{IInteractable}, LvcPoint)"/>
	public void OnVisualElementPointerDown(IEnumerable<IInteractable> visualElements, LvcPoint pointer)
	{
		var args = new VisualElementsEventArgs(CoreChart, visualElements, pointer);

		VisualElementsPointerDown?.Invoke(this, args);

		if (VisualElementsPointerDownCommand?.CanExecute(args) is true)
			VisualElementsPointerDownCommand.Execute(args);
	}


	/// <inheritdoc cref="IChartView.SyncContext" />
	public object SyncContext
	{
		get => CoreCanvas.Sync;
		set
		{
			if (CoreChart is null) return;
			CoreCanvas.SetSync(value);
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IChartView.Invalidate()"/>
	public new void Invalidate()
	{
		// base.Invalidate();
		CoreCanvas.Invalidate();
	}

	#endregion

	/// <summary>
	/// Gets or sets a command to execute when the chart update started.
	/// </summary>
	public ICommand? UpdateStartedCommand { get; set; }

	/// <summary>
	/// Gets or sets a command to execute when the pointer is pressed on the chart.
	/// </summary>
	public ICommand? PointerPressedCommand { get; set; }

	/// <summary>
	/// Gets or sets a command to execute when the pointer is released on the chart.
	/// </summary>
	public ICommand? PointerReleasedCommand { get; set; }

	/// <summary>
	/// Gets or sets a command to execute when the pointer moves over the chart.
	/// </summary>
	public ICommand? PointerMoveCommand { get; set; }

	/// <summary>
	/// Gets or sets a command to execute when the pointer goes down on a data or data points.
	/// </summary>
	public ICommand? DataPointerDownCommand { get; set; }

	/// <summary>
	/// Gets or sets a command to execute when the hovered points change.
	/// </summary>
	public ICommand? HoveredPointsChangedCommand { get; set; }

	/// <summary>
	/// Gets or sets a command to execute when the pointer goes down on a chart point.
	/// </summary>
	[Obsolete($"Use the {nameof(DataPointerDown)} event instead with a {nameof(FindingStrategy)} that used TakeClosest.")]
	public ICommand? ChartPointPointerDownCommand { get; set; }

	/// <summary>
	/// Gets or sets a command to execute when the pointer goes down on a chart point.
	/// </summary>
	public ICommand? VisualElementsPointerDownCommand { get; set; }

	public CoreMotionCanvas MotionCanvas => CoreCanvas;

	protected CollectionDeepObserver<ChartElement> VisualsObserver = default!;

	protected void OnResized(Gtk.DrawingArea sender, Gtk.DrawingArea.ResizeSignalArgs args)
	{
		if (CoreChart is null) return;
		OnPropertyChanged();
	}

	protected void OnCoreUpdateFinished(IChartView chartView) => UpdateFinished?.Invoke(chartView);

	protected void OnCoreUpdateStarted(IChartView chartView)
	{
		if (UpdateStartedCommand is not null)
		{
			var args = new ChartCommandArgs(this);
			if (UpdateStartedCommand.CanExecute(args)) UpdateStartedCommand.Execute(args);
		}

		UpdateStarted?.Invoke(chartView);
	}

	protected void OnCoreMeasuring(IChartView chartView) => Measuring?.Invoke(chartView);

	/// <summary>
	/// Called when a property changes.
	/// </summary>
	protected void OnPropertyChanged() => CoreChart?.Update();

	protected void OnPressed(object? sender, PressedEventArgs args)
	{
		var commandArgs = new PointerCommandArgs(this, new(args.Location.X, args.Location.Y), args);
		if (PointerPressedCommand?.CanExecute(commandArgs) is true)
			PointerPressedCommand.Execute(commandArgs);

		CoreChart.InvokePressed(args.Location, args.IsSecondaryPress);
	}

	protected void OnMoved(object? sender, ScreenEventArgs args)
	{
		var commandArgs = new PointerCommandArgs(this, new(args.Location.X, args.Location.Y), args.OriginalEvent);
		if (PointerMoveCommand?.CanExecute(commandArgs) is true)
			PointerMoveCommand.Execute(commandArgs);

		CoreChart.InvokeMotion(args.Location);
	}

	protected void OnReleased(object? sender, PressedEventArgs args)
	{
		var commandArgs = new PointerCommandArgs(this, new(args.Location.X, args.Location.Y), args);
		if (PointerReleasedCommand?.CanExecute(commandArgs) is true)
			PointerReleasedCommand.Execute(commandArgs);

		CoreChart.InvokeReleased(args.Location, args.IsSecondaryPress);
	}

	protected void OnExited(object? sender, EventArgs args)
	{
		CoreChart.InvokeLeave();
	}

	private static readonly MethodInfo GetProviderMethod =
	typeof(LiveChartsSettings).GetMethod("GetProvider",
		BindingFlags.NonPublic | BindingFlags.Instance)!;

	protected static ChartEngine GetProvider()
	{
		var defaultSettings = LiveCharts.DefaultSettings;
		return (ChartEngine)GetProviderMethod.Invoke(defaultSettings, null)!;
	}

	public override void Dispose()
	{
		VisualsObserver?.Dispose(_visuals);
		VisualsObserver = null!;

		_backgroundCssProvider?.Dispose();
		_backgroundCssProvider = null;
		if (CoreChart != null)
		{
			CoreChart.Unload();
			CoreChart = null!;
		}
		base.Dispose();
	}
}

public static class VisualElementExtensions
{
	private static Func<VisualElement, Chart, LvcPoint, IEnumerable<VisualElement>>? _cachedIsHitByDelegate;

	public static IEnumerable<VisualElement> InvokeIsHitBy(this VisualElement visualElement, Chart chart, LvcPoint point)
	{
		if (_cachedIsHitByDelegate == null)
		{
			var method = typeof(VisualElement).GetMethod("IsHitBy", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedIsHitByDelegate = (Func<VisualElement, Chart, LvcPoint, IEnumerable<VisualElement>>)
				Delegate.CreateDelegate(typeof(Func<VisualElement, Chart, LvcPoint, IEnumerable<VisualElement>>), method);
		}
		return _cachedIsHitByDelegate(visualElement, chart, point);
	}
}

public static class CoreMotionCanvasExtensions
{
	private static PropertyInfo? _cachedSyncProperty;

	public static object GetSync(this CoreMotionCanvas canvas)
	{
		return canvas.Sync;
	}

	public static void SetSync(this CoreMotionCanvas canvas, object value)
	{
		if (_cachedSyncProperty == null)
		{
			_cachedSyncProperty = typeof(CoreMotionCanvas).GetProperty("Sync",
				BindingFlags.Public | BindingFlags.Instance);
		}
		_cachedSyncProperty?.SetValue(canvas, value);
	}
}