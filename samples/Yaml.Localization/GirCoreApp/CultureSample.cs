using EasyUIBinding.GirCore;
using EasyUIBinding.GirCore.Binding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Yaml.Localization;

namespace GirCoreApp;

public class CultureSample : Adw.PreferencesPage, IDisposable
{
	internal readonly List<Input> Sample;
	internal readonly List<Input> Languages;

	internal Dictionary<string, Adw.PreferencesGroup> Groups;

	private readonly CultureSettings _cultures;
	private readonly IStringLocalizer<CultureSample> L;
	private readonly ILogger<CultureSample> _logger;
	private readonly DateTime dt = DateTime.Now;
	private readonly double number = 1999.69;
	public event Action? LanguageChanged;

	public CultureSample(CultureSettings cultureSettings, IStringLocalizer<CultureSample> localizer, ILogger<CultureSample> logger)
	{
		ArgumentNullException.ThrowIfNull(cultureSettings);
		ArgumentNullException.ThrowIfNull(localizer);
		ArgumentNullException.ThrowIfNull(logger);

		_cultures = cultureSettings;
		L = localizer;
		_logger = logger;

		Groups = [];
		Sample = [];
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
			["sample"] = UI.Group(L["Hello, world!"], L["Welcome to your new app."]),
			["languages"] = UI.Group(L["Languages"])
		};
	}

	private void CreateInputs()
	{
		Sample.AddRange([
			new View("current-culture", "CultureInfo.CurrentCulture.Name", CultureInfo.CurrentCulture.Name),
			new View("current-ui-culture", "CultureInfo.CurrentUICulture.Name", CultureInfo.CurrentUICulture.Name),
			new View("number", L["Number"], number.ToString("N", CultureInfo.CurrentUICulture)),
			new View("date", L["Date"], dt.ToString("D", CultureInfo.CurrentUICulture)),
			new View("language", string.Format(L["Language ({0})"], CultureInfo.CurrentUICulture.Name), CultureInfo.CurrentUICulture.Name)
		]);


		Languages.AddRange([
			new WrapToggle<string>(
				"languages",
				_cultures.SpecificActiveSelector(),
				_cultures.SpecificDefaultCultureInfo().Name,
				CultureInfo.CurrentUICulture.Name)
				.OnChanged((sender, args) => {
					if (args is not InputDictionaryChangedEventArgs<string> inputArgs)
						return;

					_logger.LogInformation("Language changed to {Key}", inputArgs.Key);

					if (inputArgs.Title == null || !_cultures.SpecificActiveCultures().Contains(inputArgs.Key))
						return;

					var culture = inputArgs.Key.ToSpecificCulture() ?? _cultures.SpecificDefaultCultureInfo();
					_logger.LogInformation("Changing culture to {Culture}", culture.Name);

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
		foreach (var input in Sample)
			Groups["sample"].Add(input.Row);

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
		foreach (var input in Sample.Concat(Languages))
			input.Dispose();

		Sample.Clear();
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
