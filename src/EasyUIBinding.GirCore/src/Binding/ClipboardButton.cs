namespace EasyUIBinding.GirCore.Binding;

public partial class ClipboardButton : InputBinding<Adw.ActionRow, string>, IDisposable
{
	private readonly Adw.ActionRow _row;
	private string? _value;
	private readonly Gtk.Image _statusIcon;
	public ClipboardButton(string name, string title)
	{
		Name = name;

		_row = Adw.ActionRow.New();
		_row.Title = string.Empty;
		_row.Activatable = true;
		_row.OnActivated += OnChanged;

		_statusIcon = Gtk.Image.New();

		_row.AddPrefix(Gtk.Label.New(title));
		_row.AddSuffix(_statusIcon);
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is string text)
		{
			_value = text;
		}
	}

	private void OnChanged(Adw.ActionRow sender, EventArgs args)
	{
		var display = Gdk.Display.GetDefault();
		if (display is null)
		{
			_statusIcon.SetFromIconName("checkbox-mixed-symbolic");
			_statusIcon.CssClasses = ["error"];
			InvokeCallback(sender, new InputChangedEventArgs<string>(Name, null));
			return;
		}

		var clipboard = display.GetClipboard();
		if (clipboard is not null)
		{
			clipboard.SetText(_value!);
			_statusIcon.SetFromIconName("checkbox-checked-symbolic");
			_statusIcon.CssClasses = ["success"];
			UpdateBoundProperty(_value!);
			InvokeCallback(sender, new InputChangedEventArgs<string>(Name, _value!));
		}
		else
		{
			_statusIcon.SetFromIconName("checkbox-mixed-symbolic");
			_statusIcon.CssClasses = ["error"];
			InvokeCallback(this, new InputChangedEventArgs<string>(Name, null));
		}
	}

	public override Adw.ActionRow Row => _row;
	public override string Value => _value!;

	public override void SetValue(string value)
	{
		_value = value;
	}

	public override void Dispose()
	{
		Row.OnActivated -= OnChanged;
		base.Dispose();
	}
}
