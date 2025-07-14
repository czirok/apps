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
		static void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
		{
			var canvas = e.Surface.Canvas;
			var info = e.Info;

			// Clear the canvas with a background color
			canvas.Clear(SKColors.Beige);

			// Set up the font and paint for drawing text
			// Note: SKTypeface.Default uses the system default font
			using SKTypeface typeface = SKTypeface.Default;
			using SKFont font = new(typeface, 36);
			using SKPaint paint = new()
			{
				Color = SKColors.DarkBlue,
				IsAntialias = true
			};

			// Measure the text to center it
			// Note: MeasureText returns the width of the text, and bounds contains the height
			SKRect bounds = SKRect.Empty;
			float width = font.MeasureText(text, out bounds, paint);
			float height = bounds.Height;

			// Draw the text centered in the canvas
			// Note: The text is centered both horizontally and vertically
			canvas.DrawText(text,
				(info.Width - width) / 2,
				(info.Height - height) / 2 + bounds.Height,
				font,
				paint);
		}
	}
}