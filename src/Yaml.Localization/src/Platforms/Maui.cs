using Microsoft.JSInterop;
using System.Globalization;
using Yaml.Localization;

namespace Microsoft.Extensions.DependencyInjection;

/// <inheritdoc/>
public abstract class MauiPlatformServiceBase : PlatformService
{
	/// <inheritdoc/>
	public override async Task<string> ChangeCultureAsync(CultureInfo culture, string currentUri, IJSRuntime JS)
	{
		await JS.InvokeVoidAsync($"BlazorHead.CultureSelector.setStorage", culture.Name);
		return currentUri;
	}
}

/// <inheritdoc/>
public class MauiPointerPlatformService : MauiPlatformServiceBase
{
	/// <inheritdoc/>
	public override bool IsTouch => false;
}

/// <inheritdoc/>
public class MauiTouchPlatformService : MauiPlatformServiceBase
{
	/// <inheritdoc/>
	public override bool IsTouch => true;
}
