using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;
using Yaml.Localization;

namespace Microsoft.Extensions.DependencyInjection;

/// <inheritdoc/>
public class WebAppWebAssemblyPlatformService(CultureSettings cultureSettings) : PlatformService
{
	/// <inheritdoc/>
	public override IComponentRenderMode RenderMode => AspNetCore.Components.Web.RenderMode.InteractiveWebAssembly;

	/// <inheritdoc/>
	public override Task<string> ChangeCultureAsync(CultureInfo culture, string currentUri, IJSRuntime? JS)
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
	public static async Task<WebAssemblyHost> SetWebAppWebAssemblyYamlLocalizationAsync(this WebAssemblyHost host)
	{
		ArgumentNullException.ThrowIfNull(host);
		var js = host.Services.GetRequiredService<IJSRuntime>();
		var cultureSettings = host.Services.GetRequiredService<CultureSettings>();

		var culture = await js.InvokeAsync<string?>($"BlazorHead.CultureSelector.getCookie");
		CultureInfo cultureInfo = culture != null ? culture.ToSpecificCulture() : CultureSettings.DefaultCultureName.ToSpecificCulture();
		CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
		CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

		return host;
	}
}
