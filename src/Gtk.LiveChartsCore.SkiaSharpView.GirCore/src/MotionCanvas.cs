// Copyright (c) Ferenc Czirok
// Licensed under the MIT License.
// See the LICENSE file in the root for more information.

using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Motion;
using LiveChartsCore.SkiaSharpView.Drawing;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.GirCore;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace Gtk.LiveChartsCore.SkiaSharpView.GirCore;

/// <summary>
/// The motion canvas control for GTK with GirCore, <see cref="CoreMotionCanvas"/>.
/// </summary>
[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public partial class MotionCanvas : SKDrawingArea, IDisposable
{
	private bool _isDrawingLoopRunning = false;
	private uint _timerId = 0;
	private uint FrameInterval => (uint)(1000 / LiveCharts.MaxFps);

	/// <inheritdoc cref="IChartView.CoreCanvas" />
	public CoreMotionCanvas CoreCanvas { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MotionCanvas"/> class.
	/// </summary>
	public MotionCanvas()
	{
		Vexpand = true;
		Hexpand = true;
		CoreCanvas = new CoreMotionCanvas();
		CoreCanvas.Invalidated += CanvasCore_Invalidated;
	}

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		CoreCanvas.DrawFrame(new SkiaSharpDrawingContext(CoreCanvas, e.Info, e.Surface, e.Surface.Canvas));
	}

	private void CanvasCore_Invalidated(CoreMotionCanvas sender) =>
		RunDrawingLoop();

	private void RunDrawingLoop()
	{
		if (_isDrawingLoopRunning) return;
		_isDrawingLoopRunning = true;
		if (_timerId != 0)
		{
			GLib.Functions.SourceRemove(_timerId);
			_timerId = 0;
		}

		_timerId = GLib.Functions.TimeoutAdd(
			priority: GLib.Constants.PRIORITY_DEFAULT_IDLE,
			interval: FrameInterval,
			function: new GLib.SourceFunc(() =>
			{
				if (!CoreCanvas.IsValid)
				{
					Invalidate();
					return GLib.Constants.SOURCE_CONTINUE;
				}

				_isDrawingLoopRunning = false;
				_timerId = 0;
				return GLib.Constants.SOURCE_REMOVE;
			})
		);
	}

	public void InvokeOnUIThread(Action action)
	{
		GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_DEFAULT, new GLib.SourceFunc(() =>
		{
			action();
			return GLib.Constants.SOURCE_REMOVE;
		}));
	}

	protected virtual void OnInitialize() { }

	public override void Dispose()
	{
		if (_timerId != 0)
		{
			GLib.Functions.SourceRemove(_timerId);
			_timerId = 0;
		}

		CoreCanvas.Invalidated -= CanvasCore_Invalidated;
		CoreCanvas.Dispose();
		_isDrawingLoopRunning = false;

		base.Dispose();
		GC.SuppressFinalize(this);
	}
}