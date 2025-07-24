using Gtk;

namespace EasyUIBinding.GirCore;

public static class Style
{
	private static string _css = """
.wrap-preferences-group box {
	margin-top: 0;
	margin-bottom: 0;
	margin-left: 0;
	margin-right: 0;
    min-height: 32px;
}

.wrap-preferences-group box label {
	margin-top: 0;
	margin-bottom: 0;
	margin-left: 6px;
	margin-right: 6px;
    min-height: 32px;
}

.wrap-preferences-group row > box.header {
	margin-top: 0;
	margin-bottom: 0;
	margin-left: 6px;
	margin-right: 6px;
    min-height: 32px;
}
""";

	public static void Initialize()
	{
		var provider = CssProvider.New();
		provider.LoadFromData(_css, _css.Length);
		StyleContext.AddProviderForDisplay(Gdk.Display.GetDefault()!, provider, Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);
	}
}

// public class Switch(string title, bool? active = false) : GirCore.Binding.Switch(Guid.NewGuid().ToString(), title, active)
// {
// }

// public class Button(string title) : GirCore.Binding.Button(Guid.NewGuid().ToString(), Label.New(title), null)
// {
// }

// public static class ChartUI
// {
// 	public static WrapBox AddChildren(this WrapBox widget, IEnumerable<Widget> children)
// 	{
// 		widget.SetOrientation(Orientation.Horizontal);
// 		foreach (var child in children)
// 		{
// 			widget.Append(child);
// 		}

// 		return widget;
// 	}

// 	public static PreferencesGroup Group(PreferencesRow child)
// 	{
// 		var group = PreferencesGroup.New();
// 		group.Add(child);
// 		return group;
// 	}
// }
