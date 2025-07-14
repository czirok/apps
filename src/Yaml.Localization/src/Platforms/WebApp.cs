using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.JSInterop;
using System.Globalization;
using Yaml.Localization;

namespace Microsoft.Extensions.DependencyInjection;

/// <inheritdoc/>
public class WebAppPlatformService(CultureSettings cultureSettings) : PlatformService
{
	/// <inheritdoc/>
	public override IComponentRenderMode RenderMode => AspNetCore.Components.Web.RenderMode.InteractiveServer;

	/// <inheritdoc/>
	public override Task<string> ChangeCultureAsync(CultureInfo culture, string currentUri, IJSRuntime JS)
	{
		var uri = new Uri(currentUri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
		var cultureEscaped = Uri.EscapeDataString(culture.Name);
		var uriEscaped = Uri.EscapeDataString(uri);
		var newUrl = $"{cultureSettings.RedirectEndpoint}?culture={cultureEscaped}&redirectUri={uriEscaped}";
		return Task.FromResult(newUrl);
	}
}

/// <summary>
/// Extension methods for setting up localization services in an <see cref="IServiceCollection" />.
/// </summary>
public static partial class YamlLocalizerExtensions
{
	public static WebApplication SetWebAppYamlLocalization(this WebApplication app)
	{
		ArgumentNullException.ThrowIfNull(app);

		app.UseRequestLocalization(options =>
		{
			var cultureSettings = app.Services.GetRequiredService<CultureSettings>();
			options.DefaultRequestCulture = new RequestCulture(
				culture: CultureSettings.DefaultCultureName.ToSpecificCulture(),
				uiCulture: CultureSettings.DefaultCultureName.ToSpecificCulture());

			options.SupportedCultures = [.. cultureSettings.Cultures.AllSpecificSupportedCultureInfos()];
			options.SupportedUICultures = [.. cultureSettings.Cultures.AllSpecificSupportedCultureInfos()];
			// Change cookie name
			options.RequestCultureProviders
				.OfType<CookieRequestCultureProvider>().First()
				.CookieName = cultureSettings.CookieName;
		});

		var cultureSettings = app.Services.GetRequiredService<CultureSettings>();

		app.MapGet($"/{cultureSettings.RedirectEndpoint}", (HttpContext http, string culture, string redirectUri, CultureSettings settings) =>
		{
			if (!string.IsNullOrWhiteSpace(culture))
			{
				http.Response.Cookies.Append(
					settings.CookieName,
					CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture))
				);
			}

			return Results.Redirect(redirectUri);
		});
		return app;
	}
}
