// Copyright (c) Ferenc Czirok
// Licensed under the MIT License.
// See the LICENSE file in the root for more information.

using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.VisualElements;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace Gtk.LiveChartsCore.SkiaSharpView.GirCore;

/// <inheritdoc cref="IPieChartView" />
[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public class PolarChart : BaseChart, IPolarChartView
{
	#region IPolarChartView fields

	private bool _fitToBounds = false;
	private double _totalAngle = 360;
	private double _innerRadius = 0.0;
	private double _initialRotation = LiveCharts.DefaultSettings.PolarInitialRotation;

	private IEnumerable<IPolarAxis> _anglesAxes = [];

	private IEnumerable<IPolarAxis> _radiusAxes = [];

	private IEnumerable<ISeries> _series = [];

	#endregion

	#region IPolarChartView properties

	/// <inheritdoc cref="IPolarChartView.FitToBounds" />
	public PolarChartEngine Core => CoreChart == null ? throw new Exception("core not found") : (PolarChartEngine)CoreChart;

	/// <inheritdoc cref="IPolarChartView.FitToBounds" />
	public bool FitToBounds { get => _fitToBounds; set { _fitToBounds = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPolarChartView.TotalAngle" />
	public double TotalAngle { get => _totalAngle; set { _totalAngle = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPolarChartView.InnerRadius" />
	public double InnerRadius { get => _innerRadius; set { _innerRadius = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPolarChartView.InitialRotation" />
	public double InitialRotation { get => _initialRotation; set { _initialRotation = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPolarChartView.AngleAxes" />
	public IEnumerable<IPolarAxis> AngleAxes
	{
		get => _anglesAxes;
		set
		{
			AngleObserver?.Dispose(_anglesAxes);
			AngleObserver?.Initialize(value);
			if (CoreChart is null) return;
			_anglesAxes = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IPolarChartView.RadiusAxes" />
	public IEnumerable<IPolarAxis> RadiusAxes
	{
		get => _radiusAxes;
		set
		{
			RadiusObserver?.Dispose(_radiusAxes);
			RadiusObserver?.Initialize(value);
			if (CoreChart is null) return;
			_radiusAxes = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IPolarChartView.Series" />
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

	#endregion

	#region IPolarChartView methods

	/// <inheritdoc cref="IPolarChartView.ScaleDataToPixels(LvcPointD, int, int)" />
	public LvcPointD ScaleDataToPixels(LvcPointD point, int angleAxisIndex = 0, int radiusAxisIndex = 0)
	{
		if (CoreChart is not PolarChartEngine coreChart) throw new Exception("core not found");

		var scaler = new PolarScaler(
			coreChart.DrawMarginLocation, coreChart.DrawMarginSize, coreChart.AngleAxes[angleAxisIndex], coreChart.RadiusAxes[radiusAxisIndex],
			coreChart.InnerRadius, coreChart.InitialRotation, coreChart.TotalAnge);

		var r = scaler.ToPixels(point.X, point.Y);

		return new LvcPointD { X = (float)r.X, Y = (float)r.Y };
	}

	/// <inheritdoc cref="IPolarChartView.ScalePixelsToData(LvcPointD, int, int)" />
	public LvcPointD ScalePixelsToData(LvcPointD point, int angleAxisIndex = 0, int radiusAxisIndex = 0)
	{
		if (CoreChart is not PolarChartEngine coreChart) throw new Exception("core not found");

		var scaler = new PolarScaler(
			coreChart.DrawMarginLocation, coreChart.DrawMarginSize, coreChart.AngleAxes[angleAxisIndex], coreChart.RadiusAxes[radiusAxisIndex],
			coreChart.InnerRadius, coreChart.InitialRotation, coreChart.TotalAnge);

		return scaler.ToChartValues(point.X, point.Y);
	}

	#endregion

	protected readonly CollectionDeepObserver<ISeries> SeriesObserver;
	protected readonly CollectionDeepObserver<IPolarAxis> AngleObserver;
	protected readonly CollectionDeepObserver<IPolarAxis> RadiusObserver;

	public PolarChart()
	{
		LiveCharts.Configure(config => config.UseDefaults());

		SeriesObserver = new CollectionDeepObserver<ISeries>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		AngleObserver = new CollectionDeepObserver<IPolarAxis>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		RadiusObserver = new CollectionDeepObserver<IPolarAxis>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		VisualsObserver = new CollectionDeepObserver<ChartElement>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		OnInitialize();

		AngleAxes = [GetProvider().GetDefaultPolarAxis()];
		RadiusAxes = [GetProvider().GetDefaultPolarAxis()];
		Series = new ObservableCollection<ISeries>();
		VisualElements = new ObservableCollection<ChartElement>();
		SyncContext = new object();
	}

	public override IEnumerable<ChartPoint> GetPointsAt(LvcPointD point, FindingStrategy strategy = FindingStrategy.Automatic, FindPointFor findPointFor = FindPointFor.HoverEvent)
	{
		if (CoreChart is not PolarChartEngine coreChart) throw new Exception("core not found");

		if (strategy == FindingStrategy.Automatic)
			strategy = coreChart.Series.GetFindingStrategy();

		return coreChart.Series.SelectMany(series => series.FindHitPoints(coreChart, new(point), strategy, findPointFor));
	}

	public override IEnumerable<IChartElement> GetVisualsAt(LvcPointD point)
	{
		return CoreChart is not PolarChartEngine coreChart
			? throw new Exception("core not found")
			: coreChart.VisualElements.SelectMany(visual => ((VisualElement)visual).InvokeIsHitBy(coreChart, new(point)));
	}

	protected override void OnInitialize()
	{
		if (CoreChart is null)
		{
			CoreChart = new PolarChartEngine(this, config => config.UseDefaults(), CoreCanvas);
			if (SyncContext != null)
				CoreCanvas.SetSync(SyncContext);

			if (CoreChart == null) throw new Exception("Core not found!");
			CoreChart.Update();

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

			chartBehaviour.On(this);
		}

		CoreChart.Load();
		CoreChart.Update();
	}

	public override void Dispose()
	{
		SeriesObserver?.Dispose(_series);
		AngleObserver?.Dispose(_anglesAxes);
		RadiusObserver?.Dispose(_radiusAxes);
		base.Dispose();
	}
}
