namespace EasyUIBinding.GirCore;

public class Toggle<TKey> : InputBindingDictionary<Adw.ActionRow, TKey>, IDisposable where TKey : notnull
{
	private readonly Adw.ActionRow _row;
	private readonly Adw.ToggleGroup _group;
	private readonly List<Adw.Toggle> _toggles = [];
	private readonly Dictionary<Adw.Toggle, TKey> _toggleToKey = new();
	private readonly IDictionary<TKey, string> _inputs = new Dictionary<TKey, string>();
	private readonly TKey? _default = default!;

	public Toggle(string title, IDictionary<TKey, string> toggles, TKey? @default = default, TKey? selected = default)
		: this(Guid.NewGuid().ToString(), title, toggles, @default, selected)
	{
	}

	public Toggle(string name, string title, IDictionary<TKey, string> toggles, TKey? @default = default, TKey? selected = default)
	{
		Name = name;
		_inputs = toggles;
		_default = @default;
		selected ??= _default;
		_row = Adw.ActionRow.New();
		_row.Title = title;
		_row.Valign = Gtk.Align.Center;
		_row.Activatable = false;

		_group = Adw.ToggleGroup.New();
		_group.Vexpand = false;
		_group.Valign = Gtk.Align.Center;
		_group.Halign = Gtk.Align.End;

		foreach (var toggleName in toggles)
		{
			var toggle = Adw.Toggle.New();
			toggle.Label = toggleName.Value;
			toggle.Name = toggleName.Value;

			_toggleToKey[toggle] = toggleName.Key;

			_group.Add(toggle);
			_toggles.Add(toggle);

			if (toggleName.Key.Equals(selected))
			{
				_group.Active = (uint)_toggles.IndexOf(toggle);
			}
		}

		_group.OnNotify += OnChanged;
		_row.AddSuffix(_group);
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

	private void OnChanged(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "active")
		{
			var activeIndex = (int)((Adw.ToggleGroup)sender).Active;

			if (activeIndex >= 0 && activeIndex < _toggles.Count)
			{
				var activeToggle = _toggles[activeIndex];
				var key = _toggleToKey[activeToggle];
				var value = _inputs[key];

				UpdateAllBoundProperties(key, value);
				InvokeCallback(this, new InputDictionaryChangedEventArgs<TKey>(Name, key, value));
			}
		}
	}

	public override Adw.ActionRow Row => _row;
	public override IDictionary<TKey, string> Values => _inputs;
	public override TKey? Default => _default;

	public override TKey? Selected
	{
		get
		{
			var activeIndex = (int)_group.Active;
			if (activeIndex >= 0 && activeIndex < _toggles.Count)
			{
				var activeToggle = _toggles[activeIndex];
				return _toggleToKey[activeToggle];
			}
			return default;
		}
	}

	public override void SetSelected(TKey key, string value)
	{
		if (_inputs.ContainsKey(key))
		{
			var targetToggle = _toggleToKey.FirstOrDefault(kvp => kvp.Value.Equals(key)).Key;
			if (targetToggle != null)
			{
				var index = _toggles.IndexOf(targetToggle);
				if (index >= 0)
				{
					_group.Active = (uint)index;
				}
			}
		}
	}

	public override void Dispose()
	{
		_group.OnNotify -= OnChanged;

		foreach (var toggle in _toggles)
			toggle.Dispose();

		_toggleToKey.Clear();
		_group.Dispose();
		base.Dispose();
	}
}