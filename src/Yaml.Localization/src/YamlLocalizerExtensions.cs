using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Yaml.Localization;
using YamlDotNet.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up localization services in an <see cref="IServiceCollection" />.
/// </summary>
public static partial class YamlLocalizerExtensions
{
	static ILogger<YamlLocalizer> _logger = default!;

	/// <summary>
	/// Adds services required for application localization.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="assembly">The shared project <see cref="Assembly"/>.</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
	public static IServiceCollection AddYamlEmbeddedResourceLocalization(this IServiceCollection services, Assembly assembly)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(assembly);

		var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
		_logger = loggerFactory.CreateLogger<YamlEmbeddedResourceLocalizer>();

		services.AddOptions();

		services.TryAddSingleton<IStringLocalizerFactory, YamlEmbeddedResourceLocalizerFactory>();
		services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

		var resourceFile = nameof(CultureSettings).YamlExt();
		try
		{
			using var resourceStream = assembly.GetManifestResourceStream(resourceFile);
			using var reader = new StreamReader(resourceStream!);
			services.TryAddSingleton(
				new StaticDeserializerBuilder(
					new StaticAoTContext()).Build().Deserialize<CultureSettings>(reader));
		}
		catch (ArgumentNullException)
		{
			Log.ResourceNotExist(_logger, resourceFile);
			throw;
		}
		catch
		{
			Log.InvalidYamlFormat(_logger, resourceFile);
			throw;
		}

		return services;
	}

	/// <summary>
	/// Adds services required for application localization.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
	public static IServiceCollection AddYamlFileSystemLocalization(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services);

		var provider = services.BuildServiceProvider();

		var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
		_logger = loggerFactory.CreateLogger<YamlFileSystemLocalizer>();

		services.AddOptions();

		services.TryAddSingleton<IStringLocalizerFactory, YamlFileSystemLocalizerFactory>();
		services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

		var resourceFile = nameof(CultureSettings).YamlExt();

		var configuration = provider.GetRequiredService<IConfiguration>();

		try
		{
			string i18nPath = configuration[nameof(i18nPath)]!;
			var fileName = Path.Combine(AppContext.BaseDirectory, i18nPath, resourceFile);
			using var resourceStream = File.OpenRead(fileName)
				?? throw new FileNotFoundException($"File '{fileName}' not found.");

			using var reader = new StreamReader(resourceStream!);
			services.TryAddSingleton(
				new StaticDeserializerBuilder(
					new StaticAoTContext()).Build().Deserialize<CultureSettings>(reader));

			var yamlPath = Path.Combine(AppContext.BaseDirectory, i18nPath);
			services.TryAddSingleton(Directory.GetFiles(yamlPath, "*".YamlExt(), SearchOption.AllDirectories));

		}
		catch (FileNotFoundException)
		{
			Log.ResourceNotExist(_logger, resourceFile);
			throw;
		}
		catch
		{
			Log.InvalidYamlFormat(_logger, resourceFile);
			throw;
		}

		return services;
	}

}

public static partial class YamlLocalizerExtensions
{
	static partial class Log
	{
		[LoggerMessage(1, LogLevel.Warning, "Culture settings resource '{Name}' not exist.", EventName = "CultureSettingsNotExist")]
		public static partial void ResourceNotExist(ILogger logger, string name);

		[LoggerMessage(2, LogLevel.Critical, "Invalid culture settings yaml format: '{ResourceName}'.", EventName = "InvalidCultureSettingsYamlFormat")]
		public static partial void InvalidYamlFormat(ILogger logger, string resourceName);
	}
}