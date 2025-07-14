using SkiaSharp;
using System.Numerics;

namespace NBody;

internal partial class Renderer
{
	public SKPoint Origin = new();


	public SKPoint ComputePoint(Vector3 location)
	{
		var scale = ComputeScale(location);
		return new SKPoint((int)Math.Round((location.X - Camera.X) * scale + Origin.X),
			(int)Math.Round((-location.Y + Camera.Y) * scale + Origin.Y));
	}

	public bool DrawSquare2D(SKCanvas canvas, SKPaint paint, Vector3 location, float width)
	{
		if (location.Z >= Camera.Z)
			return false;
		var scale = ComputeScale(location);
		var x = (location.X - Camera.X - width * 0.5f) * scale + Origin.X;
		var y = (-location.Y + Camera.Y - width * 0.5f) * scale + Origin.Y;
		var num = width * scale;
		paint.Style = SKPaintStyle.Stroke;
		canvas.DrawRect(x, y, num, num, paint);
		return true;
	}

	public bool FillCircle2D(SKCanvas canvas, SKPaint paint, Vector3 location, float radius)
	{
		if (location.Z >= Camera.Z)
			return false;
		var scale = ComputeScale(location);
		var x = (location.X - Camera.X - radius) * scale + Origin.X;
		var y = (-location.Y + Camera.Y - radius) * scale + Origin.Y;
		var num = radius * scale * 2.0f;
		paint.Style = SKPaintStyle.Fill;
		canvas.DrawOval(x, y, num, num, paint);
		return true;
	}
}