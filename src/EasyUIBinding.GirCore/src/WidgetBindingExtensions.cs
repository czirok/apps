using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyUIBinding.GirCore;

public static class WidgetBindingExtensions
{
	private static readonly ConditionalWeakTable<object, WidgetBinder> _binders = new();

	public static T BindTo<T>(this T widget, INotifyPropertyChanged target, string propertyName, string widgetPropertyName)
		where T : Gtk.Widget
	{
		if (!_binders.TryGetValue(widget, out var binder))
		{
			binder = new WidgetBinder(widget);
			_binders.Add(widget, binder);
		}

		binder.BindTo(target, propertyName, widgetPropertyName);
		return widget;
	}

	public static void ClearBindings<T>(this T widget) where T : Gtk.Widget
	{
		if (_binders.TryGetValue(widget, out var binder))
		{
			binder.Dispose();
			_binders.Remove(widget);
		}
	}
}