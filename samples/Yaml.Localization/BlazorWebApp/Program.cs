using BlazorShared.Layout;
using Yaml.Localization;

foreach (var resource in typeof(BlazorShared.Lang).Assembly.GetManifestResourceNames())
{
	Console.WriteLine(resource);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services
	.AddScoped<IPlatformService, WebAppPlatformService>()
	.AddScoped<ChangeThemeStore>()
	.AddScoped<CultureSelectorStore>()
	.AddYamlEmbeddedResourceLocalization(typeof(BlazorShared.Lang).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.SetWebAppYamlLocalization();

app.MapRazorComponents<BlazorWebApp.Components.App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(BlazorShared.Lang).Assembly);

app.Run();
