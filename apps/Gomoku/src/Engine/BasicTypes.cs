namespace Gomoku;

public enum StoneColor
{
	Black = 1,
	White = -1,
	Empty = 0
}

public class StoneCoordinate
{
	public StoneCoordinate(int row, int col, int boardSize)
	{
		if ((row >= 0) && (row < boardSize))
			Row = row;
		else
			throw new Exception("The value of 'row' is out of the allowed range");
		if ((col >= 0) && (col < boardSize))
			Col = col;
		else
			throw new Exception("The value of 'col' is out of the allowed range");
	}

	public readonly int Row;

	public readonly int Col;
}
