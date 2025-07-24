using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EasyUIBinding;

public enum FlowerType
{
	Rose,
	Tulip,
	Orchid,
	Daisy,
	Sunflower,
	Lily,
	Peony
}

public enum DayPhase
{
	Morning,
	Afternoon,
	Evening,
	Night
}

public enum ZodiacSign
{
	Aries,
	Taurus,
	Gemini,
	Cancer,
	Leo,
	Virgo,
	Libra,
	Scorpio,
	Sagittarius,
	Capricorn,
	Aquarius,
	Pisces
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public partial class SampleModel : ObservableObject
{
	[ObservableProperty]
	public partial string Clipboard { get; set; } = "Sample text copied to clipboard.";

	[ObservableProperty]
	public partial Gdk.RGBA Color { get; set; } = new() { Red = 0.8f, Green = 0.5f, Blue = 0.2f, Alpha = 1.0f };

	[ObservableProperty]
	public partial FlowerType Flower { get; set; } = FlowerType.Lily;

	[ObservableProperty]
	public partial string FlowerName { get; set; }

	[ObservableProperty]
	public partial string Font { get; set; } = default!;

	[ObservableProperty]
	public partial string SaveAs { get; set; } = "sample.txt";

	[ObservableProperty]
	public partial string LoadedFile { get; set; } = "sample.txt";

	[ObservableProperty]
	public partial double SpinDouble { get; set; } = 0.42;

	[ObservableProperty]
	public partial float SpinFloat { get; set; } = 0.42f;

	[ObservableProperty]
	public partial int SpinInt { get; set; } = 42;

	[ObservableProperty]
	public partial string Text { get; set; } = "Sample text";

	[ObservableProperty]
	public partial string Password { get; set; } = "password";

	[ObservableProperty]
	public partial bool PowerButton { get; set; } = true;

	[ObservableProperty]
	public partial DayPhase DayPhase { get; set; } = DayPhase.Night;

	[ObservableProperty]
	public partial string DayPhaseName { get; set; }

	[ObservableProperty]
	public partial string View { get; set; } = "Static View";

	[ObservableProperty]
	public partial ZodiacSign ZodiacSign { get; set; } = ZodiacSign.Leo;

	[ObservableProperty]
	public partial string ZodiacSignName { get; set; }

	[ObservableProperty]
	public partial double Volume { get; set; } = 50.0;
}
