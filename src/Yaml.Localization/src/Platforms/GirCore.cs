using Microsoft.JSInterop;
using System.Globalization;
using Yaml.Localization;

namespace Microsoft.Extensions.DependencyInjection;

/// <inheritdoc/>
public class GirCorePlatformService : PlatformService
{
	/// <inheritdoc/>
	public override async Task<string> ChangeCultureAsync(CultureInfo culture, string currentUri, IJSRuntime JS)
	{
		await JS.InvokeVoidAsync($"BlazorHead.CultureSelector.setStorage", culture.Name);
		return currentUri;
	}
}
