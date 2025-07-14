// BlazorWasm
using BlazorShared.Layout;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using Yaml.Localization;

foreach (var resource in typeof(BlazorShared.Lang).Assembly.GetManifestResourceNames())
{
	Console.WriteLine(resource);
}

var culture = CultureInfo.CurrentCulture;
while (culture.IsNeutralCulture)
{
	culture = CultureInfo.CreateSpecificCulture(culture.Name);
}
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

Console.WriteLine($" - Standalone WASM - Current culture: {CultureInfo.CurrentUICulture.Name} - {CultureInfo.CurrentUICulture.IsNeutralCulture}");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
	.AddScoped<IPlatformService, WebAssemblyPlatformService>()
	.AddScoped<ChangeThemeStore>()
	.AddScoped<CultureSelectorStore>()
	.AddYamlEmbeddedResourceLocalization(typeof(BlazorShared.Lang).Assembly);

builder.RootComponents.Add<BlazorWasm.Components.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
