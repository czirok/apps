using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace Yaml.Localization;

/// <summary>
/// System theme enumeration. 
/// </summary>
public enum Theme
{
	Light,
	Dark,
	Auto
}

/// <summary>
/// Event arguments for theme change events.
/// </summary>
public class ThemeChangedEventArgs : EventArgs
{
	/// <summary>
	/// Gets or sets the new theme.
	/// </summary>
	public Theme Theme { get; set; } = Theme.Auto;
}

/// <summary>
/// Interface for platform-specific services.
/// </summary>
public interface IPlatformService
{
	/// <summary>
	/// Gets the render mode for platform-specific components.
	/// </summary>
	IComponentRenderMode RenderMode { get; }

	/// <summary>
	/// Returns a URL that is set with the specified culture and can be used with NavigationManager.NavigateTo(url, forceLoad: true).
	/// </summary>
	/// <param name="culture">The culture to set in the URL.</param>
	/// <param name="currentUri">The current URI to modify. This is always NavigationManager.Uri.</param>
	/// <param name="JS">The JavaScript runtime.</param>
	Task<string> ChangeCultureAsync(CultureInfo culture, string currentUri, IJSRuntime JS);

	/// <summary>
	/// Gets whether the application is running on touch handheld devices.
	/// </summary>
	bool IsTouch { get; }

	/// <summary>
	/// Event that is raised when the theme changes.
	/// </summary>
	event EventHandler<ThemeChangedEventArgs> ThemeChanged;

	/// <summary>
	/// Sets the current theme of the application.
	/// </summary>
	/// <param name="theme">The theme to set.</param>
	void SetTheme(Theme theme);
}

/// <inheritdoc/>
public abstract class PlatformService : IPlatformService
{
	/// <inheritdoc/>
	public virtual IComponentRenderMode RenderMode => default!;

	/// <inheritdoc/>
	public event EventHandler<ThemeChangedEventArgs> ThemeChanged = default!;

	/// <inheritdoc/>
	public abstract Task<string> ChangeCultureAsync(CultureInfo culture, string currentUri, IJSRuntime JS);

	/// <inheritdoc/>
	public virtual bool IsTouch => false;

	/// <inheritdoc/>
	public void SetTheme(Theme theme)
	{
		ThemeChanged?.Invoke(this, new ThemeChangedEventArgs
		{
			Theme = theme
		});
	}
}