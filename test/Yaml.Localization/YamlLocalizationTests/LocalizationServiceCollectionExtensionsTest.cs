using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Yaml.Localization;

namespace YamlLocalizationTests;

public class LocalizationServiceCollectionExtensionsTest
{
	[Fact]
	public void AddYamlResourceLocalization_Services()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
		services.AddYamlEmbeddedResourceLocalization(GetType().Assembly);

		// Assert
		AssertEqual(
			services,
			typeof(IStringLocalizerFactory),
			typeof(YamlEmbeddedResourceLocalizerFactory),
			ServiceLifetime.Singleton);

		AssertEqual(
			services,
			typeof(IStringLocalizer<>),
			typeof(StringLocalizer<>),
			ServiceLifetime.Transient);
	}

	static void AssertEqual(
		IServiceCollection services,
		Type serviceType,
		Type implementationType,
		ServiceLifetime serviceLifetime
		)
	{
		var descriptor = services.Single(serviceDescriptor => serviceDescriptor.ServiceType == serviceType);
		Assert.Equal(serviceType, descriptor.ServiceType);
		Assert.Equal(implementationType, descriptor.ImplementationType);
		Assert.Equal(serviceLifetime, descriptor.Lifetime);
	}
}
