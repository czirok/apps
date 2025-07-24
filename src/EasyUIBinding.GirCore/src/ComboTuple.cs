namespace EasyUIBinding.GirCore;

public class ComboTuple<TValue> : InputBindingTuple<Adw.ComboRow, TValue>, IDisposable where TValue : notnull
{
	private readonly Adw.ComboRow _row;
	private readonly Gio.ListStore _store;
	private readonly (string Display, TValue Value)[] _tuples;
	private readonly Dictionary<int, (string Display, TValue Value)> _indexToTuple = new();
	private readonly Dictionary<TValue, int> _valueToIndex = new();
	private readonly TValue? _default = default!;

	public ComboTuple((string Display, TValue Value)[] options, TValue? @default = default, TValue? selected = default)
		: this(Guid.NewGuid().ToString(), options, @default, selected)
	{
	}

	public ComboTuple(string title, (string Display, TValue Value)[] options, TValue? @default = default, TValue? selected = default)
		: this(Guid.NewGuid().ToString(), title, options, @default, selected)
	{
	}

	public ComboTuple(string name, string title, (string Display, TValue Value)[] options, TValue? @default = default, TValue? selected = default)
	{
		Name = name;
		_tuples = options;
		_default = @default;
		selected ??= _default;

		_row = Adw.ComboRow.New();
		_row.Title = title;

		_store = Gio.ListStore.New(Gtk.StringObject.GetGType());

		// Build lookup tables and populate store
		for (int index = 0; index < options.Length; index++)
		{
			var (display, value) = options[index];

			// Add display text to the UI store
			_store.Append(Gtk.StringObject.New(display));

			// Build lookup tables
			_indexToTuple[index] = (display, value);

			// Note: If duplicate values exist, last one wins in _valueToIndex
			_valueToIndex[value] = index;
		}

		_row.Model = _store;

		// Set initial selection
		if (selected != null && _valueToIndex.ContainsKey(selected))
		{
			_row.Selected = (uint)_valueToIndex[selected];
		}

		_row.OnNotify += OnChanged;
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is TValue tValue && _valueToIndex.ContainsKey(tValue))
		{
			_row.Selected = (uint)_valueToIndex[tValue];
		}
	}

	private void OnChanged(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "selected")
		{
			var selectedIndex = (int)((Adw.ComboRow)sender).Selected;

			if (selectedIndex >= 0 && _indexToTuple.ContainsKey(selectedIndex))
			{
				var (display, value) = _indexToTuple[selectedIndex];

				// Update bound properties: first gets TValue, second gets Display string
				UpdateAllBoundProperties(value, display);

				// Fire callback with tuple info
				InvokeCallback(this, new InputTupleChangedEventArgs<TValue>(Name, value, display));
			}
		}
	}

	public override Adw.ComboRow Row => _row;

	public override (string Display, TValue Value)[] Tuples => _tuples;

	public override TValue? Default => _default;

	public override TValue? Selected
	{
		get
		{
			var selectedIndex = (int)_row.Selected;
			if (selectedIndex >= 0 && _indexToTuple.ContainsKey(selectedIndex))
			{
				return _indexToTuple[selectedIndex].Value;
			}
			return default;
		}
	}

	public override string? SelectedDisplay
	{
		get
		{
			var selectedIndex = (int)_row.Selected;
			if (selectedIndex >= 0 && _indexToTuple.ContainsKey(selectedIndex))
			{
				return _indexToTuple[selectedIndex].Display;
			}
			return default;
		}
	}

	public override void SetSelected(TValue value)
	{
		if (_valueToIndex.ContainsKey(value))
		{
			_row.Selected = (uint)_valueToIndex[value];
		}
		else
		{
			throw new ArgumentException($"Value '{value}' not found in combo options.", nameof(value));
		}
	}

	public override void Dispose()
	{
		_row.OnNotify -= OnChanged;

		_indexToTuple.Clear();
		_valueToIndex.Clear();

		_store.Dispose();
		base.Dispose();
	}
}
