namespace EasyUIBinding.GirCore;

public class WrapToggle<TKey> : InputBindingDictionary<Adw.ActionRow, TKey>, IDisposable where TKey : notnull
{
	private readonly Adw.ActionRow _row;
	private readonly Gtk.Box _container;
	private readonly Gtk.Label? _titleLabel;
	private readonly Adw.WrapBox _group;
	private readonly List<Widget.Button> _toggles = [];
	private Widget.Button _selected = default!;
	private readonly IDictionary<TKey, string> _inputs = new Dictionary<TKey, string>();
	private readonly TKey? _default = default!;

	public WrapToggle(IDictionary<TKey, string> toggles, TKey? @default = default, TKey? selected = default)
		: this(Guid.NewGuid().ToString(), default!, toggles, @default, selected)
	{ }

	public WrapToggle(string name, string title, IDictionary<TKey, string> toggles, TKey? @default = default, TKey? selected = default)
	{
		Name = name;
		_inputs = toggles;
		_default = @default;
		selected ??= _default;

		_row = Adw.ActionRow.New();
		_row.Activatable = false;

		// Vertical container
		_container = Gtk.Box.New(Gtk.Orientation.Vertical, 6);
		_container.MarginTop = 6;
		_container.MarginBottom = 6;

		if (!string.IsNullOrEmpty(title))
		{
			// Title label
			_titleLabel = Gtk.Label.New(title);
			_titleLabel.SetHalign(Gtk.Align.Start);
		}

		// WrapBox for toggles
		_group = Adw.WrapBox.New();
		_group.Vexpand = false;
		_group.MarginTop = 6;
		_group.ChildSpacing = 6;
		_group.LineSpacing = 6;

		foreach (var toggleName in _inputs)
		{
			var toggle = new Widget.Button(name, Gtk.Label.New(toggleName.Value), toggleName.Key, [], OnChanged);

			_group.Append(toggle.Widget);
			_toggles.Add(toggle);

			if (toggleName.Key.Equals(selected))
			{
				toggle.Widget.AddCssClass("suggested-action");
				_selected = toggle;
			}
		}

		// Assemble layout: title on top, toggles below
		if (_titleLabel is not null)
		{
			_container.Append(_titleLabel);
		}
		_container.Append(_group);

		// Add the whole container to the ActionRow
		_row.AddPrefix(_container);
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is TKey key)
		{
			if (_inputs.ContainsKey(key) && _inputs.TryGetValue(key, out var stringValue))
			{
				SetSelected(key, stringValue);
			}
		}
	}

	private void OnChanged(object sender, InputChangedEventArgs args)
	{
		foreach (var toggle in _toggles)
		{
			toggle.Widget.RemoveCssClass("suggested-action");
		}
		_selected = (Widget.Button)sender;
		_selected.Widget.AddCssClass("suggested-action");
		var key = Values.FirstOrDefault(x => x.Key.Equals(_selected.Value)).Key;

		UpdateAllBoundProperties(key, Values[key]);
		InvokeCallback(this, new InputDictionaryChangedEventArgs<TKey>(Name, key, Values[key]));
	}

	public override Adw.ActionRow Row => _row;
	public override IDictionary<TKey, string> Values => _inputs;
	public override void SetSelected(TKey key, string value)
	{
		foreach (var toggle in _toggles)
		{
			toggle.Widget.RemoveCssClass("suggested-action");
		}
		if (_inputs.ContainsKey(key))
		{
			_selected = _toggles.FirstOrDefault(t => key.Equals(t.Value)) ?? _toggles.First();
			_selected.Widget.AddCssClass("suggested-action");
		}
		else
		{
			_selected = _toggles.First();
			_selected.Widget.AddCssClass("suggested-action");
		}
	}
	public override TKey? Selected => (TKey)_selected.Value;
	public override TKey? Default => _default;

	public override void Dispose()
	{
		foreach (var toggle in _toggles)
			toggle.Dispose();

		_titleLabel?.Dispose();
		_group.Dispose();
		_container.Dispose();
		base.Dispose();
	}
}