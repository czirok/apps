using Gtk.MauiGraphicsSkia.GirCore;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Runtime.Versioning;

namespace Gomoku;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public partial class Board : GirCoreSkiaGraphicsView, IDisposable
{
	protected void WoodWithStones(SKPaintSurfaceEventArgs e)
	{
		base.OnPaintSurface(e);
		var canvas = e.Surface.Canvas;
		canvas.Clear();

		var cellSize = (CanvasSize.Height < CanvasSize.Width)
			? (CanvasSize.Height - 1) / _boardModel.BoardSize
			: (CanvasSize.Width - 1) / _boardModel.BoardSize;

		float boardPixelSize = (int)(cellSize * _boardModel.BoardSize);

		using var paint = new SKPaint { IsAntialias = true };

		paint.Style = SKPaintStyle.Fill;
		paint.Color = new SKColor(222, 184, 135);
		canvas.DrawRect(new SKRect(0, 0, boardPixelSize, boardPixelSize), paint);
		DrawWoodGrain(canvas, new SKRect(0, 0, boardPixelSize, boardPixelSize));
		paint.Style = SKPaintStyle.Stroke;
		paint.Color = SKColors.Black;
		paint.StrokeWidth = 1;

		for (var i = 0; i <= _boardModel.BoardSize; i++)
		{
			canvas.DrawLine(cellSize * i - cellSize / 2f, 0, cellSize * i - cellSize / 2f, cellSize * _boardModel.BoardSize, paint);
			canvas.DrawLine(0, cellSize * i - cellSize / 2f, cellSize * _boardModel.BoardSize, cellSize * i - cellSize / 2f, paint);
		}

		if (HasWinner)
		{
			foreach (var s in WinningStones)
			{
				paint.Style = SKPaintStyle.Fill;
				paint.Color = SKColors.Red;
				canvas.DrawRect(
					new SKRect(
						cellSize * s.Col,
						cellSize * s.Row,
						cellSize * s.Col + cellSize,
						cellSize * s.Row + cellSize),
					paint);
			}
		}

		// 3D shadow paint
		using var shadowPaint = new SKPaint
		{
			Color = SKColors.Black.WithAlpha(50),
			IsAntialias = true,
			Style = SKPaintStyle.Fill
		};
		var shadowOffset = new SKPoint(1, 1);

		var stones = Stones;
		for (var i = 0; i < _boardModel.BoardSize; i++)
			for (var j = 0; j < _boardModel.BoardSize; j++)
			{
				switch (stones[i, j])
				{
					case StoneColor.White:
						{
							// Shadow
							canvas.DrawOval(
								(cellSize * j) + cellSize / 2f + shadowOffset.X,
								(cellSize * i) + cellSize / 2f + shadowOffset.Y,
								(cellSize / 2f) - 2f, (cellSize / 2f) - 2f,
								shadowPaint);

							// 3D gradient
							using var whiteGradient = SKShader.CreateRadialGradient(
								new SKPoint(
									(cellSize * j) + cellSize / 2f - shadowOffset.X,
									(cellSize * i) + cellSize / 2f - shadowOffset.Y),
								(cellSize / 2f) - 2f,
								new SKColor[] {
							SKColors.White,
							new SKColor(240, 240, 240),
							new SKColor(200, 200, 200)
								},
								new float[] { 0.0f, 0.7f, 1.0f },
								SKShaderTileMode.Clamp);

							using var whitePaint = new SKPaint
							{
								Shader = whiteGradient,
								IsAntialias = true,
								Style = SKPaintStyle.Fill
							};

							canvas.DrawOval(
								(cellSize * j) + cellSize / 2f,
								(cellSize * i) + cellSize / 2f,
								(cellSize / 2f) - 2f, (cellSize / 2f) - 2f,
								whitePaint);
						}
						break;

					case StoneColor.Black:
						{
							// Shadow
							canvas.DrawOval(
								(cellSize * j) + cellSize / 2f + shadowOffset.X,
								(cellSize * i) + cellSize / 2f + shadowOffset.Y,
								(cellSize / 2f) - 2f, (cellSize / 2f) - 2f,
								shadowPaint);

							using var blackGradient = SKShader.CreateRadialGradient(
								new SKPoint(
									(cellSize * j) + cellSize / 2f - shadowOffset.X,
									(cellSize * i) + cellSize / 2f - shadowOffset.Y),
								(cellSize / 2f) - 2f,
								new SKColor[] {
							new SKColor(40, 40, 40),
							new SKColor(20, 20, 20),
							SKColors.Black
								},
								new float[] { 0.0f, 0.6f, 1.0f },
								SKShaderTileMode.Clamp);

							using var blackPaint = new SKPaint
							{
								Shader = blackGradient,
								IsAntialias = true,
								Style = SKPaintStyle.Fill
							};

							canvas.DrawOval(
								(cellSize * j) + cellSize / 2f,
								(cellSize * i) + cellSize / 2f,
								(cellSize / 2f) - 2f, (cellSize / 2f) - 2f,
								blackPaint);
						}
						break;
				}
			}

		if (StoneCoordinate != null)
		{
			// Shadow
			canvas.DrawOval(
				(cellSize * StoneCoordinate.Col) + cellSize / 2f + shadowOffset.X,
				(cellSize * StoneCoordinate.Row) + cellSize / 2f + shadowOffset.Y,
				(cellSize / 2f) - 2f, (cellSize / 2f) - 2f,
				shadowPaint);

			// 3D gradient
			using var hoverGradient = SKShader.CreateRadialGradient(
				new SKPoint(
					(cellSize * StoneCoordinate.Col) + cellSize / 2f - shadowOffset.X,
					(cellSize * StoneCoordinate.Row) + cellSize / 2f - shadowOffset.Y),
				(cellSize / 2f) - 2f,
				new SKColor[] {
			_humanColor == StoneColor.White ?
				new SKColor(220, 220, 220) :
				new SKColor(60, 60, 60),
			_humanColor == StoneColor.White ?
				SKColors.White :
				SKColors.Black,
			_humanColor == StoneColor.White ?
				new SKColor(160, 160, 160) :
				new SKColor(30, 30, 30)
				},
				new float[] { 0.0f, 0.7f, 1.0f },
				SKShaderTileMode.Clamp);

			using var hoverPaint = new SKPaint
			{
				Shader = hoverGradient,
				IsAntialias = true,
				Style = SKPaintStyle.Fill
			};

			canvas.DrawOval(
				(cellSize * StoneCoordinate.Col) + cellSize / 2f,
				(cellSize * StoneCoordinate.Row) + cellSize / 2f,
				(cellSize / 2f) - 2f, (cellSize / 2f) - 2f,
				hoverPaint);
		}
	}

