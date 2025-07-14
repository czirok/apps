using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;
using System.Runtime.Versioning;

namespace Gtk.MauiGraphicsSkia.GirCore;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class GirCoreSkiaDirectRenderer : ISkiaGraphicsRenderer
{
	private readonly SkiaCanvas _canvas = new();
	private readonly ScalingCanvas _scalingCanvas;
	private Color _backgroundColor = Colors.Transparent;
	private IDrawable? _drawable;

	public GirCoreSkiaDirectRenderer()
	{
		_scalingCanvas = new ScalingCanvas(_canvas);
	}

	public GirCoreSkiaGraphicsView GraphicsView { private get; set; } = default!;

	public ICanvas Canvas => _canvas;

	public IDrawable Drawable
	{
		get => _drawable!;
		set
		{
			_drawable = value;
			Invalidate();
		}
	}

	public Color BackgroundColor
	{
		get => _backgroundColor;
		set => _backgroundColor = value;
	}

	public void Draw(SKCanvas canvas, RectF dirtyRect)
	{
		if (canvas == null) return;

		var oldCanvas = _canvas.Canvas;
		_canvas.Canvas = canvas;

		try
		{
			_canvas.SaveState();

			if (_backgroundColor != null)
			{
				_canvas.FillColor = _backgroundColor;
				_canvas.FillRectangle(dirtyRect);
			}

			_drawable?.Draw(_scalingCanvas, dirtyRect);
		}
		catch (Exception e)
		{
			System.Diagnostics.Debug.WriteLine($"Skia render error: {e}");
		}
		finally
		{
			_canvas.Canvas = oldCanvas;
			_scalingCanvas.ResetState();
		}
	}

	public void Invalidate() => GraphicsView?.Invalidate();

	public void Invalidate(float x, float y, float w, float h) => Invalidate();

	public void SizeChanged(int width, int height) { }

	public void Detached() { }

	public void Dispose()
	{
		_canvas.Dispose();
		GC.SuppressFinalize(this);
	}
}
