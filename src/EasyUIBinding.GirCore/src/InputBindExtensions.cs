using System.ComponentModel;

namespace EasyUIBinding.GirCore;

public static partial class InputBindExtensions
{
	private static InputBinding<TRow, TValue> BindTo<TRow, TValue>(
		this InputBinding<TRow, TValue> input,
		INotifyPropertyChanged target,
		string propertyName)
		where TRow : Adw.PreferencesRow
	{
		input.Bind(target, propertyName);
		return input;
	}

	private static InputBindingDictionary<TRow, TValue> BindTo<TRow, TValue>(
		this InputBindingDictionary<TRow, TValue> input,
		INotifyPropertyChanged target,
		string propertyName)
		where TRow : Adw.PreferencesRow
	{
		input.Bind(target, propertyName);
		return input;
	}

	public static InputBindingTuple<TRow, TValue> Bind<TRow, TValue>(
		this InputBindingTuple<TRow, TValue> input,
		INotifyPropertyChanged target,
		string propertyName)
		where TRow : Adw.PreferencesRow
	{
		input.BindToTuple(target, propertyName);
		return input;
	}

	public static ClipboardButton BindTo(
		this ClipboardButton input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.ActionRow, string>)input).BindTo(target, propertyName);
		return input;
	}

	public static ColorSelector BindTo(
		this ColorSelector input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.ActionRow, Gdk.RGBA>)input).BindTo(target, propertyName);
		return input;
	}

	public static Combo<TValue> BindTo<TValue>(
		this Combo<TValue> input,
		INotifyPropertyChanged target,
		string propertyName)
		where TValue : notnull
	{
		((InputBindingDictionary<Adw.ComboRow, TValue>)input).BindTo(target, propertyName);
		return input;
	}

	public static ComboTuple<TValue> BindTo<TValue>(
		this ComboTuple<TValue> input,
		INotifyPropertyChanged target,
		string propertyName)
		where TValue : notnull
	{
		input.BindToTuple(target, propertyName);
		return input;
	}

	public static FontSelector BindTo(
		this FontSelector input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.ActionRow, string>)input).BindTo(target, propertyName);
		return input;
	}

	public static SaveAsSelector BindTo(
		this SaveAsSelector input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.ActionRow, string>)input).BindTo(target, propertyName);
		return input;
	}

	public static FileSelector BindTo(
		this FileSelector input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.ActionRow, string>)input).BindTo(target, propertyName);
		return input;
	}

	public static SpinDouble BindTo(
		this SpinDouble input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.SpinRow, double>)input).BindTo(target, propertyName);
		return input;
	}

	public static SpinFloat BindTo(
		this SpinFloat input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.SpinRow, float>)input).BindTo(target, propertyName);
		return input;
	}

	public static SpinInteger BindTo(
		this SpinInteger input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.SpinRow, int>)input).BindTo(target, propertyName);
		return input;
	}

	public static Text BindTo(
		this Text input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.EntryRow, string>)input).BindTo(target, propertyName);
		return input;
	}

	public static Password BindTo(
		this Password input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.PasswordEntryRow, string>)input).BindTo(target, propertyName);
		return input;
	}

	public static ScaleDouble BindTo(
		this ScaleDouble input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.ActionRow, double>)input).BindTo(target, propertyName);
		return input;
	}

	public static Switch BindTo(
		this Switch input,
		INotifyPropertyChanged target,
		string propertyName)
	{
		((InputBinding<Adw.SwitchRow, bool>)input).BindTo(target, propertyName);
		return input;
	}

	public static Toggle<TValue> BindTo<TValue>(
		this Toggle<TValue> input,
		INotifyPropertyChanged target,
		string propertyName)
		where TValue : notnull
	{
		((InputBindingDictionary<Adw.ActionRow, TValue>)input).BindTo(target, propertyName);
		return input;
	}

	public static WrapToggle<TValue> BindTo<TValue>(
		this WrapToggle<TValue> input,
		INotifyPropertyChanged target,
		string propertyName)
		where TValue : notnull
	{
		((InputBindingDictionary<Adw.ActionRow, TValue>)input).BindTo(target, propertyName);
		return input;
	}
}