using EasyUIBinding.GirCore;
using System.Runtime.Versioning;

namespace GirCoreSample.Test.MotionCanvasDispose;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private readonly WrapPreferencesGroup wrapGroup;
	private Widget? widget;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);

		wrapGroup = new WrapPreferencesGroup(
		[
			new Button("Change content").OnClick((s, e) => {
				if (widget != null)
				{
					Remove(widget);
					widget.Dispose();
				}
				widget = new Widget();
				Append(widget);
			})
		]);

		Append(wrapGroup);
		Append(widget = new Widget());
	}

	public override void Dispose()
	{
		wrapGroup.Dispose();
		base.Dispose();
	}
}
