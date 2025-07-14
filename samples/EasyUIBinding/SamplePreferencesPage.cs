using EasyUIBinding.GirCore.Binding;
using Microsoft.Extensions.Logging;

namespace EasyUIBinding;

public class SamplePreferencesPage : Adw.PreferencesPage, IDisposable
{
	private readonly ILogger<SamplePreferencesPage> _logger;
	private readonly SampleModel _model = new();
	private readonly Gtk.FileFilter _fileFilter = new()
	{
		Name = "Text Files",
	};

	internal readonly List<Input> Inputs;
	internal Adw.PreferencesGroup Group;

	public SamplePreferencesPage(ILogger<SamplePreferencesPage> logger)
	{
		ArgumentNullException.ThrowIfNull(logger);

		_logger = logger;

		_fileFilter.AddPattern("*.txt");

		Group = Adw.PreferencesGroup.New();
		Inputs = [];

		Group = Adw.PreferencesGroup.New();
		Group.Title = "Samples";

		CreateInputs();

		foreach (var input in Inputs)
		{
			Group.Add(input.Row);
		}

		Add(Group);
	}

	private void CreateInputs()
	{
		Inputs.AddRange([
			new Button("click", new ButtonLabel("Click"))
				.OnClick(() => {
					_logger.LogInformation("Button clicked.");
				}),

			new ClipboardButton("copy", "Copy to Clipboard")
				.BindTo(_model, nameof(SampleModel.Clipboard))
				.OnClick(() => {
					_logger.LogInformation("Clipboard changed with value: {Value}", _model.Clipboard);
				}),

			new ColorSelector("color", "Select Color", _model.Color)
				.BindTo(_model, nameof(SampleModel.Color))
				.OnChanged(() => {
					_logger.LogInformation("Color changed to: {Color}", _model.Color.ToString());
				}),

			new Combo<FlowerType>("flower", "Select Flower", new Dictionary<FlowerType, string>
			{
				{ FlowerType.Rose, "Red Rose" },
				{ FlowerType.Tulip, "Yellow Tulip" },
				{ FlowerType.Orchid, "Purple Orchid" },
				{ FlowerType.Daisy, "White Daisy" },
				{ FlowerType.Sunflower, "Yellow Sunflower" },
				{ FlowerType.Lily, "Pink Lily" },
				{ FlowerType.Peony, "Peony" }
			}, _model.Flower)
				.BindTo(_model, nameof(SampleModel.Flower))
				.BindTo(_model, nameof(SampleModel.FlowerName))
				.OnChanged(() => {
					_logger.LogInformation("Flower changed to: {Flower}", _model.FlowerName);
				}),

			new FontSelector("font", "Select Font", "Sans 12")
				.BindTo(_model, nameof(SampleModel.Font))
				.OnChanged(() => {
					_logger.LogInformation("Font changed to: {Font}", _model.Font);
				}),

			new SaveAsSelector("save-as", "Save As", Environment.CurrentDirectory, _fileFilter)
				.BindTo(_model, nameof(SampleModel.SaveAs))
				.OnChanged(() => {
					_logger.LogInformation("Save As changed to: {SaveAs}", _model.SaveAs);
				}),

			new SpinDouble("spin-double", "Spin Double", 0.0, new DoubleRange(-8.0d, 4.0d, 0.04d))
				.BindTo(_model, nameof(SampleModel.SpinDouble))
				.OnChanged(() => {
					_logger.LogInformation("Spin Double changed to: {Value}", _model.SpinDouble);
				}),

			new SpinFloat("spin-float", "Spin Float", 0.0f, new FloatRange(-10.0f, 10.0f, 0.007f))
				.BindTo(_model, nameof(SampleModel.SpinFloat))
				.OnChanged(() => {
					_logger.LogInformation("Spin Float changed to: {Value}", _model.SpinFloat);
				}),

			new SpinInteger("spin-int", "Spin Int", 0, new IntRange(-100, 100, 5))
				.BindTo(_model, nameof(SampleModel.SpinInt))
				.OnChanged(() => {
					_logger.LogInformation("Spin Int changed to: {Value}", _model.SpinInt);
				}),

			new Text("text", "Text Input", _model.Text)
				.BindTo(_model, nameof(SampleModel.Text))
				.OnChanged(() => {
					_logger.LogInformation("Text changed to: {Value}", _model.Text);
				}),

			new Switch("toggle", "Toggle Switch", _model.PowerButton)
				.BindTo(_model, nameof(SampleModel.PowerButton))
				.OnChanged(() => {
					_logger.LogInformation("Toggle changed to: {Value}", _model.PowerButton);
				}),

			new Toggle<DayPhase>("day-phase", "Day Phase", new Dictionary<DayPhase, string>
			{
				{ DayPhase.Morning, "Morning" },
				{ DayPhase.Afternoon, "Afternoon" },
				{ DayPhase.Evening, "Evening" },
				{ DayPhase.Night, "Night" }
			}, _model.DayPhase)
				.BindTo(_model, nameof(SampleModel.DayPhase))
				.OnChanged(() => {
					_logger.LogInformation("Day phase changed to: {DayPhase}", _model.DayPhase);
				}),

			new View("view", "Readonly text", _model.View),

			new WrapToggle<ZodiacSign>("zodiac-signs", new Dictionary<ZodiacSign, string>
			{
				{ ZodiacSign.Aries, "Aries" },
				{ ZodiacSign.Taurus, "Taurus" },
				{ ZodiacSign.Gemini, "Gemini" },
				{ ZodiacSign.Cancer, "Cancer" },
				{ ZodiacSign.Leo, "Leo" },
				{ ZodiacSign.Virgo, "Virgo" },
				{ ZodiacSign.Libra, "Libra" },
				{ ZodiacSign.Scorpio, "Scorpio" },
				{ ZodiacSign.Sagittarius, "Sagittarius" },
				{ ZodiacSign.Capricorn, "Capricorn" },
				{ ZodiacSign.Aquarius, "Aquarius" },
				{ ZodiacSign.Pisces, "Pisces" }
			}, _model.ZodiacSign, ZodiacSign.Leo)
				.BindTo(_model, nameof(SampleModel.ZodiacSign))
				.OnChanged(() => {
					_logger.LogInformation("Zodiac sign changed to: {ZodiacSign}", _model.ZodiacSign);
				})
		]);
	}

	public override void Dispose()
	{
		foreach (var input in Inputs)
		{
			input.Dispose();
		}
		base.Dispose();
	}
}