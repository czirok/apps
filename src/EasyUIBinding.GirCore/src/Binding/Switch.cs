namespace EasyUIBinding.GirCore.Binding;

public class Switch : InputBinding<Adw.SwitchRow, bool>, IDisposable
{
	private readonly Adw.SwitchRow _row;

	public Switch(string name, string title, bool? active = false)
	{
		Name = name;
		_row = Adw.SwitchRow.New();
		_row.Title = title;
		_row.Active = active ?? false;
		_row.OnNotify += OnChanged;
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is bool boolValue)
			_row.Active = boolValue;
	}

	private void OnChanged(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "active")
		{
			UpdateBoundProperty(((Adw.SwitchRow)sender).Active);
			InvokeCallback(sender, new InputChangedEventArgs(Name, ((Adw.SwitchRow)sender).Active));
		}
	}

	public override Adw.SwitchRow Row => _row;
	public override bool Value => _row.Active;
	public override void SetValue(bool value)
	{
		_row.Active = value;
	}

	public override void Dispose()
	{
		Row.OnNotify -= OnChanged;
		base.Dispose();
		GC.SuppressFinalize(this);
	}
}
