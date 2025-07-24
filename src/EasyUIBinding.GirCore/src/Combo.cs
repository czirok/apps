namespace EasyUIBinding.GirCore;

public class Combo<TKey> : InputBindingDictionary<Adw.ComboRow, TKey>, IDisposable where TKey : notnull
{
	private readonly Adw.ComboRow _row;
	private readonly Gio.ListStore _store;
	private readonly Dictionary<int, TKey> _indexToKey = [];
	private readonly Dictionary<TKey, int> _keyToIndex = [];
	private readonly IDictionary<TKey, string> _inputs = new Dictionary<TKey, string>();
	private readonly TKey? _default = default!;

	public Combo(string title, IDictionary<TKey, string> options, TKey? @default = default, TKey? selected = default)
		: this(Guid.NewGuid().ToString(), title, options, @default, selected)
	{
	}

	public Combo(string name, string title, IDictionary<TKey, string> options, TKey? @default = default, TKey? selected = default)
	{
		Name = name;
		_inputs = options;
		_default = @default;
		selected ??= _default;

		_row = Adw.ComboRow.New();
		_row.Title = title;

		_store = Gio.ListStore.New(Gtk.StringObject.GetGType());

		int index = 0;
		foreach (var option in options)
		{
			_store.Append(Gtk.StringObject.New(option.Value));

			_indexToKey[index] = option.Key;
			_keyToIndex[option.Key] = index;

			index++;
		}

		_row.Model = _store;

		if (selected != null && _keyToIndex.ContainsKey(selected))
		{
			_row.Selected = (uint)_keyToIndex[selected];
		}

		_row.OnNotify += OnChanged;
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
		if (args.Pspec.GetName() == "selected")
		{
			var selectedIndex = (int)((Adw.ComboRow)sender).Selected;

			if (selectedIndex >= 0 && _indexToKey.ContainsKey(selectedIndex))
			{
				var key = _indexToKey[selectedIndex];
				var value = _inputs[key];

				UpdateAllBoundProperties(key, value);
				InvokeCallback(this, new InputDictionaryChangedEventArgs<TKey>(Name, key, value));
			}
		}
	}

	public override Adw.ComboRow Row => _row;
	public override IDictionary<TKey, string> Values => _inputs;
	public override TKey? Default => _default;

	public override TKey? Selected
	{
		get
		{
			var selectedIndex = (int)_row.Selected;
			if (selectedIndex >= 0 && _indexToKey.ContainsKey(selectedIndex))
			{
				return _indexToKey[selectedIndex];
			}
			return default;
		}
	}

	public override void SetSelected(TKey key, string value)
	{
		if (_keyToIndex.ContainsKey(key))
		{
			_row.Selected = (uint)_keyToIndex[key];
		}
		else
		{
			throw new ArgumentException($"Key '{key}' not found in combo options.", nameof(key));
		}
	}

	public override void Dispose()
	{
		_row.OnNotify -= OnChanged;

		_indexToKey.Clear();
		_keyToIndex.Clear();

		_store.Dispose();
		base.Dispose();
	}
}