	private static void DrawWoodGrain(SKCanvas canvas, SKRect rect)
	{
		canvas.Save();
		canvas.ClipRect(rect);

		using var baseGradient = SKShader.CreateLinearGradient(
			new SKPoint(rect.Left, rect.Top),
			new SKPoint(rect.Right, rect.Bottom),
			new SKColor[] {
			new SKColor(210, 180, 140),  // Tan light wood
            new SKColor(160, 130, 98),   // Darker wood
            new SKColor(139, 115, 85)    // Even darker wood
			},
			null,
			SKShaderTileMode.Clamp);

		using var basePaint = new SKPaint
		{
			Shader = baseGradient,
			IsAntialias = true
		};

		canvas.DrawRect(rect, basePaint);

		// Wood grain effect
		System.Random rand = new System.Random((int)(rect.Left + rect.Top));

		for (int i = 0; i < 8; i++)
		{
			using var grainPaint = new SKPaint
			{
				Color = new SKColor(101, 67, 33, (byte)(30 + rand.Next(0, 40))),
				StrokeWidth = 0.5f + (float)rand.NextDouble() * 1.5f,
				IsAntialias = true,
				Style = SKPaintStyle.Stroke
			};

			float startY = rect.Top + (float)rand.NextDouble() * rect.Height;
			float wave = (float)(rand.NextDouble() * 2 - 1); // -1 to 1

			using var path = new SKPath();
			path.MoveTo(rect.Left, startY);

			// Wavy line across the width of the rect
			for (float x = rect.Left; x <= rect.Right; x += 3)
			{
				float progress = (x - rect.Left) / rect.Width;
				float y = startY + (float)Math.Sin(progress * Math.PI * 2 + i) * wave * 2;

				y = Math.Max(rect.Top, Math.Min(rect.Bottom, y));
				path.LineTo(x, y);
			}

			canvas.DrawPath(path, grainPaint);
		}

		// Small spots/imperfections - strictly within the rect
		for (int i = 0; i < 3; i++)
		{
			float x = rect.Left + (float)rand.NextDouble() * rect.Width;
			float y = rect.Top + (float)rand.NextDouble() * rect.Height;
			float size = 0.5f + (float)rand.NextDouble() * 1.5f;

			using var knotPaint = new SKPaint
			{
				Color = new SKColor(60, 30, 15, 60),
				IsAntialias = true
			};

			canvas.DrawCircle(x, y, size, knotPaint);
		}

		canvas.Restore(); // Restore clipping
	}

}