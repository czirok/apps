using Microsoft.Extensions.Localization;

namespace Gomoku;

public partial class Board
{
	private BoardModel _boardModel;

	public IGameEngine GameEngine { get; set; } = default!;

	StoneColor _aiColor;
	StoneColor _humanColor = StoneColor.Empty;

	private readonly IStringLocalizer<I18N> L;

	public Board(BoardModel boardModel, IStringLocalizer<I18N> localizer)
	{
		L = localizer;
		_boardModel = boardModel;
		Vexpand = true;
		Hexpand = true;

		SetupInputHandlers();

		Show();
	}

	public void BeginGame(StoneColor humanColor, StoneColor aiColor)
	{
		_stoneCoordinates = default!;
		_winningStones = default!;
		_stones = new StoneColor[_boardModel.BoardSize, _boardModel.BoardSize];
		_aiColor = aiColor;
		_humanColor = humanColor;
		if (_humanColor == StoneColor.White)
		{
			UpdateStatusOnMainThread("You start");
			IsInteractive = true;
			InvalidateOnMainThread();
		}
		else
		{
			AIMove();
		}
	}

	public void AIMove()
	{
		IsInteractive = false;

		UpdateStatusOnMainThread("Thinking");
		var threadStart = new ThreadStart(Thinking);
		var thread = new Thread(threadStart)
		{
			Priority = ThreadPriority.BelowNormal
		};
		thread.Start();
	}

	void Thinking()
	{
		// Console.WriteLine("=== BEFORE AI THINKING ===");
		// Console.WriteLine(GameEngine.GetDebugState());

		StoneCoordinate stoneCoordinate = GameEngine.GetBestMove(Stones, _aiColor, new TimeSpan(0, 0, _boardModel.ThinkingTime));

		// Console.WriteLine("=== AFTER AI THINKING ===");
		// Console.WriteLine(GameEngine.GetDebugState());

		if (MakeMove(stoneCoordinate, _aiColor))
		{
			InvalidateOnMainThread();
			if (HasWinner)
			{
				UpdateStatusOnMainThread("You lost");
				IsInteractive = false;
				InvalidateOnMainThread();
			}
			else
			{
				UpdateStatusOnMainThread("You move");
				IsInteractive = true;
			}
		}
	}

	void OnMoveMade()
	{
		IsInteractive = false;

		StoneCoordinate stoneCoordinate = StoneCoordinate;
		StoneCoordinate = default!;
		if (MakeMove(stoneCoordinate, _humanColor))
		{
			InvalidateOnMainThread();
			if (HasWinner)
			{
				UpdateStatusOnMainThread("You won");
				IsInteractive = false;
				InvalidateOnMainThread();
			}
			else
			{
				AIMove();
			}
		}
	}

	private string _lastLabel = string.Empty;
	public void UpdateStatusOnMainThread(string text)
	{
		_lastLabel = text;
		var localizedText = L[text];
		GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_LOW, new GLib.SourceFunc(() =>
		{
			_boardModel.StatusLabel.SetLabel(localizedText);
			return GLib.Constants.SOURCE_REMOVE;
		}));
	}

	public void ReplaceStatusOnMainThread()
	{
		if (string.IsNullOrEmpty(_lastLabel))
			return;
		var localizedText = L[_lastLabel];
		GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_LOW, new GLib.SourceFunc(() =>
		{
			_boardModel.StatusLabel.SetLabel(localizedText);
			return GLib.Constants.SOURCE_REMOVE;
		}));
	}

	public void InvalidateOnMainThread()
	{
		GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_LOW, new GLib.SourceFunc(() =>
		{
			Invalidate();
			return GLib.Constants.SOURCE_REMOVE;
		}));
	}

}
