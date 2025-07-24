namespace EasyUIBinding.GirCore;

public static class UI
{
	public static Gtk.ScrolledWindow Scroll(Gtk.Widget child)
		=> Gtk.ScrolledWindow.New().AddChild(child);

	public static Adw.Bin Bin(Gtk.Widget child)
		=> Adw.Bin.New().AddChild(child);

	public static Adw.Clamp Clamp(Gtk.Widget child)
		=> Adw.Clamp.New().AddChild(child);

	public static Gtk.Box Box(Gtk.Widget child, Gtk.Orientation orientation, int spacing)
		=> Box(orientation, spacing).AddChild(child);

	public static Gtk.Box Box(Gtk.Orientation orientation)
		=> Box(orientation, 24);

	public static Gtk.Box Box(Gtk.Orientation orientation, int spacing)
		=> Gtk.Box.New(orientation, spacing);

	public static Adw.PreferencesGroup Group() => Adw.PreferencesGroup.New();

	public static Adw.PreferencesGroup Group(Adw.PreferencesRow child)
		=> Adw.PreferencesGroup.New().AddChild(child);

	public static Adw.PreferencesGroup Group(string? title = default, string? description = default)
		=> Adw.PreferencesGroup.New().Create(title, description);

	public static Adw.PreferencesGroup Group(Gtk.Widget child, string? title = default)
		=> Adw.PreferencesGroup.New().AddChild(child, title);

	public static Adw.PreferencesGroup AddChild(this Adw.PreferencesGroup group, Adw.PreferencesRow child)
	{
		group.Add(child);
		return group;
	}

	public static Adw.Bin AddChild(this Adw.Bin bin, Gtk.Widget child)
	{
		bin.MarginTop = 24;
		bin.MarginBottom = 24;
		bin.MarginStart = 12;
		bin.MarginEnd = 12;
		bin.Child = child;
		return bin;
	}

	public static Gtk.ScrolledWindow AddChild(this Gtk.ScrolledWindow scrolledWindow, Gtk.Widget child)
	{
		scrolledWindow.SetPolicy(Gtk.PolicyType.Never, Gtk.PolicyType.Automatic);
		scrolledWindow.Child = child;
		scrolledWindow.Vexpand = true;
		scrolledWindow.Hexpand = true;
		return scrolledWindow;
	}

	public static Adw.Clamp AddChild(this Adw.Clamp clamp, Gtk.Widget child)
	{
		clamp.Child = child;
		return clamp;
	}

	public static Gtk.Box AddChild(this Gtk.Box box, Gtk.Widget child)
	{
		box.MarginTop = 24;
		box.MarginBottom = 24;
		box.MarginStart = 12;
		box.MarginEnd = 12;
		box.Spacing = 24;
		box.Append(child);
		return box;
	}

	public static Gtk.Box Design(this Gtk.Box box)
	{
		box.MarginTop = 24;
		box.MarginBottom = 24;
		box.MarginStart = 12;
		box.MarginEnd = 12;
		box.Spacing = 24;
		return box;
	}


	public static Gtk.Label Button(this Gtk.Label label)
	{
		label.Xalign = 0;
		label.MarginTop = 12;
		label.MarginBottom = 12;
		label.MarginStart = 12;
		label.MarginEnd = 12;
		return label;
	}

	public static Adw.PreferencesGroup Create(this Adw.PreferencesGroup group, string? title = default, string? description = default)
	{
		group.Title = title;
		group.Description = description;
		group.Hexpand = true;
		return group;
	}

	public static Adw.PreferencesGroup AddChild(this Adw.PreferencesGroup group, Gtk.Widget child, string? title)
	{
		group.Title = title;
		group.Hexpand = true;
		group.Add(child);
		return group;
	}
}
