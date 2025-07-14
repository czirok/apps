using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace SkiaSharp.Views.GirCore;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
[ToolboxItem(true)]
public class SKDrawingArea : Gtk.DrawingArea
{
	public SKDrawingArea()
	{
		SetDrawFunc(DrawCallback);
	}

	[Category("Appearance")]
	public event EventHandler<SKPaintSurfaceEventArgs>? PaintSurface;

	public SKSize CanvasSize => new(GetAllocatedWidth(), GetAllocatedHeight());
	private SKImageInfo? _cachedImageInfo;

	private void DrawCallback(Gtk.DrawingArea area, Cairo.Context context, int width, int height)
	{
		if (width == 0 || height == 0)
			return;

		if (_cachedImageInfo?.Width != width || _cachedImageInfo?.Height != height)
		{
			_cachedImageInfo = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
		}

		var imageInfo = _cachedImageInfo.Value;

		using var imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, imageInfo.Width, imageInfo.Height);
		var data = Cairo.Internal.ImageSurface.GetData(imageSurface.Handle);
		using var surface = SKSurface.Create(imageInfo, data, imageInfo.RowBytes);

		using (new SKAutoCanvasRestore(surface.Canvas, true))
		{
			OnPaintSurface(new SKPaintSurfaceEventArgs(surface, imageInfo));
		}

		surface.Canvas.Flush();
		imageSurface.MarkDirty();

		if (imageInfo.ColorType == SKColorType.Rgba8888)
		{
			using var pixmap = surface.PeekPixels();
			SKSwizzle.SwapRedBlue(pixmap.GetPixels(), imageInfo.Width * imageInfo.Height);
		}

		context.SetSourceSurface(imageSurface, 0, 0);
		context.Paint();
	}

	public void Invalidate() => QueueDraw();

	protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		PaintSurface?.Invoke(this, e);
	}
}
