using EasyUIBinding.GirCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace EasyUIBinding;

public class SamplePreferencesPage : Gtk.Box, IDisposable
{
	private readonly ILogger<SamplePreferencesPage> _logger;
	private readonly SampleModel _model = new();
	private readonly Gtk.FileFilter _fileFilter = new()
	{
		Name = "Text Files",
	};

	internal readonly List<Input> LeftInputs;
	internal readonly List<Input> RightInputs;
	internal Adw.PreferencesGroup LeftGroup;
	internal Adw.PreferencesGroup RightGroup;

	public SamplePreferencesPage(ILogger<SamplePreferencesPage> logger)
	{
		ArgumentNullException.ThrowIfNull(logger);

		_logger = logger;

		_fileFilter.AddPattern("*.txt");

		LeftGroup = Adw.PreferencesGroup.New();
		LeftGroup.Title = "Inputs";
		LeftInputs = [];
		RightGroup = Adw.PreferencesGroup.New();
		RightGroup.Title = "Inputs";
		RightInputs = [];

		CreateInputs();

		foreach (var input in LeftInputs)
		{
			LeftGroup.Add(input.Row);
		}

		foreach (var input in RightInputs)
		{
			RightGroup.Add(input.Row);
		}

		var box = Gtk.Box.New(Gtk.Orientation.Horizontal, 0).Design();
		box.Homogeneous = true;
		box.Append(LeftGroup);
		box.Append(RightGroup);

		Append(box);
	}

