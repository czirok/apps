using BlazorShared;
using CommunityToolkit.Maui.Core;
using Yaml.Localization;

namespace BlazorMaui;

public class MauiThemeService : NotifyPropertyChanged
{
	private readonly IPlatformService _sharedThemeService = default!;
	private Color _statusBarColor = Colors.Black;
	private StatusBarStyle _statusBarStyle = StatusBarStyle.LightContent;
	private Thickness _blazorOffset = new(0);

	private readonly Color _lightColor = Colors.White;
	private readonly Color _darkColor = Colors.Black;

	public MauiThemeService(IPlatformService sharedThemeService)
	{
		ArgumentNullException.ThrowIfNull(sharedThemeService);
		ArgumentNullException.ThrowIfNull(Application.Current);
		ArgumentNullException.ThrowIfNull(Application.Current.Resources);

		if (Application.Current.Resources.TryGetValue("LightStatusBarColor", out var light) && light is Color lightColor)
			_lightColor = lightColor;
		if (Application.Current.Resources.TryGetValue("DarkStatusBarColor", out var dark) && dark is Color darkColor)
			_darkColor = darkColor;

#if IOS
    	SetIOSBlazorOffset();
#elif ANDROID
		SetAndroidBlazorOffset();
#endif

		SetSystemTheme();

		_sharedThemeService = sharedThemeService;
		_sharedThemeService.ThemeChanged += OnThemeChanged;
	}

#if IOS
private void SetIOSBlazorOffset()
{
    double safeAreaTop = 0;

    if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
    {
        // iOS 15+ modern API
#pragma warning disable CA1416
        var windowScene = UIKit.UIApplication.SharedApplication.ConnectedScenes
            .OfType<UIKit.UIWindowScene>()
            .FirstOrDefault(scene => scene.ActivationState == UIKit.UISceneActivationState.ForegroundActive);
        
        var window = 
			windowScene?.Windows?.FirstOrDefault(w => w.IsKeyWindow) 
			??
			windowScene?.Windows?.FirstOrDefault();
        
        safeAreaTop = window?.SafeAreaInsets.Top ?? 0;
#pragma warning restore CA1416
    }
    else
    {
        // iOS 13-14 fallback
#pragma warning disable CA1422
        var window =
			UIKit.UIApplication.SharedApplication.KeyWindow 
			??
			UIKit.UIApplication.SharedApplication.Windows.FirstOrDefault();
        safeAreaTop = window?.SafeAreaInsets.Top ?? 0;
#pragma warning restore CA1422
    }

    BlazorOffset = new Thickness(0, safeAreaTop, 0, 0);
}
#endif

#if ANDROID
	private void SetAndroidBlazorOffset()
	{
		// 		var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity as AndroidX.AppCompat.App.AppCompatActivity;
		// 		if (activity?.Window?.DecorView?.RootWindowInsets != null)
		// 		{
		// 			var insets = activity.Window.DecorView.RootWindowInsets;
		// 			var density = Android.Content.Res.Resources.System?.DisplayMetrics?.Density ?? 1.0f;

		// 			if (density <= 0) density = 1.0f; // Null/zero védelem

		// 			// Modern API (Android 11+)
		// 			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R)
		// 			{
		// #pragma warning disable CA1416
		// 				var systemBarsInsets = insets.GetInsets(AndroidX.Core.View.WindowInsetsCompat.Type.SystemBars());
		// 				var topInset = systemBarsInsets.Top / density;
		// #pragma warning restore CA1416
		// 				BlazorOffset = new Thickness(0, topInset, 0, 0);
		// 			}
		// 			else
		// 			{
		// #pragma warning disable CA1422
		// 				var topInset = insets.SystemWindowInsetTop / density;
		// #pragma warning restore CA1422
		// 				BlazorOffset = new Thickness(0, topInset, 0, 0);
		// 			}
		// 		}
	}
#endif

	private void SetSystemTheme()
	{
		switch (Application.Current?.RequestedTheme)
		{
			case AppTheme.Light:
				StatusBarColor = _lightColor;
				StatusBarStyle = StatusBarStyle.DarkContent;
#if WINDOWS
				UpdateWindowsTitle(false);
#endif
				break;
			case AppTheme.Dark:
				StatusBarColor = _darkColor;
				StatusBarStyle = StatusBarStyle.LightContent;
#if WINDOWS
				UpdateWindowsTitle(true);
#endif
				break;
			case AppTheme.Unspecified:
				StatusBarColor = _lightColor;
				StatusBarStyle = StatusBarStyle.DarkContent;
#if WINDOWS
				UpdateWindowsTitle(false);
#endif
				break;
		}
	}

