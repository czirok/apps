using CommunityToolkit.Mvvm.ComponentModel;

namespace Gomoku;

public enum GameMode
{
	HumanVsEngine,
	EngineVsHuman
}

public partial class BoardModel : ObservableObject
{
	public GameHistory GameHistory { get; set; } = new();

	public Gtk.Label StatusLabel { get; set; } = new Gtk.Label()
	{
		CssClasses = ["title-2"],
		Valign = Gtk.Align.Start,
		Halign = Gtk.Align.Center
	};

	[ObservableProperty]
	public partial int ThinkingTime { get; set; } = 10;

	[ObservableProperty]
	public partial int BoardSize { get; set; } = 20;

	[ObservableProperty]
	public partial GameMode GameMode { get; set; } = GameMode.HumanVsEngine;

	[ObservableProperty]
	public partial BoardTheme BoardTheme { get; set; } = BoardTheme.WoodWithStones;

	private string gomokuAIProtocol = string.Empty;
	public string GomokuAIProtocol
	{
		get => gomokuAIProtocol;
		set
		{
			if (gomokuAIProtocol != value)
			{
				gomokuAIProtocol = value;
				OnPropertyChanged();
			}
		}
	}
}

public enum BoardTheme
{
	WoodWithStones,
	PaperAndPencil,
}