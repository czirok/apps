using EasyUIBinding.GirCore;
using EasyUIBinding.GirCore.Binding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Yaml.Localization;

namespace NBody.App;

public class Settings : Adw.PreferencesPage, IDisposable
{
	internal readonly List<Input> Systems;
	internal readonly List<Input> Parameters;
	internal readonly List<Input> Functions;
	internal readonly List<Input> Languages;
	internal Dictionary<string, Adw.PreferencesGroup> Groups;

	private readonly CultureSettings _cultures;
	private readonly IStringLocalizer<Settings> L;
	private readonly ILogger<Settings> _logger;

	public event Action? LanguageChanged;

	public Settings(CultureSettings cultureSettings, IStringLocalizer<Settings> localizer, ILogger<Settings> logger)
	{
		ArgumentNullException.ThrowIfNull(cultureSettings);
		ArgumentNullException.ThrowIfNull(localizer);
		ArgumentNullException.ThrowIfNull(logger);

		_cultures = cultureSettings;
		L = localizer;

		_logger = logger;

		Groups = [];
		Systems = [];
		Parameters = [];
		Functions = [];
		Languages = [];

		InitializeUI();
	}

	private void InitializeUI()
	{
		foreach (var group in Groups.Values)
		{
			Remove(group);
		}
		Groups.Clear();
		ClearInputs();

		CreateGroups();
		CreateInputs();
		AddInputsToGroups();
		AddGroupsToPage();
	}

	private void CreateGroups()
	{
		Groups = new()
		{
			["systems"] = UI.Group(L["Gravitational systems"]),
			["parameters"] = UI.Group(L["Simulation constants"]),
			["functions"] = UI.Group(L["Controls and display options"]),
			["languages"] = UI.Group(L["Languages"])
		};
	}

