namespace Gomoku;

public partial class Board
{
	private void SetupInputHandlers()
	{
		double _lastDragX = 0;
		double _lastDragY = 0;

		var click = Gtk.GestureClick.New();

		click.OnBegin += (gesture, args) =>
		{
			if (!IsInteractive) return;

			gesture.GetPoint(null, out _lastDragX, out _lastDragY);
			double cellSize = CanvasSize.Height < CanvasSize.Width
				? (CanvasSize.Height - 1) / _boardModel.BoardSize
				: (CanvasSize.Width - 1) / _boardModel.BoardSize;
			int row = (int)(_lastDragY / cellSize);
			int col = (int)(_lastDragX / cellSize);
			if (row >= 0 && row < _boardModel.BoardSize && col >= 0 &&
				col < _boardModel.BoardSize)
			{
				StoneCoordinate = new StoneCoordinate(row, col, _boardModel.BoardSize);
			}
			else
			{
				StoneCoordinate = default!;
			}
		};

		click.OnEnd += (gesture, args) =>
		{
			if (!IsInteractive) return;

			if (StoneCoordinate != null)
			{
				OnMoveMade();
			}
		};

		click.OnUpdate += (gesture, args) =>
		{
			if (!IsInteractive) return;

			gesture.GetPoint(null, out var currentX, out var currentY);
			double cellSize = CanvasSize.Height < CanvasSize.Width
				? (CanvasSize.Height - 1) / _boardModel.BoardSize
				: (CanvasSize.Width - 1) / _boardModel.BoardSize;
			int row = (int)(currentY / cellSize);
			int col = (int)(currentX / cellSize);
			if (row >= 0 && row < _boardModel.BoardSize && col >= 0 &&
				col < _boardModel.BoardSize)
			{
				StoneCoordinate = new StoneCoordinate(row, col, _boardModel.BoardSize);

			}
			else
			{
				StoneCoordinate = default!;
			}
		};

		AddController(click);
	}

	public bool IsInteractive { get; set; }
}
