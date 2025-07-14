namespace EasyUIBinding.GirCore.Binding;

public class InputChangedEventArgs(string name, object? value) : EventArgs
{
	public string Name { get; } = name;
	internal object? ObjectValue { get; } = value;
}

public class InputChangedEventArgs<TValue>(string name, TValue? value)
	: InputChangedEventArgs(name, value)
{
	public TValue? Value => ObjectValue is TValue val ? val : default!;
}

public class InputDictionaryChangedEventArgs<TKey>(string name, TKey key, string title)
	: InputChangedEventArgs(name, title)
{
	public TKey Key { get; } = key;
	public string Title => (string)ObjectValue!;
}