namespace EasyUIBinding.GirCore.Binding;

public abstract class InputSelectableList<TRow, TValue> : Input
	where TRow : Adw.PreferencesRow
{
	public abstract override TRow Row { get; }
	public abstract IList<TValue> Values { get; }
	public abstract TValue? Default { get; }
	public abstract TValue? Selected { get; }

	public abstract void SetSelected(TValue value);
}
