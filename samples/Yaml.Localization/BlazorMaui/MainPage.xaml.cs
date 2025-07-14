namespace BlazorMaui;

public partial class MainPage : ContentPage
{
	public MainPage(MauiThemeService mauiThemeService)
	{
		InitializeComponent();
		blazorWebView.BindingContext = mauiThemeService;
		statusBarBehavior.BindingContext = mauiThemeService;
	}
}
