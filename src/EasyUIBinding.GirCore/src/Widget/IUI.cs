namespace EasyUIBinding.GirCore.Widget;

public abstract class UI : IDisposable
{
	public abstract Gtk.Widget Widget { get; }
	public abstract string Name { get; }

	public virtual void Dispose()
	{
		Widget.Dispose();
	}
}

public abstract class UI<TWidget, TValue> : UI
	where TWidget : Gtk.Widget
{
	public abstract override TWidget Widget { get; }
	public abstract TValue Value { get; }
	public abstract void SetValue(TValue value);
}