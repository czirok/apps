using System.ComponentModel;

namespace EasyUIBinding.GirCore;

public abstract class Input : NotifyPropertyModel, IDisposable
{
	public string Name { get; set; } = default!;
	public abstract Adw.PreferencesRow Row { get; }

	protected Action? Callback;
	protected Action<object, InputChangedEventArgs>? ValueCallback;
	public void SetCallback(Action callback) => Callback = callback;
	public void SetValueCallback(Action<object, InputChangedEventArgs> callback) => ValueCallback = callback;

	public virtual void Dispose()
	{
		Row.Dispose();
		GC.SuppressFinalize(this);
	}

	public void InvokeCallback(object sender, InputChangedEventArgs args)
	{
		Callback?.Invoke();
		ValueCallback?.Invoke(sender, args);
	}
}

public abstract class Input<TRow, TValue> : Input
	where TRow : Adw.PreferencesRow
{
	public abstract override TRow Row { get; }
	public abstract TValue Value { get; }
	public abstract void SetValue(TValue value);
}

public abstract class InputBinding<TRow, TValue> : Input<TRow, TValue>
	where TRow : Adw.PreferencesRow
{
	protected INotifyPropertyChanged? BoundObject;
	protected string? BoundProperty;

	public void Bind(INotifyPropertyChanged target, string propertyName)
	{
		BoundObject = target;
		BoundProperty = propertyName;
		if (BoundObject.GetPropertyValue(propertyName) is var initialValue &&
			initialValue != null)
		{
			SetValueFromBinding(initialValue);
		}
		BoundObject.PropertyChanged += OnBoundObjectPropertyChanged;
	}

	protected virtual void OnBoundObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == BoundProperty &&
			BoundObject != null &&
			BoundProperty != null &&
			BoundObject.GetPropertyValue(BoundProperty) is object value)
		{
			SetValueFromBinding(value);
		}
	}

	protected void UpdateBoundProperty(object value)
	{
		if (BoundObject != null && BoundProperty != null)
		{
			BoundObject.SetPropertyValue(BoundProperty, value);
		}
	}

	protected abstract void SetValueFromBinding(object value);

	public override void Dispose()
	{
		BoundObject?.PropertyChanged -= OnBoundObjectPropertyChanged;
		base.Dispose();
	}

}