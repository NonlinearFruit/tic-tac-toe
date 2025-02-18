namespace TicTacToe;

public class Program
{
    private const char nill = ' ';
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
