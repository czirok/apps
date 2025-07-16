namespace Gomoku;

public interface IGameEngine
{
	StoneCoordinate GetBestMove(StoneColor[,] board, StoneColor colorOnMove, TimeSpan thinkingTime);
	string GetDebugState();
	void SetMoves(List<(int x, int y, int player)> moves);
}
