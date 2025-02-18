using Board = char[];
using Player = System.Func<char[], int>;

namespace TicTacToe;

public class Program
{
    private const char nill = ' ';
    public record GameResult(char Winner, Board Board);
    private record Game(Player CurrentPlayer, Player X, Player O, Board Board);
    private static Board BlankBoard = [nill, nill, nill, nill, nill, nill, nill, nill, nill];
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

    public static GameResult PlayTheGame(Player x, Player o, Board board = null)
        => PlayTheGame(new(
                    x,
                    x,
                    o,
                    board ?? BlankBoard));

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

    private static Board GetBoardWithNextMove(Game game)
    {
        var move = game.CurrentPlayer(game.Board);
        game.Board[move] = GetCurrentSymbol(game);
        return game.Board;
    }

    private static char GetCurrentSymbol(Game game)
        => game.CurrentPlayer == game.X
            ? 'x'
            : 'o';

    public static bool IsComplete(Board board)
        => IsInvalid(board)
            || board
                .Where(IsFilled)
                .Distinct()
                .Where(p => HasPlayerWon(p, board))
                .Any()
            || IsCatsGame(board);

    public static bool HasPlayerWon(char player, Board board)
        => IsValid(board)
            && Lines
                .Where(l => HasPlayerWon(player, board, l))
                .Any();

    public static bool IsCatsGame(Board board)
        => IsValid(board)
            && board.All(IsFilled);

    private static bool HasPlayerWon(char player, Board board, int[] line)
        => line
            .Select(i => board[i])
            .All(c => c == player);

    private static bool IsFilled(char cell)
        => cell != nill;

    private static bool IsInvalid(Board board)
        => !IsValid(board);

    private static bool IsValid(Board board)
        => board?.Length == 9;
}