	public Color StatusBarColor
	{
		get => _statusBarColor;
		private set
		{
			if (_statusBarColor != value)
			{
				_statusBarColor = value;
				OnPropertyChanged();
			}
		}
	}

	public StatusBarStyle StatusBarStyle
	{
		get => _statusBarStyle;
		private set
		{
			if (_statusBarStyle != value)
			{
				_statusBarStyle = value;
				OnPropertyChanged();
			}
		}
	}

	public Thickness BlazorOffset
	{
		get => _blazorOffset;
		private set
		{
			if (_blazorOffset != value)
			{
				_blazorOffset = value;
				OnPropertyChanged();
			}
		}
	}

	private void OnThemeChanged(object? sender, ThemeChangedEventArgs e)
	{
#if IOS
    	SetIOSBlazorOffset();
#elif ANDROID
		SetAndroidBlazorOffset();
#endif
		UpdateStatusBar(e.Theme);
	}

#if WINDOWS
    private TitleBar? _titleBar;
    private Microsoft.UI.Windowing.AppWindow? _appWindow;
    
    public void SetWindowReferences(TitleBar? titleBar, Microsoft.UI.Windowing.AppWindow? appWindow)
    {
        _titleBar = titleBar;
        _appWindow = appWindow;
    }
#endif

	private void UpdateStatusBar(Theme theme)
	{
		switch (theme)
		{
			case Theme.Light:
				StatusBarColor = _lightColor;
				StatusBarStyle = StatusBarStyle.DarkContent;
#if WINDOWS
                UpdateWindowsTitle(false);
#endif
				break;
			case Theme.Dark:
				StatusBarColor = _darkColor;
				StatusBarStyle = StatusBarStyle.LightContent;
#if WINDOWS
                UpdateWindowsTitle(true);
#endif
				break;
			case Theme.Auto:
				SetSystemTheme();
				break;
		}
	}

#if WINDOWS
    private void UpdateWindowsTitle(bool isDark)
    {
        if (_titleBar != null)
        {
            _titleBar.BackgroundColor = isDark ? _darkColor : _lightColor;
            _titleBar.ForegroundColor = isDark ? Colors.White : Colors.Black;
        }

		if (_appWindow?.TitleBar != null)
		{
			// Az App.xaml-ből jövő színek használata
			var buttonBg = isDark ? _darkColor.ToWindowsColor() : _lightColor.ToWindowsColor();
			var buttonFg = isDark ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.Black;

			_appWindow.TitleBar.ButtonBackgroundColor = buttonBg;
			_appWindow.TitleBar.ButtonForegroundColor = buttonFg;

			// Hover és Pressed színek is az alapszínekből származzanak
			var hoverBg = isDark ? _darkColor.LightenColor().ToWindowsColor() : _lightColor.DarkenColor().ToWindowsColor();
			var pressedBg = isDark ? _darkColor.DarkenColor().ToWindowsColor() : _lightColor.LightenColor().ToWindowsColor();

			_appWindow.TitleBar.ButtonHoverBackgroundColor = hoverBg;
			_appWindow.TitleBar.ButtonHoverForegroundColor = buttonFg;
			_appWindow.TitleBar.ButtonPressedBackgroundColor = pressedBg;
			_appWindow.TitleBar.ButtonPressedForegroundColor = buttonFg;
		}
	}
#endif
}


#if WINDOWS
public static class WindowsExtensions
{
	public static Windows.UI.Color ToWindowsColor(this Color mauiColor)
	{
		return Microsoft.UI.ColorHelper.FromArgb(
			(byte)(mauiColor.Alpha * 255),
			(byte)(mauiColor.Red * 255),
			(byte)(mauiColor.Green * 255),
			(byte)(mauiColor.Blue * 255));
	}


	public static Color LightenColor(this Color color)
	{
		float factor = 0.2f; // 20%-kal világosabb
		return Color.FromRgba(
			Math.Min(1.0f, color.Red + factor),
			Math.Min(1.0f, color.Green + factor),
			Math.Min(1.0f, color.Blue + factor),
			color.Alpha);
	}

	public static Color DarkenColor(this Color color)
	{
		float factor = 0.2f; // 20%-kal sötétebb
		return Color.FromRgba(
			Math.Max(0.0f, color.Red - factor),
			Math.Max(0.0f, color.Green - factor),
			Math.Max(0.0f, color.Blue - factor),
			color.Alpha);
	}
}
#endif