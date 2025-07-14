namespace EasyUIBinding.GirCore.Binding;

public abstract class InputList<TRow, TValue> : Input
	where TRow : Adw.PreferencesRow
{
	public abstract override TRow Row { get; }
	public abstract IList<TValue> Values { get; }
}