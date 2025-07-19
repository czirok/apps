using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.GirCore;
using System.Runtime.Versioning;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
internal class Program
{
	internal const string ApplicationId = "skiasharp.gircore.quickstart1";
	private static void Main(string[] _)
	{
		// Initialize the required modules
		Adw.Module.Initialize();
		GdkPixbuf.Module.Initialize();
		Cairo.Module.Initialize();

		SkiaSharp.Views.GirCore.Module.Initialize();

		// Create the Adw application
		var app = Adw.Application.New(ApplicationId, Gio.ApplicationFlags.FlagsNone);
		SKDrawingArea? skDrawingArea = null;

		app.OnActivate += (sender, args) =>
		{
			// Create SkiaSharp drawing area
			skDrawingArea = new SKDrawingArea
			{
				Vexpand = true,
				Hexpand = true
			};
			// Handle paint events
			skDrawingArea.PaintSurface += OnPaintSurface;

			// Create header bar and toolbar view
			var headerBar = Adw.HeaderBar.New();
			var toolbarView = Adw.ToolbarView.New();
			toolbarView.AddTopBar(headerBar);
			var box = Gtk.Box.New(Gtk.Orientation.Vertical, 0);
			box.Append(toolbarView);
			box.Append(skDrawingArea);

			// Create window and show
			var window = Adw.ApplicationWindow.New((Adw.Application)sender);
			window.Title = "SkiaSharp Quick Start";
			window.Content = box;
			window.SetDefaultSize(800, 600);
			window.Show();
		};

		app.OnShutdown += (sender, args) =>
		{
			// Clean up resources
			if (skDrawingArea != null)
			{
				skDrawingArea.PaintSurface -= OnPaintSurface;
				skDrawingArea.Dispose();
				skDrawingArea = null;
			}
		};

		app.Run(0, null);


		const string text = "Hello SkiaSharp on Linux!";
		const string starPath = "m8.03 2-1.875 3.939-4.15 0.621 2.982 3.08-0.732 4.336 3.719-2.037 3.697 2.061-0.684-4.34 3.02-3.062-4.143-0.645zm-8e-3 2 1.221 2.7308789 2.762 0.432-2.01 1.9451211 0.455 3.048803-2.463-1.373-2.48 1.357 0.488-3.046803-1.988-1.9591211 2.766-0.412z";

		static void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
		{
			var canvas = e.Surface.Canvas;
			var info = e.Info;

			// Clear the canvas with a background color
			canvas.Clear(SKColors.Beige);

			// Draw text
			using SKTypeface typeface = SKTypeface.Default;
			using SKFont font = new(typeface, 36);
			using SKPaint textPaint = new()
			{
				Color = SKColors.DarkBlue,
				IsAntialias = true
			};

			SKRect bounds = SKRect.Empty;
			float width = font.MeasureText(text, out bounds, paint: textPaint);
			float height = bounds.Height;

			canvas.DrawText(text,
				(info.Width - width) / 2,
				(info.Height - height) / 2 + bounds.Height,
				font,
				textPaint);

			// Draw random stars
			var random = new Random();
			using SKPaint starPaint = new()
			{
				Color = SKColors.Red,
				IsAntialias = true,
				Style = SKPaintStyle.Fill
			};

			// Draw 10 random stars
			for (int i = 0; i < 10; i++)
			{
				var starSkPath = SKPath.ParseSvgPathData(starPath);
				if (starSkPath != null)
				{
					// Random position
					float x = random.Next(0, info.Width - 25);
					float y = random.Next(0, info.Height - 25);

					// Random scale
					float scale = (float)(random.NextDouble() * 2 + 0.5); // 0.5 to 2.5

					canvas.Save();
					canvas.Translate(x, y);
					canvas.Scale(scale);
					canvas.DrawPath(starSkPath, starPaint);
					canvas.Restore();
				}
			}
		}
	}
}