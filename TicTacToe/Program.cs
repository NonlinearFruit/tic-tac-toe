using Player = System.Func<char[], int>;

namespace TicTacToe;

public class Program
{
    private const char nill = ' ';
    public record GameResult(char Winner, char[] Board);
    private record Game(Player CurrentPlayer, Player X, Player O, char[] Board);
    private static IEnumerable<int[]> Lines = new Dictionary<string, int[][]>
    {
        ["rows"] = [[0, 1, 2], [3, 4, 5], [6, 7, 8]],
        ["columns"] = [[0, 3, 6], [1, 4, 7], [2, 5, 8]],
        ["diagonals"] = [[0, 4, 8], [2, 4, 6]]
    }.Values.SelectMany(v => v);

    public static void Main(string[] args)
    {
        Console.WriteLine("Hi");
    }

    public static GameResult PlayTheGame(Player x, Player o, char[] board = null)
        => PlayTheGame(new(
                    x,
                    x,
                    o,
                    board ?? [nill, nill, nill, nill, nill, nill, nill, nill, nill]));

    private static GameResult PlayTheGame(Game game)
        => HasPlayerWon('x', game.Board) ? new('x', game.Board)
            : HasPlayerWon('o', game.Board) ? new('o', game.Board)
            : IsCatsGame(game.Board) ? new('c', game.Board)
            : PlayTheGame(new(
                        GetOpponent(game),
                        game.X,
                        game.O,
                        GetBoardWithNextMove(game)));

    private static Player GetOpponent(Game game)
        => game.CurrentPlayer == game.O
            ? game.X
            : game.O;

    private static char[] GetBoardWithNextMove(Game game)
    {
        var move = game.CurrentPlayer(game.Board);
        game.Board[move] = GetCurrentSymbol(game);
        return game.Board;
    }

    private static char GetCurrentSymbol(Game game)
        => game.CurrentPlayer == game.X
            ? 'x'
            : 'o';

    public static bool IsComplete(char[] board)
        => IsInvalid(board)
            || board
                .Where(IsFilled)
                .Distinct()
                .Where(p => HasPlayerWon(p, board))
                .Any()
            || IsCatsGame(board);

    public static bool HasPlayerWon(char player, char[] board)
        => IsValid(board)
            && Lines
                .Where(l => HasPlayerWon(player, board, l))
                .Any();

    public static bool IsCatsGame(char[] board)
        => IsValid(board)
            && board.All(IsFilled);

    private static bool HasPlayerWon(char player, char[] board, int[] line)
        => line
            .Select(i => board[i])
            .All(c => c == player);

    private static bool IsFilled(char cell)
        => cell != nill;

    private static bool IsInvalid(char[] board)
        => !IsValid(board);

    private static bool IsValid(char[] board)
        => board?.Length == 9;
}
