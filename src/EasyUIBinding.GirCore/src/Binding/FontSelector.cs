namespace EasyUIBinding.GirCore.Binding;

public class FontSelector : InputBinding<Adw.ActionRow, string>, IDisposable
{
	private readonly Adw.ActionRow _row;
	private readonly Gtk.FontDialogButton _fontButton;

	public FontSelector(string name, string title, string initialFont)
	{
		Name = name;
		_row = Adw.ActionRow.New();
		_row.Valign = Gtk.Align.Center;
		_row.Title = title;

		_row.Activatable = false;

		_fontButton = new Gtk.FontDialogButton
		{
			Vexpand = false,
			Valign = Gtk.Align.Center,
			Halign = Gtk.Align.End,
			Dialog = Gtk.FontDialog.New(),
			FontDesc = Pango.FontDescription.FromString(initialFont)
		};

		_fontButton.OnNotify += OnChanged;
		_row.AddSuffix(_fontButton);
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is string fontDesc)
		{
			_fontButton.FontDesc = Pango.FontDescription.FromString(fontDesc);
		}
	}

	private void OnChanged(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "font-desc")
		{
			var newValue = ((Gtk.FontDialogButton)sender).FontDesc.ToString();
			UpdateBoundProperty(newValue);
			InvokeCallback(this, new InputChangedEventArgs<string>(Name, newValue));
		}
	}

	public override Adw.ActionRow Row => _row;
	public override string Value => _fontButton.FontDesc.ToString();

	public override void SetValue(string value)
	{
		_fontButton.FontDesc = Pango.FontDescription.FromString(value);
	}

	public override void Dispose()
	{
		_fontButton.OnNotify -= OnChanged;
		_fontButton.Dialog?.Dispose();
		_fontButton.Dispose();
		base.Dispose();
	}
}
