namespace EasyUIBinding.GirCore.Binding;

public partial class ButtonLabel : Gtk.Label
{
	public ButtonLabel(string text)
	{
		Label_ = text;
		Xalign = 0;
		MarginTop = 12;
		MarginBottom = 12;
		MarginStart = 12;
		MarginEnd = 12;
	}
}

public partial class Button : Input<Adw.ButtonRow, object>, IDisposable
{
	private readonly Adw.ButtonRow _row;
	private object? _value;

	public Button(string name, Gtk.Widget child, object? value = null)
	{
		Name = name;
		_value = value;
		_row = Adw.ButtonRow.New();
		_row.Activatable = true;
		_row.OnActivated += OnChanged;
		_row.Child = child;
	}

	private void OnChanged(Adw.ButtonRow sender, EventArgs args)
	{
		Callback?.Invoke();
		ValueCallback?.Invoke(this, new InputChangedEventArgs<object>(Name, _value));
	}

	public override Adw.ButtonRow Row => _row;
	public override object Value => _value!;

	public override void SetValue(object value)
	{
		_value = value;
	}

	public override void Dispose()
	{
		Row.OnActivated -= OnChanged;
		if (_value is IDisposable disposable)
		{
			disposable.Dispose();
		}
		Row.Child?.Dispose();
		base.Dispose();
	}
}
