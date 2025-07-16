using System.Text;

namespace Gomoku;

public partial class GameHistory
{
	private List<(int x, int y, int player)> _moves = [];

	private int _currentMoveIndex = 0;

	public int CurrentMoveIndex
	{
		get => _currentMoveIndex;
		set => _currentMoveIndex = value;
	}
	public bool CanUndo => _currentMoveIndex > 0;
	public bool CanRedo => _currentMoveIndex < _moves.Count;
	public int Max => _moves.Count;

	public List<(int x, int y, int player)> Moves => _moves;

	public void AddMove(int x, int y, int player)
	{
		if (_currentMoveIndex < _moves.Count)
		{
			_moves.RemoveRange(_currentMoveIndex, _moves.Count - _currentMoveIndex);
		}
		_moves.Add((x, y, player));
		_currentMoveIndex++;
	}

	public (int x, int y, int player)? Undo(GameMode gameMode)
	{
		if (!CanUndo) return null;

		if (gameMode == GameMode.HumanVsEngine)
		{
			// Ember kezd: páros számokra (0,2,4,6...)
			var targetIndex = _currentMoveIndex - 2;
			if (targetIndex < 0) targetIndex = 0;
			_currentMoveIndex = targetIndex;
		}
		else
		{
			// AI kezd: páratlan számokra (1,3,5,7...)
			var targetIndex = _currentMoveIndex - 2;
			if (targetIndex < 1) targetIndex = 1;
			_currentMoveIndex = targetIndex;
		}

		return null; // Nem kell visszaadni semmit, mert rebuild-del dolgozunk
	}

	public (int x, int y, int player)? Redo(GameMode gameMode)
	{
		if (!CanRedo) return null;

		if (gameMode == GameMode.HumanVsEngine)
		{
			// Ember kezd: páros számokra előre
			var targetIndex = _currentMoveIndex + 2;
			if (targetIndex > _moves.Count) targetIndex = _moves.Count;
			_currentMoveIndex = targetIndex;
		}
		else
		{
			// AI kezd: páratlan számokra előre
			var targetIndex = _currentMoveIndex + 2;
			if (targetIndex > _moves.Count) targetIndex = _moves.Count;
			_currentMoveIndex = targetIndex;
		}

		return null;
	}

	public void Clear()
	{
		_moves.Clear();
		_currentMoveIndex = 0;
	}

	public string ToGomokuAIProtocol()
	{
		var result = new StringBuilder();
		result.AppendLine("BOARD");

		foreach (var (x, y, player) in _moves)
		{
			result.AppendLine($"{x},{y},{player}");
		}

		result.AppendLine("DONE");
		return result.ToString();
	}
}