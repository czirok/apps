namespace EasyUIBinding.GirCore;

public class SpinFloat : InputBinding<Adw.SpinRow, float>, IDisposable
{
	private readonly Adw.SpinRow _row;
	private bool _isUpdatingFromBinding = false;
	private float _lastValue;
	private readonly FloatRange range;
	private readonly int _decimalPlaces;
	private readonly float _tolerance;

	public SpinFloat(string title, float initialValue, FloatRange range)
		: this(Guid.NewGuid().ToString(), title, initialValue, range)
	{
	}

	public SpinFloat(string name, string title, float initialValue, FloatRange range)
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
		if (value is float floatValue)
		{
			var roundedValue = Round(floatValue);
			if (MathF.Abs(roundedValue - _lastValue) > _tolerance)
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
			var value = (float)((Adw.SpinRow)sender).Value;

			var roundedValue = Round(value);
			if (MathF.Abs(roundedValue - _lastValue) > _tolerance)
			{
				_lastValue = roundedValue;
				UpdateBoundProperty(roundedValue);
				InvokeCallback(this, new InputChangedEventArgs<float>(Name, roundedValue));
			}
		}
	}

	public override Adw.SpinRow Row => _row;
	public override float Value => _lastValue;

	public override void SetValue(float value)
	{
		var roundedValue = Round(value);
		if (MathF.Abs(roundedValue - _lastValue) > _tolerance)
		{
			_isUpdatingFromBinding = true;
			_row.Value = roundedValue;
			_lastValue = roundedValue;
			_isUpdatingFromBinding = false;
		}
	}

	private static int GetDecimalPlaces(float step)
	{
		float temp = step;
		int places = 0;

		while (temp != MathF.Floor(temp) && places < 10)
		{
			temp *= 10;
			places++;
		}

		return places;
	}

	private float Round(float value)
	{
		var steps = MathF.Round(value / range.Step);
		return MathF.Round(steps * range.Step, _decimalPlaces);
	}

	public override void Dispose()
	{
		Row.OnNotify -= OnChanged;
		base.Dispose();
	}
}
public class FloatRange(float min, float max, float step = 0.1f)
{
	public float Min => min;
	public float Max => max;
	public float Step => step;
}
