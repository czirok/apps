namespace EasyUIBinding.GirCore;

public partial class ClipboardButton : InputBinding<Adw.ActionRow, string>, IDisposable
{
	private readonly Adw.ActionRow _row;
	private string? _value;
	private readonly Gtk.Image _statusIcon;

	public ClipboardButton(string title)
		: this(Guid.NewGuid().ToString(), title)
	{
	}

	public ClipboardButton(string name, string title)
	{
		Name = name;

		_row = Adw.ActionRow.New();
		_row.Title = title;
		_row.Activatable = true;
		_row.OnActivated += OnChanged;

		_statusIcon = Gtk.Image.New();
		_statusIcon.SetFromIconName("edit-copy-symbolic");
		_statusIcon.CssClasses = [];

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
			InvokeCallback(this, new InputChangedEventArgs<string>(Name, null));
			return;
		}

		var clipboard = display.GetClipboard();
		if (clipboard is not null)
		{
			clipboard.SetText(_value!);
			_statusIcon.SetFromIconName("checkbox-checked-symbolic");
			_statusIcon.CssClasses = ["success"];
			UpdateBoundProperty(_value!);
			InvokeCallback(this, new InputChangedEventArgs<string>(Name, _value!));

			GLib.Functions.TimeoutAdd(
				priority: GLib.Constants.PRIORITY_LOW,
				interval: 4000, // 4 seconds
				function: new GLib.SourceFunc(() =>
				{
					_statusIcon.SetFromIconName("edit-copy-symbolic");
					_statusIcon.CssClasses = [];
					return GLib.Constants.SOURCE_REMOVE;
				})
			);
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
