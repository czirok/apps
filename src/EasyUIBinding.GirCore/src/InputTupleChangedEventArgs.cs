namespace EasyUIBinding.GirCore;

// Extended event args for tuple-based combos
public class InputTupleChangedEventArgs<TValue>(string name, TValue value, string display)
    : InputChangedEventArgs(name, value)
{
    public TValue Value => (TValue)ObjectValue!;
    public string Display { get; } = display;
}
