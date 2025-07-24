namespace EasyUIBinding.GirCore;

public class ColorSelector : InputBinding<Adw.ActionRow, Gdk.RGBA>, IDisposable
{
	private readonly Adw.ActionRow _row;
	private readonly Gtk.ColorDialogButton _colorButton;

	public ColorSelector(string title, Gdk.RGBA color)
		: this(Guid.NewGuid().ToString(), title, color)
	{
	}

	public ColorSelector(string name, string title, Gdk.RGBA color)
	{
		Name = name;

		_row = Adw.ActionRow.New();
		_row.Valign = Gtk.Align.Center;
		_row.Title = title;
		_row.Activatable = false;

		_colorButton = new Gtk.ColorDialogButton
		{
			Vexpand = false,
			Valign = Gtk.Align.Center,
			Halign = Gtk.Align.End,
			Dialog = Gtk.ColorDialog.New()

		};
		_colorButton.SetRgba(color);
		_colorButton.OnNotify += OnChanged;
		_row.AddSuffix(_colorButton);
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is Gdk.RGBA rgba)
		{
			_colorButton.SetRgba(rgba);
		}
	}

	private void OnChanged(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "rgba")
		{
			var newValue = ((Gtk.ColorDialogButton)sender).GetRgba();
			UpdateBoundProperty(newValue);
			InvokeCallback(this, new InputChangedEventArgs<Gdk.RGBA>(Name, newValue));
		}
	}

	public override Adw.ActionRow Row => _row;
	public override Gdk.RGBA Value => _colorButton.GetRgba();

	public override void SetValue(Gdk.RGBA value)
	{
		_colorButton.SetRgba(value);
	}

	public override void Dispose()
	{
		_colorButton.OnNotify -= OnChanged;
		_colorButton.Dialog?.Dispose();
		_colorButton.Dispose();
		base.Dispose();
	}
}
