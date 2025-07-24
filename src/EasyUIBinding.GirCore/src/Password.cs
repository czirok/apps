namespace EasyUIBinding.GirCore;

public class Password : InputBinding<Adw.PasswordEntryRow, string>, IDisposable
{
	private readonly Adw.PasswordEntryRow _row;

	public Password(string title, string value = "")
		: this(Guid.NewGuid().ToString(), title, value)
	{
	}

	public Password(string name, string title, string value = "")
	{
		Name = name;
		_row = Adw.PasswordEntryRow.New();

		_row.Title = title;
		_row.SetText(value);
		_row.ShowApplyButton = true;
		_row.OnNotify += OnChanged;
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is string strValue && _row.GetText() != strValue)
		{
			_row.SetText(strValue);
		}
	}

	private void OnChanged(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "text")
		{
			UpdateBoundProperty(((Adw.PasswordEntryRow)sender).GetText());
			InvokeCallback(this, new InputChangedEventArgs<string>(Name, ((Adw.PasswordEntryRow)sender).GetText()));
		}
	}

	public override Adw.PasswordEntryRow Row => _row;
	public override string Value => _row.GetText();
	public override void SetValue(string value)
	{
		if (_row.GetText() != value)
		{
			_row.SetText(value);
		}
	}

	public override void Dispose()
	{
		Row.OnNotify -= OnChanged;
		base.Dispose();
	}
}