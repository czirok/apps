#nullable disable

using Benchmark;
using Benchmark.Components;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Benchmark;

[ShortRunJob, MemoryDiagnoser, RankColumn]
public class Each : Core
{
	[Params(100000)]
	public int N;

	private IStringLocalizer<HelloWorld> _yamlLocalizer;
	private IStringLocalizer<HelloWorld> _resxLocalizer;

	[GlobalSetup]
	public void GlobalSetup()
	{
		Setup();

		// Pre-cache localizers
		_yamlLocalizer = ServicesYaml.GetService<IStringLocalizer<HelloWorld>>();
		_resxLocalizer = ServicesResx.GetService<IStringLocalizer<HelloWorld>>();
	}

	[Benchmark]
	public void YamlEach()
	{
		foreach (var testResource in TestResources)
		{
			testResource.Culture.SwitchToThisCulture();
			if (_yamlLocalizer[testResource.Name].Value.Equals(testResource.Value)) continue;
			Environment.Exit(0);
		}
	}

	[Benchmark]
	public void ResxEach()
	{
		foreach (var testResource in TestResources)
		{
			testResource.Culture.SwitchToThisCulture();
			if (_resxLocalizer[testResource.Name].Value.Equals(testResource.Value)) continue;
			Environment.Exit(0);
		}
	}

	[GlobalCleanup]
	public void GlobalCleanup()
	{
		Cleanup();
	}

}
