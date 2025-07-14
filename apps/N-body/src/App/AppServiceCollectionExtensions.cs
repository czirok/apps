using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;

namespace NBody.App;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public static class AppServiceCollectionExtensions
{
	public static IServiceCollection AddNBodyApp(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, "Service collection is required.");

		services.AddSingleton(services);

		var config = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.Build();
		ArgumentNullException.ThrowIfNull(config, "Configuration is required.");

		services.AddSingleton<IConfiguration>(config);

		services.AddLogging(config =>
		{
			config.AddSimpleConsole(options =>
			{
				options.SingleLine = true;
				options.TimestampFormat = "HH:mm:ss ";
			});
		});

		services.AddYamlFileSystemLocalization();

		services.AddSingleton<Settings>();
		services.AddSingleton<SettingsWindow>();

		services.AddTransient<About>();

		services.AddSingleton<Canvas>();
		services.AddSingleton<CanvasWindow>();

		services.AddSingleton<Manager>();

		return services;
	}
}