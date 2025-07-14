using EasyUIBinding.GirCore.Binding;

namespace EasyUIBinding.GirCore.Widget;

public class Button : UI<Gtk.Button, object>, IDisposable
{
	private readonly string _name;
	private readonly Gtk.Button _widget;
	private readonly Action<object, InputChangedEventArgs>? _changed;
	private object? _value;

	public Button(string name, Gtk.Widget child, object value, string[] css, Action<object, InputChangedEventArgs>? changed = null)
	{
		_name = name;
		_value = value;
		_changed = changed;
		_widget = new Gtk.Button();
		_widget.SetCssClasses(css);
		_widget.OnClicked += OnChanged;
		_widget.Child = child;
	}

	private void OnChanged(Gtk.Button sender, EventArgs args)
	{
		_changed?.Invoke(this, new InputChangedEventArgs(_name, _value));
	}

	public override Gtk.Button Widget => _widget;
	public override string Name => _name;
	public override object Value => _value!;

	public override void SetValue(object value)
	{
		_value = value;
	}

	public override void Dispose()
	{
		_widget.OnClicked -= OnChanged;
		base.Dispose();
	}
}
