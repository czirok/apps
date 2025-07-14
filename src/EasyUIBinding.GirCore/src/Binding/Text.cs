namespace EasyUIBinding.GirCore.Binding;

public class Text : InputBinding<Adw.EntryRow, string>, IDisposable
{
	private readonly Adw.EntryRow _row;

	public Text(string name, string title, string value)
	{
		Name = name;
		_row = Adw.EntryRow.New();

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
			UpdateBoundProperty(((Adw.EntryRow)sender).GetText());
			InvokeCallback(sender, new InputChangedEventArgs<string>(Name, ((Adw.EntryRow)sender).GetText()));
		}
	}

	public override Adw.EntryRow Row => _row;
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

