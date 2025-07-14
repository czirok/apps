namespace EasyUIBinding.GirCore.Binding;

public class SaveAsSelector : InputBinding<Adw.ActionRow, string>, IDisposable
{
	private readonly Adw.ActionRow _row;
	private readonly Gtk.FileDialog _dialog;
	private string? _value;

	public SaveAsSelector(
		string name,
		string title,
		string? initialPath = null,
		Gtk.FileFilter? fileFilter = null)
	{
		Name = name;

		_row = Adw.ActionRow.New();
		_row.Title = title;
		_row.Activatable = true;
		_row.OnActivated += OnChanged;

		_dialog = Gtk.FileDialog.New();
		if (fileFilter is not null)
			_dialog.SetDefaultFilter(fileFilter);
		if (!string.IsNullOrEmpty(initialPath))
			_dialog.SetInitialFile(Gio.FileHelper.NewForPath(initialPath));
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is string path)
		{
			_dialog.SetInitialFile(Gio.FileHelper.NewForPath(path ?? string.Empty));
		}
	}

	private async void OnChanged(Adw.ActionRow sender, EventArgs args)
	{
		try
		{
			var file = await _dialog.SaveAsync((Gtk.Window?)sender.GetRoot());
			if (file is not null)
			{
				_value = file.GetPath();
				_dialog.SetInitialFile(file);

				if (string.IsNullOrEmpty(_value))
				{
					InvokeCallback(this, new InputChangedEventArgs<string>(Name, null));
					return;
				}
				UpdateBoundProperty(_value);
				InvokeCallback(this, new InputChangedEventArgs<string>(Name, _value));
			}
		}
		catch (Exception)
		{
			InvokeCallback(this, new InputChangedEventArgs<string>(Name, null));
		}
	}

	public override Adw.ActionRow Row => _row;
	public override string Value => _value ?? string.Empty;
	public override void SetValue(string value)
	{
		_value = value;
	}

	public override void Dispose()
	{
		_dialog.Dispose();
		Row.OnActivated -= OnChanged;
		base.Dispose();
	}
}
