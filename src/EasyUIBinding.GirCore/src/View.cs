namespace EasyUIBinding.GirCore;

public class View : Input<Adw.ActionRow, string>, IDisposable
{
	private readonly Adw.ActionRow _row;

	public View(string title, string? value = null)
		: this(Guid.NewGuid().ToString(), title, value)
	{
	}

	public View(string name, string title, string? value = null)
	{
		Name = name;
		_row = Adw.ActionRow.New();
		_row.Activatable = false;
		_row.Title = title;
		_row.Subtitle = value;
	}

	public override Adw.ActionRow Row => _row;
	public override string Value => _row.Subtitle!;
	public override void SetValue(string value)
	{
		_row.Subtitle = value;
	}

	public override void Dispose()
	{
		base.Dispose();
	}
}

