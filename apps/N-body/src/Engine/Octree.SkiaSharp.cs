using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;

namespace NBody;

internal partial class Octree
{
	/// <summary>
	/// The pen to use for drawing. 
	/// </summary>
	//private static Pen DrawingPen = new Pen(new SolidBrush(Color.FromArgb(100, Color.Red)));

	static readonly SKPaint DrawingPen = new SKPaint
	{
		Style = SKPaintStyle.Stroke,
		Color = Colors.Red.WithAlpha(100).ToColor(),
		IsAntialias = true,
	};

	/// <summary>
	/// Draws the tree and its subtrees. 
	/// </summary>
	/// <param name="canvas">The graphics surface to draw on.</param>
	public void Draw(SKCanvas canvas, Renderer renderer)
	{
		renderer.DrawSquare2D(canvas, DrawingPen, _location, _width);

		if (_subtrees != null)
			foreach (Octree subtree in _subtrees)
				if (subtree != null)
					subtree.Draw(canvas, renderer);
	}
}