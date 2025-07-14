using System.ComponentModel;

namespace EasyUIBinding.GirCore.Binding;

public abstract class InputDictionary<TRow, TKey> : Input
	where TRow : Adw.PreferencesRow
{
	public abstract override TRow Row { get; }
	public abstract IDictionary<TKey, string> Values { get; }
	public abstract TKey? Default { get; }
	public abstract TKey? Selected { get; }
	public abstract void SetSelected(TKey key, string value);
}

public abstract class InputBindingDictionary<TRow, TKey> : InputDictionary<TRow, TKey>
	where TRow : Adw.PreferencesRow
{
	private readonly HashSet<NotifyPropertyModel> _boundTargets = [];
	protected readonly List<(NotifyPropertyModel target, string property)> BoundObjects = [];

	public void Bind(NotifyPropertyModel target, string propertyName)
	{
		BoundObjects.Add((target, propertyName));

		if (_boundTargets.Add(target))
		{
			target.PropertyChanged += OnBoundObjectPropertyChanged;
		}

		if (BoundObjects.Count == 1 && target[propertyName] is var initialValue)
			SetValueFromBinding(initialValue);
	}

	protected void OnBoundObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		foreach (var (target, property) in BoundObjects)
		{
			if (sender == target && e.PropertyName == property)
			{
				SetValueFromBinding(target[property]);
				break;
			}
		}
	}

	protected void UpdateAllBoundProperties(TKey key, string title)
	{
		if (BoundObjects.Count == 0) return;

		for (int i = 0; i < BoundObjects.Count; i++)
		{
			var (target, property) = BoundObjects[i];

			if (i == 0 && key is not null)
				target[property] = key;
			else if (i == 1 && title is not null)
				target[property] = title;
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

		base.Dispose();
	}
}
