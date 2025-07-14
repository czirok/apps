namespace BlazorShared.Layout;

public static class Extensions
{
	static readonly IFormatProvider InvariantCulture = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;

	public static string ToStringInvariant<T>(this T source, string? format = null)
	{
		return format == null ?
			FormattableString.Invariant($"{source}") :
			string.Format(InvariantCulture, $"{{0:{format}}}", source);
	}
}
