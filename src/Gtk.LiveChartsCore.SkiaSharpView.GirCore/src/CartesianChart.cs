// Copyright (c) Ferenc Czirok
// Licensed under the MIT License.
// See the LICENSE file in the root for more information.

using LiveChartsCore;
using LiveChartsCore.Behaviours.Events;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.VisualElements;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Versioning;

namespace Gtk.LiveChartsCore.SkiaSharpView.GirCore;

/// <inheritdoc cref="ICartesianChartView" />
[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public class CartesianChart : BaseChart, ICartesianChartView
{
	#region ICartesianChartView fields

	private IEnumerable<ISeries> _series = [];
	private IEnumerable<ICartesianAxis> _xAxes = [];
	private IEnumerable<ICartesianAxis> _yAxes = [];
	private IEnumerable<CoreSection> _sections = [];
	private CoreDrawMarginFrame? _drawMarginFrame;
	private FindingStrategy _findingStrategy = LiveCharts.DefaultSettings.FindingStrategy;
	private bool _matchAxesScreenDataRatio;

	#endregion

	#region ICartesianChartView properties

	public CartesianChartEngine Core => CoreChart == null ? throw new Exception("core not found") : (CartesianChartEngine)CoreChart;

	/// <inheritdoc cref="ICartesianChartView.Series" />
	public IEnumerable<ISeries> Series
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

	/// <inheritdoc cref="ICartesianChartView.XAxes" />
	public IEnumerable<ICartesianAxis> XAxes
	{
		get => _xAxes;
		set
		{
			XObserver?.Dispose(_xAxes);
			XObserver?.Initialize(value);
			if (CoreChart is null) return;
			_xAxes = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="ICartesianChartView.YAxes" />
	public IEnumerable<ICartesianAxis> YAxes
	{
		get => _yAxes;
		set
		{
			YObserver?.Dispose(_yAxes);
			YObserver?.Initialize(value);
			if (CoreChart is null) return;
			_yAxes = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="ICartesianChartView.Sections" />
	public IEnumerable<CoreSection> Sections
	{
		get => _sections;
		set
		{
			SectionsObserver?.Dispose(_sections);
			SectionsObserver?.Initialize(value);
			if (CoreChart is null) return;
			_sections = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="ICartesianChartView.DrawMarginFrame" />
	public CoreDrawMarginFrame? DrawMarginFrame
	{
		get => _drawMarginFrame;
		set
		{
			_drawMarginFrame = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="ICartesianChartView.ZoomMode" />
	public ZoomAndPanMode ZoomMode { get; set; } = LiveCharts.DefaultSettings.ZoomMode;

	/// <inheritdoc cref="ICartesianChartView.ZoomingSpeed" />
	public double ZoomingSpeed { get; set; } = LiveCharts.DefaultSettings.ZoomSpeed;

#pragma warning disable CS0618 // Type or member is obsolete
	private static readonly MethodInfo HackAsOld = typeof(FindingStrategy).GetMethod("AsOld", BindingFlags.NonPublic | BindingFlags.Instance)!;

	private static readonly MethodInfo HackAsNew = typeof(TooltipFindingStrategy).GetMethod("AsNew", BindingFlags.NonPublic | BindingFlags.Instance)!;
#pragma warning restore CS0618 // Type or member is obsolete

	/// <inheritdoc cref="ICartesianChartView.FindingStrategy" />
	[Obsolete($"Renamed to {nameof(FindingStrategy)}")]
	public TooltipFindingStrategy TooltipFindingStrategy
	{
		get => (TooltipFindingStrategy)HackAsOld?.Invoke(FindingStrategy, null)!;
		set => FindingStrategy = (FindingStrategy)HackAsNew.Invoke(value, null)!;
	}

	/// <inheritdoc cref="ICartesianChartView.FindingStrategy" />
	public FindingStrategy FindingStrategy { get => _findingStrategy; set { _findingStrategy = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="ICartesianChartView.MatchAxesScreenDataRatio" />
	public bool MatchAxesScreenDataRatio
	{
		get => _matchAxesScreenDataRatio;
		set
		{
			_matchAxesScreenDataRatio = value;
			OnMatchAxesScreenDataRatioChanged();
		}
	}

	private void OnMatchAxesScreenDataRatioChanged()
	{
		if (CoreChart is null) return;
		if (MatchAxesScreenDataRatio) SharedAxes.MatchAxesScreenDataRatio(this);
		else SharedAxes.DisposeMatchAxesScreenDataRatio(this);
	}

	#endregion

	#region IChartView methods

	/// <inheritdoc cref="ICartesianChartView.ScalePixelsToData(LvcPointD, int, int)"/>
	public LvcPointD ScalePixelsToData(LvcPointD point, int xAxisIndex = 0, int yAxisIndex = 0)
	{
		if (CoreChart is not CartesianChartEngine cc) throw new Exception("core not found");
		var xScaler = new Scaler(cc.DrawMarginLocation, cc.DrawMarginSize, cc.XAxes[xAxisIndex]);
		var yScaler = new Scaler(cc.DrawMarginLocation, cc.DrawMarginSize, cc.YAxes[yAxisIndex]);

		return new LvcPointD { X = xScaler.ToChartValues(point.X), Y = yScaler.ToChartValues(point.Y) };
	}

	/// <inheritdoc cref="ICartesianChartView.ScaleDataToPixels(LvcPointD, int, int)"/>
	public LvcPointD ScaleDataToPixels(LvcPointD point, int xAxisIndex = 0, int yAxisIndex = 0)
	{
		if (CoreChart is not CartesianChartEngine cc) throw new Exception("core not found");

		var xScaler = new Scaler(cc.DrawMarginLocation, cc.DrawMarginSize, cc.XAxes[xAxisIndex]);
		var yScaler = new Scaler(cc.DrawMarginLocation, cc.DrawMarginSize, cc.YAxes[yAxisIndex]);

		return new LvcPointD { X = xScaler.ToPixels(point.X), Y = yScaler.ToPixels(point.Y) };
	}

	#endregion

	protected readonly CollectionDeepObserver<ISeries> SeriesObserver;
	protected readonly CollectionDeepObserver<ICartesianAxis> XObserver;
	protected readonly CollectionDeepObserver<ICartesianAxis> YObserver;
	protected readonly CollectionDeepObserver<CoreSection> SectionsObserver;

	public CartesianChart()
	{
		LiveCharts.Configure(config => config.UseDefaults());

		SeriesObserver = new CollectionDeepObserver<ISeries>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		XObserver = new CollectionDeepObserver<ICartesianAxis>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		YObserver = new CollectionDeepObserver<ICartesianAxis>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		SectionsObserver = new CollectionDeepObserver<CoreSection>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		VisualsObserver = new CollectionDeepObserver<ChartElement>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		OnInitialize();

		XAxes = [GetProvider().GetDefaultCartesianAxis()];
		YAxes = [GetProvider().GetDefaultCartesianAxis()];
		Series = new ObservableCollection<ISeries>();
		VisualElements = new ObservableCollection<ChartElement>();
		SyncContext = new object();
	}

	#region Chart overrides

	public override IEnumerable<ChartPoint> GetPointsAt(LvcPointD point, FindingStrategy strategy = FindingStrategy.Automatic, FindPointFor findPointFor = FindPointFor.HoverEvent)
	{
		if (CoreChart is not CartesianChartEngine coreChart) throw new Exception("core not found");

		if (strategy == FindingStrategy.Automatic)
			strategy = coreChart.Series.GetFindingStrategy();

		return coreChart.Series.SelectMany(series => series.FindHitPoints(coreChart, new(point), strategy, findPointFor));
	}

	public override IEnumerable<IChartElement> GetVisualsAt(LvcPointD point)
	{
		return CoreChart is not CartesianChartEngine coreChart
			? throw new Exception("core not found")
			: coreChart.VisualElements.SelectMany(visual => ((VisualElement)visual).InvokeIsHitBy(coreChart, new(point)));
	}

	#endregion

	protected override void OnInitialize()
	{
		if (CoreChart is null)
		{
			CoreChart = new CartesianChartEngine(this, config => config.UseDefaults(), CoreCanvas);

			if (SyncContext != null)
				CoreCanvas.SetSync(SyncContext);

			CoreChart.Update();

			if (CoreChart == null) throw new Exception("Core not found!");

			CoreChart.Measuring += OnCoreMeasuring;
			CoreChart.UpdateStarted += OnCoreUpdateStarted;
			CoreChart.UpdateFinished += OnCoreUpdateFinished;

			OnResize += OnResized;

			var chartBehaviour = new ChartBehaviour();

			chartBehaviour.Pressed += OnPressed;
			chartBehaviour.Update += OnMoved;
			chartBehaviour.Motion += OnMoved;
			chartBehaviour.Release += OnReleased;
			chartBehaviour.Leave += OnExited;

			chartBehaviour.Scroll += OnScrolled;
			chartBehaviour.ScaleChanged += OnPinched;

			chartBehaviour.On(this);
		}

		CoreChart.Load();
		CoreChart.Update();
	}

	private void OnScrolled(object? sender, ScrollEventArgs args)
	{
		if (CoreChart is null) return;

		var c = (CartesianChartEngine)CoreChart;
		c.Zoom(args.Location, args.ScrollDelta > 0 ? ZoomDirection.ZoomIn : ZoomDirection.ZoomOut);
	}


	private void OnPinched(object? sender, PinchEventArgs args)
	{
		if (CoreChart is null) return;

		var c = (CartesianChartEngine)CoreChart;
		var p = args.PinchStart;
		var s = c.ControlSize;
		var pivot = new LvcPoint((float)(p.X * s.Width), (float)(p.Y * s.Height));
		c.Zoom(pivot, ZoomDirection.DefinedByScaleFactor, args.Scale, true);
	}

	public override void Dispose()
	{
		SeriesObserver?.Dispose(_series);
		XObserver?.Dispose(_xAxes);
		YObserver?.Dispose(_yAxes);
		SectionsObserver?.Dispose(_sections);
		base.Dispose();
	}
}