	private void CreateInputs()
	{
		LeftInputs.AddRange([
			new Button("click", "Click")
				.OnClick(() => {
					_logger.LogInformation("Button clicked.");
				})
				.OnClick((sender, args) => {
					_logger.LogInformation("Button clicked with sender: {Sender}", sender);
				}),

			new ClipboardButton("copy", "Copy to Clipboard")
				.BindTo(_model, nameof(SampleModel.Clipboard))
				.OnClipboardButtonClicked(() => {
					_logger.LogInformation("Clipboard changed with value: {Value}", _model.Clipboard);
				})
				.OnClipboardButtonClicked((sender, args) => {
					_logger.LogInformation("Clipboard button clicked with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),

			new ColorSelector("color", "Select Color", _model.Color)
				.BindTo(_model, nameof(SampleModel.Color))
				.OnColorSelected(() => {
					_logger.LogInformation("Color changed to: {Color}", _model.Color.ToString());
				})
				.OnColorSelected((sender, args) => {
					_logger.LogInformation("Color selected with sender: {Sender}. Value: {Value}", sender, args.Value != null ? args.Value.ToString() : "null");
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
			}, _model.Flower, FlowerType.Lily)
				.BindTo(_model, nameof(SampleModel.Flower))
				.BindTo(_model, nameof(SampleModel.FlowerName))
				.OnComboChanged(() => {
					_logger.LogInformation("Flower changed to: {Flower}", _model.FlowerName);
				})
				.OnComboChanged<FlowerType>((sender, args) => {
					_logger.LogInformation("Combo changed with sender: {Sender}. {Key}:{Tilte}", sender, args.Key, args.Title);
				}),

			new FontSelector("font", "Select Font", "Sans 12")
				.BindTo(_model, nameof(SampleModel.Font))
				.OnFontSelected((sender, args) => {
					_logger.LogInformation("Font changed to: {Font}", _model.Font);
				})
				.OnFontSelected((sender, args) => {
					_logger.LogInformation("Font selected with sender: {Sender}. Font: {Font}", sender, args.Value);
				}),

			new SaveAsSelector("save-as", "Save As", Environment.CurrentDirectory, _fileFilter)
				.BindTo(_model, nameof(SampleModel.SaveAs))
				.OnSaveAsSelected(() => {
					_logger.LogInformation("Save As changed to: {SaveAs}", _model.SaveAs);
				})
				.OnSaveAsSelected((sender, args) => {
					_logger.LogInformation("Save As changed with sender: {Sender}. Value: {Value}", sender, args.Value);
					try
					{
						sender.ShowSuccess();
					}
					catch
					{
						sender.ShowError();
					}
				}),

			new FileSelector("load-file", "Load File", Environment.CurrentDirectory, _fileFilter)
				.BindTo(_model, nameof(SampleModel.LoadedFile))
				.OnFileSelected(() => {
					_logger.LogInformation("File selected: {File}", _model.LoadedFile);
				})
				.OnFileSelected((sender, args) => {
					_logger.LogInformation("File selected with sender: {Sender}. Value: {Value}", sender, args.Value);
					try
					{
						sender.ShowSuccess();
					}
					catch
					{
						sender.ShowError();
					}
				}),

			new SpinDouble("spin-double", "Spin Double", 0.42, new DoubleRange(-8.0d, 4.0d, 0.02d))
				.BindTo(_model, nameof(SampleModel.SpinDouble))
				.OnSpinDoubleChanged(() => {
					_logger.LogInformation("Spin Double changed to: {Value}", _model.SpinDouble);
				})
				.OnSpinDoubleChanged((sender, args) => {
					_logger.LogInformation("Spin Double changed with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),

			new SpinFloat("spin-float", "Spin Float", 0.42f, new FloatRange(-10.0f, 10.0f, 0.007f))
				.BindTo(_model, nameof(SampleModel.SpinFloat))
				.OnSpinFloatChanged(() => {
					_logger.LogInformation("Spin Float changed to: {Value}", _model.SpinFloat);
				})
				.OnSpinFloatChanged((sender, args) => {
					_logger.LogInformation("Spin Float changed with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),

			new SpinInteger("spin-int", "Spin Int", 42, new IntRange(-100, 100, 5))
				.BindTo(_model, nameof(SampleModel.SpinInt))
				.OnSpinIntegerChanged(() => {
					_logger.LogInformation("Spin Int changed to: {Value}", _model.SpinInt);
				})
				.OnSpinIntegerChanged((sender, args) => {
					_logger.LogInformation("Spin Int changed with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),
		]);

		RightInputs.AddRange([
			new Text("text", "Text Input", _model.Text)
				.BindTo(_model, nameof(SampleModel.Text))
				.OnTextChanged(() => {
					_logger.LogInformation("Text changed to: {Text}", _model.Text);
				})
				.OnTextChanged((sender, args) => {
					_logger.LogInformation("Text changed with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),

			new Password("password", "Enter Password", "")
				.BindTo(_model, nameof(SampleModel.Password))
				.OnPasswordChanged(() => {
					_logger.LogInformation("Password changed.");
				})
				.OnPasswordChanged((sender, args) => {
					_logger.LogInformation("Password changed with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),

			new Switch("toggle", "Toggle Switch", _model.PowerButton)
				.BindTo(_model, nameof(SampleModel.PowerButton))
				.OnSwitchToggled(() => {
					_logger.LogInformation("Toggle changed to: {Value}", _model.PowerButton);
				})
				.OnSwitchToggled((sender, args) => {
					_logger.LogInformation("Toggle changed with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),

			new Toggle<DayPhase>("day-phase", "Day Phase", new Dictionary<DayPhase, string>
			{
				{ DayPhase.Morning, "Morning" },
				{ DayPhase.Afternoon, "Afternoon" },
				{ DayPhase.Evening, "Evening" },
				{ DayPhase.Night, "Night" }
			}, _model.DayPhase)
				.BindTo(_model, nameof(SampleModel.DayPhase))
				.OnToggleChanged(() => {
					_logger.LogInformation("Day phase changed to: {DayPhase}", _model.DayPhase);
				})
				.OnToggleChanged<DayPhase>((sender, args) => {
					_logger.LogInformation("Day phase changed with sender: {Sender}. {Key}: {Title}", sender, args.Key, args.Title);
				}),

			new View("view", "Readonly text", _model.View),

			new WrapToggle<ZodiacSign>("zodiac-signs", "Zodiac Signs", new Dictionary<ZodiacSign, string>
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
				.OnWrapToggleChanged(() => {
					_logger.LogInformation("Zodiac sign changed to: {ZodiacSign}", _model.ZodiacSign);
				})
				.OnWrapToggleChanged<ZodiacSign>((sender, args) => {
					_logger.LogInformation("Zodiac sign changed with sender: {Sender}. {Key}: {Title}", sender, args.Key, args.Title);
				}),

			new ScaleDouble("volume", "Volume", 50.0, new DoubleRange(0, 100, 1))
				.BindTo(_model, nameof(SampleModel.Volume))
				.OnScaleChanged(() => {
					_logger.LogInformation("Volume changed to: {Volume}", _model.Volume);
				})
				.OnScaleChanged((sender, args) => {
					_logger.LogInformation("Volume changed with sender: {Sender}. Value: {Value}", sender, args.Value);
				}),
		]);
	}

	public override void Dispose()
	{
		foreach (var input in LeftInputs)
		{
			input.Dispose();
		}
		base.Dispose();
	}
}