	private void CreateInputs()
	{
		Systems.AddRange([
			new WrapToggle<SystemType>("systems", new Dictionary<SystemType, string>
			{
				[SystemType.None] = L["None"],
				[SystemType.DistributionTest] = L["Distribution Test"],
				[SystemType.SlowParticles] = L["Slow Particles"],
				[SystemType.FastParticles] = L["Fast Particles"],
				[SystemType.MassiveBody] = L["Massive Body"],
				[SystemType.OrbitalSystem] = L["Orbital System"],
				[SystemType.BinarySystem] = L["Binary System"],
				[SystemType.PlanetarySystem] = L["Planetary System"],
				[SystemType.PlanetarySystemColor] = L["Colored Planetary System"],
				[SystemType.ThreeBody] = L["Three Body"],
				[SystemType.CollidingSystems] = L["Colliding Systems"],
				[SystemType.QuantumAurora] = L["Quantum Aurora"],
				[SystemType.GalacticDance] = L["Galactic Dance"],
				[SystemType.GalacticSpiralChaos] = L["Galactic Spiral Chaos"],
				[SystemType.SpiralGalaxy] = L["Spiral Galaxy"],
				[SystemType.FractalChaosAttractor] = L["Fractal Chaos Attractor"],
				[SystemType.AuroraHarmonia] = L["Aurora Harmonia"],
				[SystemType.CosmicBallet] = L["Cosmic Ballet"],
				[SystemType.SupernovaRemnants] = L["Supernova Remnants"]
			},
			SystemType.None)
				.OnChanged(() => {
					_logger.LogInformation("System type changed to {SystemType} with title {SystemTitle}",
						World.Instance.SystemType, World.Instance.SystemTitle);
					World.Instance.Generate();
				})
				.BindTo(World.Instance, nameof(World.Instance.SystemType))
				.BindTo(World.Instance, nameof(World.Instance.SystemTitle))

		]);

		if (Systems.FirstOrDefault(input => input.Name == "systems") is WrapToggle<SystemType> system)
		{
			World.Instance.SystemTitle = system.Values[World.Instance.SystemType];
		}

		Parameters.AddRange([
			new SpinInteger("G", L["Gravitational Constant G"], (int)World.Instance.G, new IntRange(1, 1000))
				.OnChanged(() => {
					_logger.LogInformation("Gravitational constant G changed to {G}", World.Instance.G);
				})
				.BindTo(World.Instance, nameof(World.G)),
			new SpinInteger("C", L["Speed of Light C"], (int)World.Instance.C, new IntRange(10, 1_000_000))
				.OnChanged(() => {
					_logger.LogInformation("Speed of light C changed to {C}", World.Instance.C);
				})
				.BindTo(World.Instance, nameof(World.C)),
			new SpinInteger("N", L["Number of Bodies N"], World.Instance.BodyAllocationCount, new IntRange(0, 10000))
				.OnChanged(() => {
					_logger.LogInformation("Body allocation count changed to {Count}", World.Instance.BodyAllocationCount);
					World.Instance.Generate();
				})
				.BindTo(World.Instance, nameof(World.Instance.BodyAllocationCount))
		]);

		Functions.AddRange([
			new Switch("stop-start", L["Stop / Start"])
				.OnChanged(() => {
					_logger.LogInformation("World active state changed to {Active}", World.Instance.Active);
				})
				.BindTo(World.Instance, nameof(World.Active)),
			new Switch("tree", L["Hide / Show Tree"])
				.OnChanged(() => {
					_logger.LogInformation("Draw tree state changed to {DrawTree}", World.Instance.DrawTree);
				})
				.BindTo(World.Instance, nameof(World.DrawTree)),
			new Switch("traces", L["Hide / Show Traces"])
				.OnChanged(() => {
					_logger.LogInformation("Draw traces state changed to {DrawTracers}", World.Instance.DrawTracers);
				})
				.BindTo(World.Instance, nameof(World.DrawTracers)),
			new Button("reset-camera", new ButtonLabel(L["Reset Camera"]))
				.OnClick(() => {
					World.Instance.ResetCamera();
					_logger.LogInformation("Camera reset to default position and orientation.");
				}),
			new Switch("stats", L["Hide / Show Stats"])
				.OnChanged(() => {
					_logger.LogInformation("Show stats state changed to {ShowStats}", World.Instance.ShowStats);
				})
				.BindTo(World.Instance, nameof(World.ShowStats))
		]);

		Languages.AddRange([
			new WrapToggle<string>(
				"languages",
				_cultures.SpecificActiveSelector(),
				_cultures.SpecificDefaultCultureInfo().Name,
				CultureInfo.CurrentUICulture.Name)
				.OnChanged((sender, args) => {
					_logger.LogInformation($"Args type: {args.GetType().Name}");
					if (args is not InputDictionaryChangedEventArgs<string> inputArgs)
						return;
					_logger.LogInformation("Language changed to {Language} with title {Title}", inputArgs.Key, inputArgs.Title);
					if (inputArgs.Title == null || !_cultures.SpecificActiveCultures().Contains(inputArgs.Key))
						return;
					var culture = inputArgs.Key.ToSpecificCulture();
					if (CultureInfo.CurrentUICulture.Name == culture.Name)
						return;
					CultureInfo.CurrentUICulture = culture;
					CultureInfo.CurrentCulture = culture;
					RefreshUI();
					LanguageChanged?.Invoke();
				})
		]);
	}

	private void AddInputsToGroups()
	{
		foreach (var input in Systems)
			Groups["systems"].Add(input.Row);

		foreach (var input in Parameters)
			Groups["parameters"].Add(input.Row);

		foreach (var input in Functions)
			Groups["functions"].Add(input.Row);

		foreach (var input in Languages)
			Groups["languages"].Add(input.Row);
	}

	private void AddGroupsToPage()
	{
		foreach (var group in Groups.Values)
			Add(group);
	}

	private void ClearInputs()
	{
		foreach (var input in Systems.Concat(Parameters).Concat(Functions).Concat(Languages))
			input.Dispose();

		Systems.Clear();
		Parameters.Clear();
		Functions.Clear();
		Languages.Clear();
	}

	public void RefreshUI()
	{
		InitializeUI();
	}

	public override void Dispose()
	{
		ClearInputs();
		base.Dispose();
	}
}