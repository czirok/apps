#nullable disable

using Benchmark;
using Benchmark.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;
using System.Resources;
using Yaml.Localization;

namespace Benchmark;

public class Core
{
	public class TestResource
	{
		public required string Culture { get; set; }
		public required string Name { get; set; }
		public required string Value { get; set; }
	}

	protected ServiceProvider ServicesResx;
	protected ServiceProvider ServicesYaml;

	protected IEnumerable<TestResource> TestResources => Config.HelloWorld().Select(value =>
		new TestResource
		{
			Culture = (string)value[0],
			Name = (string)value[1],
			Value = (string)value[2]
		}).ToList();

	protected void Setup()
	{
		var serviceCollectionResx = new ServiceCollection();
		serviceCollectionResx.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
		serviceCollectionResx.AddLocalization();
		ServicesResx = serviceCollectionResx.BuildServiceProvider();

		var serviceCollectionYaml = new ServiceCollection();
		serviceCollectionYaml.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
		serviceCollectionYaml.AddYamlEmbeddedResourceLocalization(typeof(Core).Assembly);
		ServicesYaml = serviceCollectionYaml.BuildServiceProvider();
	}

	protected void Cleanup()
	{
		ServicesResx?.Dispose();
		ServicesYaml?.Dispose();
	}
}