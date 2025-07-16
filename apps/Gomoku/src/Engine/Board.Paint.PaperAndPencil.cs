using Gtk.MauiGraphicsSkia.GirCore;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Runtime.Versioning;

namespace Gomoku;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public partial class Board : GirCoreSkiaGraphicsView, IDisposable
{
	protected void PaperAndPencil(SKPaintSurfaceEventArgs e)
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
		paint.Color = new SKColor(199, 213, 230);
		canvas.DrawRect(new SKRect(0, 0, boardPixelSize, boardPixelSize), paint);

		paint.Style = SKPaintStyle.Stroke;
		paint.Color = new SKColor(167, 189, 212);
		paint.StrokeWidth = 1;

		for (var i = 0; i <= _boardModel.BoardSize; i++)
		{
			canvas.DrawLine(cellSize * i, 0, cellSize * i, cellSize * _boardModel.BoardSize, paint);
			canvas.DrawLine(0, cellSize * i, cellSize * _boardModel.BoardSize, cellSize * i, paint);
		}

		// Winning squares
		if (HasWinner)
		{
			foreach (var s in WinningStones)
			{
				paint.Style = SKPaintStyle.Fill;
				paint.Color = SKColors.Yellow;
				canvas.DrawRect(
					new SKRect(
						cellSize * s.Col,
						cellSize * s.Row,
						cellSize * s.Col + cellSize,
						cellSize * s.Row + cellSize),
					paint);
			}
		}

		// Drawing Stones as X and O characters
		var stones = Stones;
		for (var i = 0; i < _boardModel.BoardSize; i++)
			for (var j = 0; j < _boardModel.BoardSize; j++)
			{
				switch (stones[i, j])
				{
					case StoneColor.White: // O
						{
							using var redPaint = new SKPaint
							{
								Color = new SKColor(189, 49, 30),
								IsAntialias = true,
								Style = SKPaintStyle.Stroke,
								StrokeWidth = 4,
								StrokeCap = SKStrokeCap.Round
							};

							float centerX = (cellSize * j) + cellSize / 2f;
							float centerY = (cellSize * i) + cellSize / 2f;
							float radius = Math.Min(12f, (cellSize / 2f) - 4f);

							canvas.DrawCircle(centerX, centerY, radius, redPaint);
						}
						break;

					case StoneColor.Black: // X
						{
							using var bluePaint = new SKPaint
							{
								Color = new SKColor(37, 30, 189),
								IsAntialias = true,
								Style = SKPaintStyle.Stroke,
								StrokeWidth = 4,
								StrokeCap = SKStrokeCap.Round
							};

							float centerX = (cellSize * j) + cellSize / 2f;
							float centerY = (cellSize * i) + cellSize / 2f;
							float offset = Math.Min(12f, (cellSize / 2f) - 4f);

							canvas.DrawLine(
								centerX - offset, centerY - offset,
								centerX + offset, centerY + offset,
								bluePaint);
							canvas.DrawLine(
								centerX + offset, centerY - offset,
								centerX - offset, centerY + offset,
								bluePaint);
						}
						break;
				}
			}

		if (StoneCoordinate != null)
		{
			float centerX = (cellSize * StoneCoordinate.Col) + cellSize / 2f;
			float centerY = (cellSize * StoneCoordinate.Row) + cellSize / 2f;

			if (_humanColor == StoneColor.White) // O hover
			{
				using var hoverPaint = new SKPaint
				{
					Color = SKColors.Red.WithAlpha(100),
					IsAntialias = true,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = 4,
					StrokeCap = SKStrokeCap.Round
				};

				float radius = (cellSize / 2f) - 4f;
				canvas.DrawCircle(centerX, centerY, radius, hoverPaint);
			}
			else // X hover
			{
				using var hoverPaint = new SKPaint
				{
					Color = SKColors.Blue.WithAlpha(100),
					IsAntialias = true,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = 4,
					StrokeCap = SKStrokeCap.Round
				};

				float offset = (cellSize / 2f) - 4f;

				canvas.DrawLine(
					centerX - offset, centerY - offset,
					centerX + offset, centerY + offset,
					hoverPaint);
				canvas.DrawLine(
					centerX + offset, centerY - offset,
					centerX - offset, centerY + offset,
					hoverPaint);
			}
		}
	}
}