using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using Yaml.Localization;

namespace Microsoft.Extensions.DependencyInjection;

/// <inheritdoc/>
public class WebAssemblyPlatformService : PlatformService
{
	/// <inheritdoc/>
	public override IComponentRenderMode RenderMode => AspNetCore.Components.Web.RenderMode.InteractiveWebAssembly;

	/// <inheritdoc/>
	public override async Task<string> ChangeCultureAsync(CultureInfo culture, string currentUri, IJSRuntime JS)
	{
		await JS.InvokeVoidAsync($"BlazorHead.CultureSelector.setCookie", culture.Name);
		return currentUri;
	}
}
