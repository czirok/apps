#nullable disable

using Benchmark;
using Benchmark.Components;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Benchmark;

[ShortRunJob, MemoryDiagnoser, RankColumn]
public class Concurrently : Core
{

	private IStringLocalizer<HelloWorld> _yamlLocalizer;
	private IStringLocalizer<HelloWorld> _resxLocalizer;
	const int NumberOfThreads = 10;

	[Params(100000)]
	public int N;

	[GlobalSetup]
	public void GlobalSetup()
	{
		Setup();

		// Pre-cache localizers
		_yamlLocalizer = ServicesYaml.GetService<IStringLocalizer<HelloWorld>>();
		_resxLocalizer = ServicesResx.GetService<IStringLocalizer<HelloWorld>>();
	}

	[Benchmark]
	public void YamlConcurrently()
	{
		Start(NumberOfThreads, _yamlLocalizer);
	}

	[Benchmark]
	public void ResxConcurrently()
	{
		Start(NumberOfThreads, _resxLocalizer);
	}

	[GlobalCleanup]
	public void GlobalCleanup()
	{
		Cleanup();
	}

	void Start(int numberOfThreads, IStringLocalizer<HelloWorld> localizer)
	{
		var actions = TestResources.Select(testResource => (Action<ManualResetEvent>)(manualResetEvent =>
		{
			testResource.Culture.SwitchToThisCulture();
			if (localizer[testResource.Name].Value.Equals(testResource.Value)) return;

			manualResetEvent.Reset();
			Environment.Exit(0);
		})).ToList();

		var manualResetEvent = new ManualResetEvent(false);

		var tasks = new Task[numberOfThreads * actions.Count];
		var count = 0;
		foreach (var action in actions)
		{
			for (var i = 0; i < numberOfThreads; i++)
			{
				tasks[count] = Task.Run(() =>
				{
					manualResetEvent.WaitOne();
					action(manualResetEvent);
				});
				count++;
			}
		}

		manualResetEvent.Set();
		Task.WhenAll(tasks);
	}
}
