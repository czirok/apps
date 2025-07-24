namespace EasyUIBinding.GirCore;

public class ThemeDetector : IDisposable
{
	private readonly Adw.StyleManager _styleManager;

	public event Action<bool>? ThemeChanged; // true = Dark, false = Light

	public ThemeDetector()
	{
		_styleManager = Adw.StyleManager.GetDefault();
		_styleManager.OnNotify += OnStyleManagerNotify;
	}

	private void OnStyleManagerNotify(GObject.Object sender, GObject.Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "dark")
		{
			bool isDark = _styleManager.Dark;
			ThemeChanged?.Invoke(isDark);
		}
	}

	public bool IsDarkTheme => _styleManager.Dark;

	public void Dispose()
	{
		_styleManager.OnNotify -= OnStyleManagerNotify;
	}
}