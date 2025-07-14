namespace EasyUIBinding.GirCore.Binding;

public class SpinInteger : InputBinding<Adw.SpinRow, int>, IDisposable
{
	private readonly Adw.SpinRow _row;
	private bool _isUpdatingFromBinding = false;
	private int _lastValue;
	private readonly IntRange _range;

	public SpinInteger(string name, string title, int initialValue, IntRange range)
	{
		Name = name;
		_lastValue = initialValue;
		_range = range;

		var adjustment = Gtk.Adjustment.New(initialValue, range.Min, range.Max, stepIncrement: range.Step, pageIncrement: range.Step * 10, pageSize: 0);
		_row = new Adw.SpinRow
		{
			Adjustment = adjustment,
			Title = title,
			Activatable = false,
			Value = initialValue,
			Digits = 0
		};
		_row.OnNotify += OnChanged;
	}

	protected override void SetValueFromBinding(object value)
	{
		if (value is int intValue)
		{
			var roundedValue = Round(intValue);
			if (roundedValue != _lastValue)
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
			var value = (int)((Adw.SpinRow)sender).Value;
			var roundedValue = Round(value);

			if (roundedValue != _lastValue)
			{
				_lastValue = roundedValue;
				UpdateBoundProperty(roundedValue);
				InvokeCallback(sender, new InputChangedEventArgs<int>(Name, roundedValue));
			}
		}
	}

	public override Adw.SpinRow Row => _row;
	public override int Value => _lastValue;

	public override void SetValue(int value)
	{
		var roundedValue = Round(value);
		if (roundedValue != _lastValue)
		{
			_isUpdatingFromBinding = true;
			_row.Value = roundedValue;
			_lastValue = roundedValue;
			_isUpdatingFromBinding = false;
		}
	}

	private int Round(int value)
	{
		return value / _range.Step * _range.Step;
	}

	public override void Dispose()
	{
		Row.OnNotify -= OnChanged;
		base.Dispose();
	}
}

public class IntRange(int min, int max, int step = 1)
{
	public int Min => min;
	public int Max => max;
	public int Step => step;
}