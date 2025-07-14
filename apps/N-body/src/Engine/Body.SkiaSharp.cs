using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;

namespace NBody;

internal partial class Body
{
	/// <summary>
	/// The brush to use for drawing the body. 
	/// </summary>
	readonly SKPaint _drawingBrush = new()
	{
		Style = SKPaintStyle.Fill,
		IsAntialias = true,
		Color = SKColors.White // Alapértelmezett, felülírja a Color property
	};

	/// <summary>
	/// The pen to use for drawing tracers. 
	/// </summary>
	//private static Pen TracerPen = new Pen(new SolidBrush(Color.FromArgb(40, Color.Cyan)));

	static readonly SKPaint TracerPen = new()
	{
		Style = SKPaintStyle.Stroke,
		Color = Colors.Cyan.WithAlpha(40).ToColor(),
		IsAntialias = true,
	};

	/// <summary>
	/// Draws the body. 
	/// </summary>
	/// <param name="canvas">The graphics surface to draw on.</param>
	/// <param name="renderer">The renderer to use for drawing.</param>
	public void Draw(SKCanvas canvas, Renderer renderer)
	{
		renderer.FillCircle2D(canvas, _drawingBrush, Location, Radius);

		if (DrawTracers)
		{
			for (int i = 0; i < _locationHistory.Length; i++)
			{
				int j = (_locationHistoryIndex + i) % _locationHistory.Length;
				int k = (j + 1) % _locationHistory.Length;
				SKPoint start = renderer.ComputePoint(_locationHistory[j]);
				SKPoint end = renderer.ComputePoint(_locationHistory[k]);
				canvas.DrawLine(start, end, TracerPen);
			}
		}
	}
}