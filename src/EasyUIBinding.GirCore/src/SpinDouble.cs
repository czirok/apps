namespace EasyUIBinding.GirCore;

public class SpinDouble : InputBinding<Adw.SpinRow, double>, IDisposable
{
	private readonly Adw.SpinRow _row;
	private bool _isUpdatingFromBinding = false;
	private double _lastValue;
	private readonly DoubleRange range;
	private readonly int _decimalPlaces;
	private readonly double _tolerance;

	public SpinDouble(string title, double initialValue, DoubleRange range)
		: this(Guid.NewGuid().ToString(), title, initialValue, range)
	{
	}

	public SpinDouble(string name, string title, double initialValue, DoubleRange range)
	{
		Name = name;
		_lastValue = initialValue;
		this.range = range;
		_decimalPlaces = GetDecimalPlaces(range.Step);
		_tolerance = range.Step / 2f;

		var adjustment = Gtk.Adjustment.New(initialValue, range.Min, range.Max, stepIncrement: (double)range.Step, pageIncrement: 1, pageSize: 0);
		_row = new Adw.SpinRow
		{
			Adjustment = adjustment,
			Title = title,
			Activatable = false,
			Value = initialValue,
			Digits = (uint)_decimalPlaces
		};
		_row.OnNotify += OnChanged;
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is double floatValue)
		{
			var roundedValue = Round(floatValue);
			if (Math.Abs(roundedValue - _lastValue) > _tolerance)
			{
				_isUpdatingFromBinding = true;
				_row.Value = roundedValue;
				_lastValue = roundedValue;
				_isUpdatingFromBinding = false;
			}
		}
	}

	private void OnChanged(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "value" && !_isUpdatingFromBinding)
		{
			var value = (double)((Adw.SpinRow)sender).Value;

			var roundedValue = Round(value);
			if (Math.Abs(roundedValue - _lastValue) > _tolerance)
			{
				_lastValue = roundedValue;
				UpdateBoundProperty(roundedValue);
				InvokeCallback(this, new InputChangedEventArgs<double>(Name, roundedValue));
			}
		}
	}

	public override Adw.SpinRow Row => _row;
	public override double Value => _lastValue;

	public override void SetValue(double value)
	{
		var roundedValue = Round(value);
		if (Math.Abs(roundedValue - _lastValue) > _tolerance)
		{
			_isUpdatingFromBinding = true;
			_row.Value = roundedValue;
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
		Row.OnNotify -= OnChanged;
		base.Dispose();
	}
}
public class DoubleRange(double min, double max, double step = 0.1f)
{
	public double Min => min;
	public double Max => max;
	public double Step => step;
}
