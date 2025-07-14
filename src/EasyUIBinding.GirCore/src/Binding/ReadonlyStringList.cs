namespace EasyUIBinding.GirCore.Binding;

public class ReadonlyStringList : InputList<Adw.ActionRow, string>, IDisposable
{
	private readonly Adw.ActionRow _row;
	private readonly IList<string> _inputs;
	private readonly string? _default;

	public ReadonlyStringList(string name, string title, string[] values, string? selected = null)
	{
		Name = name;
		_inputs = values;
		_default = selected;
		_row = Adw.ActionRow.New();
		_row.Title = title;
		_row.Subtitle = Name;
		_row.Activatable = false;
		var listBox = Gtk.Box.New(Gtk.Orientation.Vertical, 6);
		foreach (var value in values)
		{
			var label = Gtk.Label.New(value);
			label.Xalign = 0f;
			listBox.Append(label);
		}
		_row.AddSuffix(listBox);
	}

	public override Adw.ActionRow Row => _row;
	public override IList<string> Values => _inputs;

	public override void Dispose()
	{
		base.Dispose();
	}
}