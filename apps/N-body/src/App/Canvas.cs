using Gtk.MauiGraphicsSkia.GirCore;
using Microsoft.Extensions.Localization;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Numerics;
using System.Runtime.Versioning;

namespace NBody.App;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class Canvas : GirCoreSkiaGraphicsView, IDisposable
{
	/// <summary>
	/// The number of milliseconds between simulation frames. 
	/// </summary>
	private const int FrameInterval = 40; // 25 FPS

	private readonly IStringLocalizer<Canvas> L;

	public Canvas(IStringLocalizer<Canvas> localizer)
	{
		L = localizer;
		Vexpand = true;
		Hexpand = true;

		// Initialize the world and set it as the drawable.
		GLib.Functions.TimeoutAdd(
			priority: GLib.Constants.PRIORITY_DEFAULT_IDLE,
			interval: FrameInterval,
			function: new GLib.SourceFunc(() =>
			{
				Invalidate();
				return GLib.Constants.SOURCE_CONTINUE;
			})
		);

		SetupInputHandlers();

		Show();
	}

	private long _tick = Environment.TickCount64;
	private double _fps = 0;

	private SKTypeface? _statTypeface = SKTypeface.FromFamilyName("monospace");
	private SKFont? _statFont = new SKFont(SKTypeface.FromFamilyName("monospace"), 11);
	private SKPaint? _statPaint = new SKPaint
	{
		Color = SKColors.Yellow.WithAlpha(100),
		IsAntialias = true
	};
	private SKPaint? _authorsPaint = new SKPaint
	{
		Color = SKColors.Gray.WithAlpha(100),
		IsAntialias = true
	};

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var canvas = e.Surface.Canvas;
		canvas.Clear();
		canvas.Save();
		canvas.Translate(e.Info.Width / 2.0f, e.Info.Height / 2.0f);
		World.Instance.Draw(canvas);
		canvas.Restore();
		World.Instance.Simulate();

		if (World.Instance.ShowStats)
		{
			float x = 10;
			float y = 20;
			float dy = 11 + 4;

			canvas.DrawText(L["StatLine", L["System"].Value, World.Instance.SystemTitle].Value, x, y + dy * 0, SKTextAlign.Left, _statFont, _statPaint);
			canvas.DrawText(L["StatLine2", L["Simulation"].Value, _fps, L["FPS"].Value].Value, x, y + dy * 2, SKTextAlign.Left, _statFont, _statPaint);
			canvas.DrawText(L["StatLine", L["Bodies"].Value, World.Instance.BodyCount].Value, x, y + dy * 3, SKTextAlign.Left, _statFont, _statPaint);
			canvas.DrawText(L["StatLineExp", L["Total mass"].Value, World.Instance.TotalMass].Value, x, y + dy * 4, SKTextAlign.Left, _statFont, _statPaint);
			canvas.DrawText(L["StatLine", L["Frames"].Value, World.Instance.Frames].Value, x, y + dy * 5, SKTextAlign.Left, _statFont, _statPaint);

			canvas.DrawText("ZONG ZHENG LI & FERENC CZIROK", x, e.Info.Height - 20, SKTextAlign.Left, _statFont, _authorsPaint);
		}

		// Update draw FPS. 
		var now = Environment.TickCount64;
		var delta = now - _tick;
		_tick = now;
		_fps = delta > 0 ? 1000.0 / delta : 0;

		base.OnPaintSurface(e);
	}

	private void SetupInputHandlers()
	{
		double _lastDragX = 0;
		double _lastDragY = 0;
		float _accumX = 0;
		float _accumY = 0;

		var drag = Gtk.GestureDrag.New();

		drag.OnDragBegin += (gesture, args) =>
		{
			gesture.GetPoint(null, out _lastDragX, out _lastDragY);
			_accumX = 0;
			_accumY = 0;
		};

		drag.OnDragUpdate += (gesture, args) =>
		{
			gesture.GetPoint(null, out var currentX, out var currentY);

			var deltaX = currentX - _lastDragX;
			var deltaY = currentY - _lastDragY;

			_lastDragX = currentX;
			_lastDragY = currentY;

			_accumX += (float)deltaX;
			_accumY += (float)deltaY;

			var vector = new Vector3((float)deltaX, (float)deltaY, 0.0f);

			if (vector.Magnitude() > 0.0d)
			{
				World.Instance.Rotate(Vector3.Zero, new Vector3(vector.Y, vector.X, 0.0f), (float)(vector.Magnitude() * 0.005f));
			}
		};

		drag.OnDragEnd += (gesture, args) =>
		{
			var axis = Vector3.Normalize(new Vector3(_accumY, _accumX, 0.0f));
			var angle = MathF.Sqrt(_accumX * _accumX + _accumY * _accumY) * 0.005f;

			if (angle > 0)
				Console.WriteLine(
					FormattableString.Invariant(
						$"Rotate(Vector3.Zero, Vector3.Normalize(new Vector3({_accumY:0.0#}f, {_accumX:0.0#}f, 0.0f)), {angle:0.0000}f);"));
		};

		AddController(drag);

		var scroll = Gtk.EventControllerScroll.New(Gtk.EventControllerScrollFlags.Vertical);
		scroll.OnScroll += (controller, args) =>
		{
			World.Instance.MoveCamera(((int)args.Dy) * 50);
			return false;
		};

		AddController(scroll);
	}

	public override void Dispose()
	{
		_authorsPaint?.Dispose();
		_statPaint?.Dispose();
		_statFont?.Dispose();
		_statTypeface?.Dispose();
		World.Instance.Active = false;
		base.Dispose();
	}
}
