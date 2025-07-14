using GObject;
using Microsoft.Maui.Graphics;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.GirCore;
using System.Runtime.Versioning;

namespace Gtk.MauiGraphicsSkia.GirCore;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class GirCoreSkiaGraphicsView : SKDrawingArea, IDisposable
{
	private RectF _dirtyRect = default;
	private IDrawable _drawable = default!;
	private ISkiaGraphicsRenderer _renderer = default!;
	private SignalHandler<DrawingArea, ResizeSignalArgs>? _resizeHandler;

	public GirCoreSkiaGraphicsView()
	{
		Renderer = CreateDefaultRenderer();

		_resizeHandler = (area, args) =>
		{
			_dirtyRect.Width = args.Width;
			_dirtyRect.Height = args.Height;
			_renderer?.SizeChanged(args.Width, args.Height);
		};

		OnResize += _resizeHandler;
	}

	public ISkiaGraphicsRenderer Renderer
	{
		get => _renderer;
		set
		{
			if (_renderer != null)
			{
				_renderer.Drawable = default!;
				_renderer.GraphicsView = default!;
				_renderer.Dispose();
			}

			_renderer = value ?? CreateDefaultRenderer();
			_renderer.GraphicsView = this;
			_renderer.Drawable = _drawable;
			_renderer.SizeChanged((int)CanvasSize.Width, (int)CanvasSize.Height);
		}
	}

	private ISkiaGraphicsRenderer CreateDefaultRenderer()
	{
		return new GirCoreSkiaDirectRenderer();
	}

	public Color BackgroundColor
	{
		get => _renderer.BackgroundColor;
		set => _renderer.BackgroundColor = value;
	}

	public IDrawable Drawable
	{
		get => _drawable;
		set
		{
			_drawable = value;
			_renderer.Drawable = _drawable;
		}
	}

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		_renderer?.Draw(e.Surface.Canvas, _dirtyRect);
	}

	public override void Dispose()
	{
		Dispose(true);
		base.Dispose();
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _resizeHandler is not null)
		{
			OnResize -= _resizeHandler;
			_resizeHandler = null;
		}
	}
}
