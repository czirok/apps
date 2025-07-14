using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using YamlLocalizationTests;
using YamlLocalizationTests.Components;

namespace YamlLocalizationSimulatedResourcePathTests;

public class YamlResourceStringLocalizerTest : IClassFixture<CoreFixture>
{
	private readonly ServiceProvider _services;

	public YamlResourceStringLocalizerTest(CoreFixture fixture)
	{
		_services = fixture.Services;
	}

	[Theory]
	[MemberData(nameof(Config.HelloWorld), MemberType = typeof(Config))]
	public void Compare(string culture, string name, string expected)
	{
		// Arrange
		culture.SwitchToThisCulture();

		// Act
		var localizer = _services.GetService<IStringLocalizer<HelloWorld>>();

		// Assert
		Assert.NotNull(localizer);
		Assert.Equal(expected, localizer[name]);
	}

	[Theory]
	[MemberData(nameof(Config.Answer), MemberType = typeof(Config))]
	public void CompareWithArguments(string culture, string name, string expected, int argument)
	{
		// Arrange
		culture.SwitchToThisCulture();

		// Act
		var stringLocalizer = _services.GetService<IStringLocalizer<Answer>>();

		// Assert
		Assert.NotNull(stringLocalizer);
		Assert.Equal(expected, stringLocalizer[name, argument]);
	}
}