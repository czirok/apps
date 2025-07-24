namespace EasyUIBinding.GirCore;

public static partial class InputBindExtensions
{
	private static T OnChanged<T>(this T input, Action action)
		where T : Input
	{
		input.SetCallback(action);
		return input;
	}

	private static T OnChanged<T>(this T input, Action<object, InputChangedEventArgs> action)
		where T : Input
	{
		input.SetValueCallback(action);
		return input;
	}

	public static Button OnClick(this Button input, Action action)
	{
		input.OnChanged(action);
		return input;
	}

	public static Button OnClick(this Button input, Action<object, InputChangedEventArgs> action)
	{
		input.OnChanged((obj, args) => action(obj, args));
		return input;
	}

	public static ClipboardButton OnClipboardButtonClicked(this ClipboardButton input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static ClipboardButton OnClipboardButtonClicked(this object input, Action<ClipboardButton, InputChangedEventArgs<string>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((ClipboardButton)sender, (InputChangedEventArgs<string>)args));
		return (ClipboardButton)input;
	}

	public static ColorSelector OnColorSelected(this ColorSelector input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static ColorSelector OnColorSelected(this object input, Action<ColorSelector, InputChangedEventArgs<Gdk.RGBA>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((ColorSelector)sender, (InputChangedEventArgs<Gdk.RGBA>)args));
		return (ColorSelector)input;
	}

	public static Combo<TValue> OnComboChanged<TValue>(this Combo<TValue> input, Action action)
		where TValue : notnull
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static Combo<TValue> OnComboChanged<TValue>(this object input, Action<Combo<TValue>, InputDictionaryChangedEventArgs<TValue>> action)
		where TValue : notnull
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((Combo<TValue>)sender, (InputDictionaryChangedEventArgs<TValue>)args));
		return (Combo<TValue>)input;
	}

	public static ComboTuple<TValue> OnComboTupleChanged<TValue>(this ComboTuple<TValue> input, Action action)
		where TValue : notnull
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static ComboTuple<TValue> OnComboTupleChanged<TValue>(this object input, Action<ComboTuple<TValue>, InputTupleChangedEventArgs<TValue>> action)
		where TValue : notnull
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((ComboTuple<TValue>)sender, (InputTupleChangedEventArgs<TValue>)args));
		return (ComboTuple<TValue>)input;
	}

	public static FileSelector OnFileSelected(this FileSelector input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static FileSelector OnFileSelected(this object input, Action<FileSelector, InputChangedEventArgs<string>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((FileSelector)sender, (InputChangedEventArgs<string>)args));
		return (FileSelector)input;
	}

	public static FontSelector OnFontSelected(this FontSelector input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static FontSelector OnFontSelected(this object input, Action<FontSelector, InputChangedEventArgs<string>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((FontSelector)sender, (InputChangedEventArgs<string>)args));
		return (FontSelector)input;
	}

	public static Password OnPasswordChanged(this Password input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static Password OnPasswordChanged(this object input, Action<Password, InputChangedEventArgs<string>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((Password)sender, (InputChangedEventArgs<string>)args));
		return (Password)input;
	}

	public static SaveAsSelector OnSaveAsSelected(this SaveAsSelector input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static SaveAsSelector OnSaveAsSelected(this object input, Action<SaveAsSelector, InputChangedEventArgs<string>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((SaveAsSelector)sender, (InputChangedEventArgs<string>)args));
		return (SaveAsSelector)input;
	}

	public static ScaleDouble OnScaleChanged(this ScaleDouble input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static ScaleDouble OnScaleChanged(this object input, Action<ScaleDouble, InputChangedEventArgs<double>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((ScaleDouble)sender, (InputChangedEventArgs<double>)args));
		return (ScaleDouble)input;
	}

	public static SpinDouble OnSpinDoubleChanged(this SpinDouble input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static SpinDouble OnSpinDoubleChanged(this object input, Action<SpinDouble, InputChangedEventArgs<double>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((SpinDouble)sender, (InputChangedEventArgs<double>)args));
		return (SpinDouble)input;
	}

	public static SpinFloat OnSpinFloatChanged(this SpinFloat input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static SpinFloat OnSpinFloatChanged(this object input, Action<SpinFloat, InputChangedEventArgs<float>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((SpinFloat)sender, (InputChangedEventArgs<float>)args));
		return (SpinFloat)input;
	}

	public static SpinInteger OnSpinIntegerChanged(this SpinInteger input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static SpinInteger OnSpinIntegerChanged(this object input, Action<SpinInteger, InputChangedEventArgs<int>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((SpinInteger)sender, (InputChangedEventArgs<int>)args));
		return (SpinInteger)input;
	}

	public static Switch OnSwitchToggled(this Switch input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static Switch OnSwitchToggled(this object input, Action<Switch, InputChangedEventArgs<bool>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((Switch)sender, (InputChangedEventArgs<bool>)args));
		return (Switch)input;
	}

	public static Text OnTextChanged(this Text input, Action action)
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static Text OnTextChanged(this object input, Action<Text, InputChangedEventArgs<string>> action)
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((Text)sender, (InputChangedEventArgs<string>)args));
		return (Text)input;
	}

	public static Toggle<TValue> OnToggleChanged<TValue>(this Toggle<TValue> input, Action action)
		where TValue : notnull
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static Toggle<TValue> OnToggleChanged<TValue>(this object input, Action<Toggle<TValue>, InputDictionaryChangedEventArgs<TValue>> action)
		where TValue : notnull
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((Toggle<TValue>)sender, (InputDictionaryChangedEventArgs<TValue>)args));
		return (Toggle<TValue>)input;
	}

	public static WrapToggle<TValue> OnWrapToggleChanged<TValue>(this WrapToggle<TValue> input, Action action)
		where TValue : notnull
	{
		((Input)input).OnChanged(action);
		return input;
	}

	public static WrapToggle<TValue> OnWrapToggleChanged<TValue>(this object input, Action<WrapToggle<TValue>, InputDictionaryChangedEventArgs<TValue>> action)
		where TValue : notnull
	{
		((Input)input).OnChanged((object sender, InputChangedEventArgs args) => action((WrapToggle<TValue>)sender, (InputDictionaryChangedEventArgs<TValue>)args));
		return (WrapToggle<TValue>)input;
	}
}