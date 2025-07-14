using System.Globalization;

namespace YamlLocalizationTests;

public static class Extensions
{
	public static void SwitchToThisCulture(this string culture)
	{
		CultureInfo.CurrentCulture = new CultureInfo(culture);
		CultureInfo.CurrentUICulture = new CultureInfo(culture);
	}
}