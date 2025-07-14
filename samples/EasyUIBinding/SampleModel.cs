using EasyUIBinding.GirCore.Binding;

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

public partial class SampleModel : NotifyPropertyModel
{
	[GirCoreNotify]
	private string clipboard = "Sample text copied to clipboard.";

	[GirCoreNotify]
	private Gdk.RGBA color = new() { Red = 0.8f, Green = 0.5f, Blue = 0.2f, Alpha = 1.0f };

	[GirCoreNotify]
	private FlowerType flower = FlowerType.Rose;

	[GirCoreNotify]
	private string flowerName = "Rose";

	[GirCoreNotify]
	private string font = default!;

	[GirCoreNotify]
	private string saveAs = "sample.txt";

	[GirCoreNotify]
	private double spinDouble;

	[GirCoreNotify]
	private float spinFloat;

	[GirCoreNotify]
	private int spinInt;

	[GirCoreNotify]
	private string text = "Sample text";

	[GirCoreNotify]
	private bool powerButton = true;

	[GirCoreNotify]
	private DayPhase dayPhase = DayPhase.Morning;

	[GirCoreNotify]
	private string dayPhaseName = default!;

	[GirCoreNotify]
	private string view = "Static View";

	[GirCoreNotify]
	private ZodiacSign zodiacSign = ZodiacSign.Leo;

	[GirCoreNotify]
	private string zodiacSignName = "Leo";
}
