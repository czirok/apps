using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace EasyUIBinding.GirCore;

public abstract class InputTuple<TRow, TValue> : Input
	where TRow : Adw.PreferencesRow
{
	public abstract override TRow Row { get; }
	public abstract (string Display, TValue Value)[] Tuples { get; }
	public abstract TValue? Default { get; }
	public abstract TValue? Selected { get; }
	public abstract string? SelectedDisplay { get; }
	public abstract void SetSelected(TValue value);
}

public abstract class InputBindingTuple<TRow, TValue> : InputTuple<TRow, TValue>
	where TRow : Adw.PreferencesRow
{
	private readonly HashSet<INotifyPropertyChanged> _boundTargets = [];
	protected readonly List<(INotifyPropertyChanged target, string property)> BoundObjects = [];

	private bool _isTupleBinding = false;
	private (INotifyPropertyChanged target, string property)? _tupleBinding = null;

	public void BindToProperties(INotifyPropertyChanged target, string propertyName)
	{
		BoundObjects.Add((target, propertyName));

		if (_boundTargets.Add(target))
		{
			target.PropertyChanged += OnBoundObjectPropertyChanged;
		}

		if (BoundObjects.Count == 1)
		{
			var initialValue = target.GetPropertyValue(propertyName);
			if (initialValue is not null)
				SetValueFromBinding(initialValue);
		}
	}

	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Property access is safe as we control the types involved.")]
	public void BindToTuple(INotifyPropertyChanged target, string propertyName)
	{
		_isTupleBinding = true;
		_tupleBinding = (target, propertyName);

		if (_boundTargets.Add(target))
		{
			target.PropertyChanged += OnBoundObjectPropertyChanged;
		}

		var initialTuple = target.GetPropertyValue(propertyName);
		if (initialTuple != null)
		{
			var tupleType = initialTuple.GetType();
			if (tupleType.IsGenericType && tupleType.GetGenericTypeDefinition() == typeof(ValueTuple<,>))
			{
				var item2Property = tupleType.GetProperty("Item2");
				var value = item2Property?.GetValue(initialTuple);
				if (value is TValue tValue)
					SetValueFromBinding(tValue);
			}
		}
	}

	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Property access is safe as we control the types involved.")]
	protected void OnBoundObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_isTupleBinding && _tupleBinding.HasValue)
		{
			var (target, property) = _tupleBinding.Value;
			if (sender == target && e.PropertyName == property)
			{
				var tupleValue = target.GetPropertyValue(property);
				if (tupleValue != null)
				{
					var tupleType = tupleValue.GetType();
					if (tupleType.IsGenericType && tupleType.GetGenericTypeDefinition() == typeof(ValueTuple<,>))
					{
						var item2Property = tupleType.GetProperty("Item2");
						var value = item2Property?.GetValue(tupleValue);
						if (value is TValue tValue)
							SetValueFromBinding(tValue);
					}
				}
			}
		}
		else
		{
			foreach (var (target, property) in BoundObjects)
			{
				if (sender == target && e.PropertyName == property)
				{
					var propertyValue = target.GetPropertyValue(property);
					if (propertyValue is not null)
						SetValueFromBinding(propertyValue);
					break;
				}
			}
		}
	}

	protected void UpdateAllBoundProperties(TValue value, string display)
	{
		if (_isTupleBinding && _tupleBinding.HasValue)
		{
			var (target, property) = _tupleBinding.Value;
			var tuple = (display, value);
			target.SetPropertyValue(property, tuple);
		}
		else
		{
			if (BoundObjects.Count == 0) return;

			for (int i = 0; i < BoundObjects.Count; i++)
			{
				var (target, property) = BoundObjects[i];

				if (i == 0 && value is not null)
					target.SetPropertyValue(property, value);
				else if (i == 1 && display is not null)
					target.SetPropertyValue(property, display);
			}
		}
	}

	protected abstract void SetValueFromBinding(object value);

	public override void Dispose()
	{
		foreach (var target in _boundTargets)
		{
			target.PropertyChanged -= OnBoundObjectPropertyChanged;
		}

		_boundTargets.Clear();
		BoundObjects.Clear();
		_tupleBinding = null;

		base.Dispose();
	}
}