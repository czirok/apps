namespace Gomoku;

public partial class Board
{
	const int StonesRequiredToWin = 5;

	StoneColor[,] _stones = new StoneColor[20, 20];
	StoneCoordinate[] _winningStones = default!;

	public StoneColor[,] Stones => (StoneColor[,])_stones.Clone();

	public bool MakeMove(StoneCoordinate stoneCoordinate, StoneColor stoneColor)
	{
		if (_stones[stoneCoordinate.Row, stoneCoordinate.Col] != StoneColor.Empty || HasWinner)
			return false;

		_stones[stoneCoordinate.Row, stoneCoordinate.Col] = stoneColor;
		FindWinningSequence();

		int player = stoneColor == StoneColor.Black ? 1 : 2;
		_boardModel.GameHistory.AddMove(stoneCoordinate.Row, stoneCoordinate.Col, player);
		_boardModel.GomokuAIProtocol = _boardModel.GameHistory.ToGomokuAIProtocol();

		return true;
	}

	internal void RebuildBoardFromHistory()
	{
		_stones = new StoneColor[_boardModel.BoardSize, _boardModel.BoardSize];
		_winningStones = default!;

		foreach ((int x, int y, int player) in _boardModel.GameHistory.Moves.Take(_boardModel.GameHistory.CurrentMoveIndex))
		{
			_stones[x, y] = player == 1 ? StoneColor.Black : StoneColor.White;
		}

		FindWinningSequence();
		InvalidateOnMainThread();
	}

	void FindWinningSequence()
	{
		_winningStones = default!;
		FindStraightWinningLines(BoardTransformation.None);
		if (_winningStones == null)
			FindStraightWinningLines(BoardTransformation.Symmetry);
		if (_winningStones == null)
			FindDiagonalWinningLines(BoardTransformation.None);
		if (_winningStones == null)
			FindDiagonalWinningLines(BoardTransformation.Rotation);
	}

	public bool HasWinner => _winningStones != null;

	public StoneColor WinnerStone
	{
		get
		{
			if (_winningStones == null)
				return StoneColor.Empty;
			else
				return _stones[_winningStones[0].Row, _winningStones[0].Col];
		}
	}

	public StoneCoordinate[] WinningStones => (StoneCoordinate[])_winningStones.Clone();

	enum BoardTransformation
	{
		None = 0,
		Symmetry = 1,
		Rotation = 2
	}

	StoneColor GetBoardPosition(int row, int col, BoardTransformation transformation)
	{
		int x = -1, y = -1;
		switch (transformation)
		{
			case BoardTransformation.None:
				x = row;
				y = col;
				break;
			case BoardTransformation.Symmetry:
				x = col;
				y = row;
				break;
			case BoardTransformation.Rotation:
				x = _boardModel.BoardSize - 1 - col;
				y = row;
				break;
		}

		return x >= 0 && x < _boardModel.BoardSize && y >= 0 && y < _boardModel.BoardSize
			? _stones[x, y]
			: StoneColor.Empty;
	}

	void FindStraightWinningLines(BoardTransformation transformation)
	{
		int sequence = 1;
		for (int y = 0; y < _boardModel.BoardSize; y++)
			for (int x = 0; x < _boardModel.BoardSize; x++)
			{
				if (GetBoardPosition(x, y, transformation) == StoneColor.Black && GetBoardPosition(x - 1, y, transformation) == StoneColor.Black)
					sequence++;
				else if (GetBoardPosition(x, y, transformation) == StoneColor.White && GetBoardPosition(x - 1, y, transformation) == StoneColor.White)
					sequence++;
				else if (GetBoardPosition(x, y, transformation) == StoneColor.Black && GetBoardPosition(x - 1, y, transformation) != StoneColor.Black)
					sequence = 1;
				else if (GetBoardPosition(x, y, transformation) == StoneColor.White && GetBoardPosition(x - 1, y, transformation) != StoneColor.White)
					sequence = 1;
				else
					sequence = 0;
				if (sequence == StonesRequiredToWin)
				{
					_winningStones = new StoneCoordinate[StonesRequiredToWin];
					for (int i = x - StonesRequiredToWin + 1; i <= x; i++)
						if (transformation == BoardTransformation.None)
							_winningStones[i - (x - StonesRequiredToWin + 1)] = new StoneCoordinate(i, y, _boardModel.BoardSize);
						else
							_winningStones[i - (x - StonesRequiredToWin + 1)] = new StoneCoordinate(y, i, _boardModel.BoardSize);
					return;
				}
			}
	}

	void FindDiagonalWinningLines(BoardTransformation transformace)
	{
		int rada = 1;
		for (int diagonal = -_boardModel.BoardSize + 1; diagonal <= _boardModel.BoardSize - 1; diagonal++)
			for (int x = 0; x < _boardModel.BoardSize; x++)
			{
				if (GetBoardPosition(x + diagonal, x, transformace) == StoneColor.Black && GetBoardPosition(x + diagonal - 1, x - 1, transformace) == StoneColor.Black)
					rada++;
				else if (GetBoardPosition(x + diagonal, x, transformace) == StoneColor.White && GetBoardPosition(x + diagonal - 1, x - 1, transformace) == StoneColor.White)
					rada++;
				else if (GetBoardPosition(x + diagonal, x, transformace) == StoneColor.Black && GetBoardPosition(x + diagonal - 1, x - 1, transformace) != StoneColor.Black)
					rada = 1;
				else if (GetBoardPosition(x + diagonal, x, transformace) == StoneColor.White && GetBoardPosition(x + diagonal - 1, x - 1, transformace) != StoneColor.White)
					rada = 1;
				else rada = 0;
				if (rada == StonesRequiredToWin)
				{
					_winningStones = new StoneCoordinate[StonesRequiredToWin];
					for (int i = x - StonesRequiredToWin + 1; i <= x; i++)
						if (transformace == BoardTransformation.None)
							_winningStones[i - (x - StonesRequiredToWin + 1)] = new StoneCoordinate(i + diagonal, i, _boardModel.BoardSize);
						else
							_winningStones[i - (x - StonesRequiredToWin + 1)] = new StoneCoordinate(_boardModel.BoardSize - 1 - i, i + diagonal, _boardModel.BoardSize);
					return;
				}
			}
	}
}
