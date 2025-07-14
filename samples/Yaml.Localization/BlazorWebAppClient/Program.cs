using BlazorShared.Layout;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Yaml.Localization;

foreach (var resource in typeof(BlazorShared.Lang).Assembly.GetManifestResourceNames())
{
	Console.WriteLine(resource);
}

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
	.AddScoped<IPlatformService, WebAppWebAssemblyPlatformService>()
	.AddScoped<ChangeThemeStore>()
	.AddScoped<CultureSelectorStore>()
	.AddYamlEmbeddedResourceLocalization(typeof(BlazorShared.Lang).Assembly);

var host = builder.Build();

await host.SetWebAppWebAssemblyYamlLocalizationAsync();

await host.RunAsync();
