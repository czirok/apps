using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace EasyUIBinding.GirCore;

public class WidgetBinder(object widget) : IDisposable
{
	private readonly WeakReference _widgetRef = new(widget);
	private readonly HashSet<INotifyPropertyChanged> _boundTargets = [];
	private readonly List<(INotifyPropertyChanged target, string property, string widgetProperty)> _boundObjects = [];

	public WidgetBinder BindTo(INotifyPropertyChanged target, string propertyName, string widgetPropertyName)
	{
		_boundObjects.Add((target, propertyName, widgetPropertyName));

		if (_boundTargets.Add(target))
		{
			target.PropertyChanged += OnBoundObjectPropertyChanged;
		}

		var initialValue = GetPropertyValue(target, propertyName);
		if (initialValue != null)
			SetValueFromBinding(widgetPropertyName, initialValue);

		return this;
	}

	private void OnBoundObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (!_widgetRef.IsAlive)
		{
			Dispose();
			return;
		}

		foreach (var (target, property, widgetProperty) in _boundObjects)
		{
			if (sender == target && e.PropertyName == property)
			{
				var value = GetPropertyValue(target, property);
				SetValueFromBinding(widgetProperty, value);
				break;
			}
		}
	}

	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Property access is safe as we control the types involved.")]
	private static object? GetPropertyValue(object target, string propertyName)
	{
		var property = target.GetType().GetProperty(propertyName)
			?? throw new ArgumentException($"Property '{propertyName}' not found in '{target.GetType().Name}'.");
		return property.GetValue(target);
	}

	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Property access is safe as we control the types involved.")]
	private void SetValueFromBinding(string propertyName, object? value)
	{
		var widget = _widgetRef.Target;
		if (widget == null) return;

		var property = widget.GetType().GetProperty(propertyName)
			?? throw new ArgumentException($"Property '{propertyName}' not found in '{widget.GetType().Name}'.");
		property.SetValue(widget, value);
	}

	public void Dispose()
	{
		foreach (var target in _boundTargets)
			target.PropertyChanged -= OnBoundObjectPropertyChanged;
		_boundTargets.Clear();
		_boundObjects.Clear();
	}
}