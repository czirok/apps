namespace EasyUIBinding.GirCore;

public class ScaleDouble : InputBinding<Adw.ActionRow, double>, IDisposable
{
	private readonly Adw.ActionRow _row;
	private readonly Gtk.Scale _scale;
	private bool _isUpdatingFromBinding = false;
	private double _lastValue;
	private readonly DoubleRange range;
	private readonly int _decimalPlaces;
	private readonly double _tolerance;

	public ScaleDouble(string title, double initialValue, DoubleRange range, Gtk.Orientation orientation = Gtk.Orientation.Horizontal, bool showValue = true)
		: this(Guid.NewGuid().ToString(), title, initialValue, range, orientation, showValue)
	{
	}

	public ScaleDouble(string name, string title, double initialValue, DoubleRange range, Gtk.Orientation orientation = Gtk.Orientation.Horizontal, bool showValue = true)
	{
		Name = name;
		_lastValue = initialValue;
		this.range = range;
		_decimalPlaces = GetDecimalPlaces(range.Step);
		_tolerance = range.Step / 2f;

		_row = Adw.ActionRow.New();
		_row.Title = title;
		_row.Activatable = false;

		_scale = Gtk.Scale.NewWithRange(orientation, range.Min, range.Max, range.Step);
		_scale.SetValue(Round(initialValue));
		_scale.DrawValue = showValue;
		_scale.Digits = _decimalPlaces;
		_scale.Hexpand = true;
		_scale.Vexpand = orientation == Gtk.Orientation.Vertical;

		if (orientation == Gtk.Orientation.Horizontal)
		{
			_scale.MarginStart = 12;
			_scale.MarginEnd = 12;
		}
		else
		{
			_scale.MarginTop = 12;
			_scale.MarginBottom = 12;
		}

		_scale.OnValueChanged += OnChanged;
		_row.AddSuffix(_scale);
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is double doubleValue)
		{
			var roundedValue = Round(doubleValue);
			if (Math.Abs(roundedValue - _lastValue) > _tolerance)
			{
				_isUpdatingFromBinding = true;
				_scale.SetValue(roundedValue);
				_lastValue = roundedValue;
				_isUpdatingFromBinding = false;
			}
		}
	}

	private void OnChanged(Gtk.Range sender, EventArgs args)
	{
		if (!_isUpdatingFromBinding)
		{
			var value = sender.GetValue();
			var roundedValue = Round(value);

			if (Math.Abs(roundedValue - _lastValue) > _tolerance)
			{
				_lastValue = roundedValue;
				UpdateBoundProperty(roundedValue);
				InvokeCallback(this, new InputChangedEventArgs<double>(Name, roundedValue));
			}
		}
	}

	public override Adw.ActionRow Row => _row;
	public override double Value => _lastValue;

	public override void SetValue(double value)
	{
		var roundedValue = Round(value);
		if (Math.Abs(roundedValue - _lastValue) > _tolerance)
		{
			_isUpdatingFromBinding = true;
			_scale.SetValue(roundedValue);
			_lastValue = roundedValue;
			_isUpdatingFromBinding = false;
		}
	}

	private static int GetDecimalPlaces(double step)
	{
		double temp = step;
		int places = 0;

		while (temp != Math.Floor(temp) && places < 10)
		{
			temp *= 10;
			places++;
		}

		return places;
	}

	private double Round(double value)
	{
		var steps = Math.Round(value / range.Step);
		return Math.Round(steps * range.Step, _decimalPlaces);
	}

	public override void Dispose()
	{
		_scale.OnValueChanged -= OnChanged;
		_scale.Dispose();
		base.Dispose();
	}
}