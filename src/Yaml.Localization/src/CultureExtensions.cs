using System.Globalization;

namespace Yaml.Localization;

/// <summary>
/// Extension methods for culture-related types
/// </summary>
public static class CultureSettingsExtensions
{
	/// <summary>
	/// The active cultures.
	/// </summary>
	public static List<string> ActiveCultures(this IList<CultureItem>? cultures)
	{
		return cultures is { Count: > 0 } && cultures.Any(item => item.Active)
			? cultures.Where(item => item.Active).Select(s => s.Name!).ToList()
			: [CultureSettings.DefaultCultureName];
	}

	/// <summary>
	/// The culture infos supported by the application.
	/// </summary>
	public static IEnumerable<CultureInfo> ActiveCultureInfos(this IList<CultureItem>? cultures)
	{
		return cultures is { Count: > 0 } && cultures.Any(item => item.Active)
			? cultures
				.Where(where => where.Active)
				.Select(culture =>
					new CultureInfo(
						!string.IsNullOrWhiteSpace(culture.Name)
							? culture.Name
							: CultureSettings.DefaultCultureName)
				).ToList()
			: [new CultureInfo(CultureSettings.DefaultCultureName)];
	}

	/// <summary>
	/// The active culture info by key.
	/// </summary>
	public static CultureInfo? ActiveCultureInfo(this CultureSettings? settings, string key)
	{
		return settings?.Cultures?.ActiveCultureInfos().SingleOrDefault(item => item.Name == key);
	}

	/// <summary>
	/// The default culture.
	/// </summary>
	public static CultureItem DefaultCulture(this IList<CultureItem>? cultures)
	{
		return cultures is { Count: > 0 } && cultures.Any(item => item.Default)
			? cultures.Single(item => item.Default)
			: new CultureItem
			{
				Name = CultureSettings.DefaultCultureName,
				Default = true,
				Active = true
			};
	}

	/// <summary>
	/// The default culture info.
	/// </summary>
	public static CultureInfo DefaultCultureInfo(this CultureSettings? settings)
	{
		var defaultCulture = settings?.Cultures?.DefaultCulture();
		return new CultureInfo(
			!string.IsNullOrWhiteSpace(defaultCulture?.Name)
				? defaultCulture.Name
				: CultureSettings.DefaultCultureName);
	}

	/// <summary>
	/// The specific default culture info.
	/// </summary>
	public static CultureInfo SpecificDefaultCultureInfo(this CultureSettings? settings)
	{
		return settings.DefaultCultureInfo().Name.ToSpecificCulture();
	}

	/// <summary>
	/// The specific active culture infos.
	/// </summary>
	public static IEnumerable<CultureInfo> SpecificActiveCultureInfos(this IList<CultureItem>? cultures)
	{
		return cultures.ActiveCultureInfos().Select(culture => culture.Name.ToSpecificCulture());
	}

	/// <summary>
	/// The specific active cultures.
	/// </summary>
	public static List<string> SpecificActiveCultures(this CultureSettings? settings)
	{
		var activeCultures = settings?.Cultures?.ActiveCultures() ?? [CultureSettings.DefaultCultureName];
		return activeCultures.Select(cultureName => cultureName.ToSpecificCulture().Name).ToList();
	}

	/// <summary>
	/// Gets all supported culture infos.
	/// </summary>
	public static IEnumerable<CultureInfo> AllSpecificSupportedCultureInfos(this IList<CultureItem>? cultures)
	{
		return cultures.SpecificActiveCultureInfos()
			.SelectMany(culture => culture.GetCultureHierarchy())
			.Distinct()
			.ToList();
	}

	/// <summary>
	/// The active selector.
	/// </summary>
	public static Dictionary<string, string> ActiveSelector(this CultureSettings settings)
	{
		var activeCultures = settings.Cultures.ActiveCultures();
		return settings.Selector is { Count: > 0 }
			? settings.Selector.Where(item => activeCultures.Contains(item.Key) && item.Value != null)
				.ToDictionary(item => item.Key, item => item.Value!)
			: new Dictionary<string, string> { { CultureSettings.DefaultCultureName, "English" } };
	}

	/// <summary>
	/// The specific active selector.
	/// </summary>
	public static Dictionary<string, string> SpecificActiveSelector(this CultureSettings settings)
	{
		return settings.ActiveSelector().ToDictionary(
			s => s.Key.ToSpecificCulture().Name,
			s => s.Value);
	}

	/// <summary>
	/// The specific selected culture name.
	/// </summary>
	public static string SpecificSelectedCultureName(this CultureSettings settings)
	{
		var specificActiveSelector = settings.SpecificActiveSelector();
		return specificActiveSelector[CultureInfo.CurrentUICulture.Name.ToSpecificCulture().Name];
	}

	/// <summary>
	/// Converts a culture name to a specific culture.
	/// </summary>
	public static CultureInfo ToSpecificCulture(this string cultureName)
	{
		var culture = new CultureInfo(cultureName);
		while (culture.IsNeutralCulture)
			culture = CultureInfo.CreateSpecificCulture(culture.Name);
		return culture;
	}

	/// <summary>
	/// Gets the culture hierarchy for a given culture.
	/// </summary>
	public static IEnumerable<CultureInfo> GetCultureHierarchy(this CultureInfo culture)
	{
		var current = culture;
		while (!current.Equals(CultureInfo.InvariantCulture))
		{
			yield return current;
			current = current.Parent;
		}
	}
}
