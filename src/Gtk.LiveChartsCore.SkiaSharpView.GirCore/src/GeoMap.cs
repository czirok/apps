// Copyright (c) Ferenc Czirok
// Licensed under the MIT License.
// See the LICENSE file in the root for more information.

using LiveChartsCore;
using LiveChartsCore.Behaviours.Events;
using LiveChartsCore.Drawing;
using LiveChartsCore.Geo;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.Motion;
using LiveChartsCore.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Versioning;
using EventArgs = LiveChartsCore.Behaviours.Events.EventArgs;

namespace Gtk.LiveChartsCore.SkiaSharpView.GirCore;

/// <inheritdoc cref="IPieChartView" />
[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public class GeoMap : MotionCanvas, IGeoMapView
{
	private GeoMapChart CoreChart { get; set; } = default!;

	#region IGeoMapView fields

	private DrawnMap _activeMap = default!;
	private object? _viewCommand;
	private MapProjection _mapProjection = MapProjection.Default;
	private IEnumerable<IGeoSeries> _series = default!;
	private Paint? _stroke;
	private Paint? _fill;

	#endregion

	#region IGeoMapView properties

	/// <inheritdoc cref="IGeoMapView.ActiveMap" />
	public DrawnMap ActiveMap { get => _activeMap; set { _activeMap = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IGeoMapView.Width" />
	public float Width => GetWidth();

	/// <inheritdoc cref="IGeoMapView.Height" />
	public float Height => GetHeight();

	/// <inheritdoc cref="IGeoMapView.SyncContext" />
	public object SyncContext { get; set; } = new();

	/// <inheritdoc cref="IGeoMapView.ViewCommand" />
	public object? ViewCommand
	{
		get => _viewCommand;
		set
		{
			_viewCommand = value;
			CoreChart.ViewTo(_viewCommand);
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IGeoMapView.MapProjection" />
	public MapProjection MapProjection { get => _mapProjection; set { _mapProjection = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IGeoMapView.Series" />
	public IEnumerable<IGeoSeries> Series
	{
		get => _series;
		set
		{
			SeriesObserver?.Dispose(_series);
			SeriesObserver?.Initialize(value);
			if (CoreChart is null) return;
			_series = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IGeoMapView.Stroke" />
	public Paint? Stroke
	{
		get => _stroke;
		set
		{
			value?.SetPaintStyle(PaintStyle.Stroke);
			_stroke = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IGeoMapView.Fill" />
	public Paint? Fill
	{
		get => _fill;
		set
		{
			value?.SetPaintStyle(PaintStyle.Fill);
			_fill = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IGeoMapView.AutoUpdateEnabled" />
	public bool AutoUpdateEnabled { get; set; } = true;

	/// <inheritdoc cref="IGeoMapView.DesignerMode" />
	public bool DesignerMode => false;

	#endregion

	protected readonly CollectionDeepObserver<IGeoSeries> SeriesObserver;

	public CoreMotionCanvas Canvas => CoreCanvas;

	/// <summary>
	/// Initializes a new instance of the <see cref="GeoMap"/> class.
	/// </summary> 
	public GeoMap()
	{
		_stroke = new SolidColorPaint(new SKColor(255, 255, 255, 255));
		_stroke.SetPaintStyle(PaintStyle.Stroke);
		_fill = new SolidColorPaint(new SKColor(240, 240, 240, 255));
		_fill.SetPaintStyle(PaintStyle.Fill);

		LiveCharts.Configure(config => config.UseDefaults());

		CoreChart = new GeoMapChart(this);

		var chartBehaviour = new ChartBehaviour();

		chartBehaviour.Pressed += OnPressed;
		chartBehaviour.Update += OnMotion;
		chartBehaviour.Motion += OnMotion;
		chartBehaviour.Release += OnReleased;
		chartBehaviour.Leave += OnLeave;

		chartBehaviour.Scroll += OnScroll;
		chartBehaviour.ScaleChanged += OnScaleChanged;

		chartBehaviour.On(this);

		OnResize += OnResized;

		SeriesObserver = new CollectionDeepObserver<IGeoSeries>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		// Series = [];
		ActiveMap = Maps.GetWorldMap();
		// SyncContext = new object();
	}

	protected override void OnInitialize() { }

	protected void OnResized(Gtk.DrawingArea sender, Gtk.DrawingArea.ResizeSignalArgs args)
	{
		if (CoreChart is null) return;
		OnPropertyChanged();
	}

	private void OnPressed(object? sender, PressedEventArgs args)
	{
		CoreChart.InvokePressed(args.Location);
	}

	private void OnMotion(object? sender, ScreenEventArgs args)
	{
		CoreChart.InvokeMotion(args.Location);
	}

	private void OnReleased(object? sender, PressedEventArgs args)
	{
		CoreChart.InvokeReleased(args.Location);
	}

	private void OnLeave(object? sender, EventArgs args)
	{
		CoreChart.InvokeLeave();
	}

	private void OnScroll(object? sender, ScrollEventArgs args)
	{
		if (CoreChart is null) return;

		CoreChart.ViewTo(
			new ZoomOnPointerView(args.Location,
				args.ScrollDelta > 0 ? ZoomDirection.ZoomIn : ZoomDirection.ZoomOut));
	}

	private void OnScaleChanged(object? sender, PinchEventArgs args)
	{
		if (CoreChart is null) return;
		var p = args.PinchStart;
		if (GetBounds(out var x, out var y, out var width, out var height))
		{
			CoreChart.ViewTo(
				new ZoomOnPointerView(
					new LvcPoint((float)(p.X * width), (float)(p.Y * height)),
					ZoomDirection.DefinedByScaleFactor));
		}
	}

	/// <summary>
	/// Called when a property changes.
	/// </summary>
	protected void OnPropertyChanged() => CoreChart?.Update();

	public override void Dispose()
	{
		SeriesObserver?.Dispose(_series);
		CoreChart?.Unload();
		base.Dispose();
	}
}

public static class GeoMapExtensions
{
	private static Action<GeoMapChart, LvcPoint>? _cachedPressedDelegate;
	private static Action<GeoMapChart, LvcPoint>? _cachedReleasedDelegate;
	private static Action<GeoMapChart, LvcPoint>? _cachedMotionDelegate;
	private static Action<GeoMapChart>? _cachedLeaveDelegate;

	public static void InvokePressed(this GeoMapChart chart, LvcPoint point)
	{
		if (_cachedPressedDelegate == null)
		{
			var method = typeof(GeoMapChart).GetMethod("InvokePointerDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedPressedDelegate = (Action<GeoMapChart, LvcPoint>)
				Delegate.CreateDelegate(typeof(Action<GeoMapChart, LvcPoint>), method);
		}
		_cachedPressedDelegate(chart, point);
	}

	public static void InvokeReleased(this GeoMapChart chart, LvcPoint point)
	{
		if (_cachedReleasedDelegate == null)
		{
			var method = typeof(GeoMapChart).GetMethod("InvokePointerUp", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedReleasedDelegate = (Action<GeoMapChart, LvcPoint>)
				Delegate.CreateDelegate(typeof(Action<GeoMapChart, LvcPoint>), method);
		}
		_cachedReleasedDelegate(chart, point);
	}

	public static void InvokeMotion(this GeoMapChart chart, LvcPoint point)
	{
		if (_cachedMotionDelegate == null)
		{
			var method = typeof(GeoMapChart).GetMethod("InvokePointerMove", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedMotionDelegate = (Action<GeoMapChart, LvcPoint>)
				Delegate.CreateDelegate(typeof(Action<GeoMapChart, LvcPoint>), method);
		}
		_cachedMotionDelegate(chart, point);
	}

	public static void InvokeLeave(this GeoMapChart chart)
	{
		if (_cachedLeaveDelegate == null)
		{
			var method = typeof(GeoMapChart).GetMethod("InvokePointerLeft", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedLeaveDelegate = (Action<GeoMapChart>)
				Delegate.CreateDelegate(typeof(Action<GeoMapChart>), method);
		}
		_cachedLeaveDelegate(chart);
	}
}

public static class PaintExtensions
{
	private static Func<Paint, PaintStyle>? _cachedPaintStyleGetDelegate;
	private static Action<Paint, PaintStyle>? _cachedPaintStyleSetDelegate;

	public static PaintStyle GetPaintStyle(this Paint paint)
	{
		if (_cachedPaintStyleGetDelegate == null)
		{
			var property = typeof(Paint).GetProperty("PaintStyle", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedPaintStyleGetDelegate = (Func<Paint, PaintStyle>)
				Delegate.CreateDelegate(typeof(Func<Paint, PaintStyle>), property.GetMethod!);
		}
		return _cachedPaintStyleGetDelegate(paint);
	}

	public static void SetPaintStyle(this Paint paint, PaintStyle value)
	{
		if (_cachedPaintStyleSetDelegate == null)
		{
			var property = typeof(Paint).GetProperty("PaintStyle", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedPaintStyleSetDelegate = (Action<Paint, PaintStyle>)
				Delegate.CreateDelegate(typeof(Action<Paint, PaintStyle>), property.SetMethod!);
		}
		_cachedPaintStyleSetDelegate(paint, value);
	}
}