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
public class PieChart : BaseChart, IPieChartView
{
	#region IPieChartView fields

	private IEnumerable<ISeries> _series = [];
	private bool _isClockwise = true;
	private double _initialRotation;
	private double _maxAngle = 360;
	private double _maxValue = double.NaN;
	private double _minValue;

	#endregion

	#region IPieChartView properties

	public PieChartEngine Core => CoreChart == null ? throw new Exception("core not found") : (PieChartEngine)CoreChart;

	/// <inheritdoc cref="IPieChartView.Series" />
	public IEnumerable<ISeries> Series
	{
		get => _series;
		set
		{
			SeriesObserver.Dispose(_series);
			SeriesObserver.Initialize(value);
			if (CoreChart is null) return;
			_series = value;
			OnPropertyChanged();
		}
	}

	/// <inheritdoc cref="IPieChartView.IsClockwise" />
	public bool IsClockwise { get => _isClockwise; set { _isClockwise = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPieChartView.InitialRotation" />
	public double InitialRotation { get => _initialRotation; set { _initialRotation = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPieChartView.MaxAngle" />
	public double MaxAngle { get => _maxAngle; set { _maxAngle = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPieChartView.MaxValue" />
	public double MaxValue { get => _maxValue; set { _maxValue = value; OnPropertyChanged(); } }

	/// <inheritdoc cref="IPieChartView.MinValue" />
	public double MinValue { get => _minValue; set { _minValue = value; OnPropertyChanged(); } }

	#endregion

	protected readonly CollectionDeepObserver<ISeries> SeriesObserver;

	public PieChart()
	{
		LiveCharts.Configure(config => config.UseDefaults());

		SeriesObserver = new CollectionDeepObserver<ISeries>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		VisualsObserver = new CollectionDeepObserver<ChartElement>(
			(sender, e) => CoreChart?.Update(),
			(sender, e) => CoreChart?.Update());

		OnInitialize();

		Series = new ObservableCollection<ISeries>();
		VisualElements = new ObservableCollection<ChartElement>();
		SyncContext = new object();
	}

	#region Chart overrides

	public override IEnumerable<ChartPoint> GetPointsAt(LvcPointD point, FindingStrategy strategy = FindingStrategy.Automatic, FindPointFor findPointFor = FindPointFor.HoverEvent)
	{
		if (CoreChart is not PieChartEngine coreChart) throw new Exception("core not found");

		if (strategy == FindingStrategy.Automatic)
			strategy = coreChart.Series.GetFindingStrategy();

		return coreChart.Series.SelectMany(series => series.FindHitPoints(coreChart, new(point), strategy, findPointFor));
	}

	public override IEnumerable<IChartElement> GetVisualsAt(LvcPointD point)
	{
		return CoreChart is not PieChartEngine coreChart
			? throw new Exception("core not found")
			: coreChart.VisualElements.SelectMany(visual => ((VisualElement)visual).InvokeIsHitBy(coreChart, new(point)));
	}

	#endregion

	protected override void OnInitialize()
	{
		if (CoreChart is null)
		{
			CoreChart = new PieChartEngine(this, config => config.UseDefaults(), CoreCanvas);

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
		base.Dispose();
	}
}
