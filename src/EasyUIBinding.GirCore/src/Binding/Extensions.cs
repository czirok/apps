
namespace EasyUIBinding.GirCore.Binding;

public static partial class Extensions
{
	public static InputBinding<TRow, TValue> BindTo<TRow, TValue>(
		this InputBinding<TRow, TValue> input,
		NotifyPropertyModel target,
		string propertyName)
		where TRow : Adw.PreferencesRow
	{
		input.Bind(target, propertyName);
		return input;
	}

	public static InputBindingDictionary<TRow, TValue> BindTo<TRow, TValue>(
		this InputBindingDictionary<TRow, TValue> input,
		NotifyPropertyModel target,
		string propertyName)
		where TRow : Adw.PreferencesRow
	{
		input.Bind(target, propertyName);
		return input;
	}

	public static T OnChanged<T>(this T input, Action action)
		where T : Input
	{
		input.SetCallback(action);
		return input;
	}

	public static T OnChanged<T>(this T input, Action<object, InputChangedEventArgs> action)
		where T : Input
	{
		input.SetValueCallback(action);
		return input;
	}

	public static T OnClick<T>(this T input, Action action)
		where T : Input => input.OnChanged(action);

	public static T OnClick<T>(this T input, Action<object, InputChangedEventArgs> action)
		where T : Input => input.OnChanged((obj, args) => action(obj, args));
}
