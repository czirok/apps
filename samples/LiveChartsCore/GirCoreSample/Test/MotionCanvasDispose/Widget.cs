using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Test.MotionCanvasDispose;

namespace GirCoreSample.Test.MotionCanvasDispose;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class Widget : Gtk.Box, IDisposable
{
	private readonly MotionCanvas motionCanvas;

	public Widget()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		motionCanvas = new MotionCanvas();
		Append(motionCanvas);
		ViewModel.Generate(motionCanvas.CoreCanvas);
	}

	public override void Dispose()
	{
		motionCanvas.Dispose();
		base.Dispose();
	}
}
