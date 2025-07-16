using EasyUIBinding.GirCore.Binding;

namespace Gomoku;

public enum GameMode
{
	HumanVsEngine,
	EngineVsHuman
}

public partial class BoardModel : NotifyPropertyModel
{
	public GameHistory GameHistory { get; set; } = new();

	public Gtk.Label StatusLabel { get; set; } = new Gtk.Label()
	{
		CssClasses = ["title-2"],
		Valign = Gtk.Align.Start,
		Halign = Gtk.Align.Center
	};

	[GirCoreNotify]
	private int thinkingTime = 10;

	[GirCoreNotify]
	private int boardSize = 20;

	[GirCoreNotify]
	private GameMode gameMode = GameMode.HumanVsEngine;

	[GirCoreNotify]
	private BoardTheme boardTheme = BoardTheme.WoodWithStones;

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