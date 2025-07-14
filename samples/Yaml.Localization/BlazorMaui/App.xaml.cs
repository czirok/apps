namespace BlazorMaui
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			var mainPage = Handler?.MauiContext?.Services?.GetService<MainPage>()!;

#if WINDOWS
            // Windows-specifikus TitleBar logika
            var mauiThemeService = Handler?.MauiContext?.Services?.GetService<MauiThemeService>();
            
            Color _lightColor = Colors.White;
            Color _darkColor = Colors.Black;
            
            if (Application.Current?.Resources != null)
            {
                if (Application.Current.Resources.TryGetValue("LightStatusBarColor", out var light) && light is Color lightColor)
                    _lightColor = lightColor;
                if (Application.Current.Resources.TryGetValue("DarkStatusBarColor", out var dark) && dark is Color darkColor)
                    _darkColor = darkColor;
            }
            
            var theme = Application.Current?.RequestedTheme ?? AppTheme.Light;
            var backgroundColor = theme == AppTheme.Dark ? _darkColor : _lightColor;
            
            var titleBar = new TitleBar
            {
                Title = "BlazorMaui",
                BackgroundColor = backgroundColor,
                ForegroundColor = theme == AppTheme.Dark ? Colors.White : Colors.Black,
            };
            
            var window = new Window(mainPage) { TitleBar = titleBar };
            
            window.HandlerChanged += (s, e) =>
            {
                if (s is not Window w || w.Handler?.PlatformView is not MauiWinUIWindow winUIWindow)
                    return;
                    
                var hwnd = winUIWindow.WindowHandle;
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd));
                
                mauiThemeService?.SetWindowReferences(titleBar, appWindow);
            };
            
            return window;
#else
			return new Window(mainPage) { Title = "BlazorMaui" };
#endif
		}
	}
}