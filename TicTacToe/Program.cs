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
    {
        if (IsInvalid(board)) return true;
        return board
            .Where(c => c != nill)
            .Distinct()
            .Where(p => HasPlayerWon(p, board))
            .Any() || IsCatsGame(board);
    }

    public static bool HasPlayerWon(char player, char[] board)
    {
        if (IsInvalid(board)) return false;
        return Lines
            .Where(l => HasPlayerWon(player, board, l))
            .Any();
    }

    public static bool IsCatsGame(char[] board)
    {
        if (IsInvalid(board)) return false;
        return board.Any() && board.All(c => c != nill);
    }

    private static bool HasPlayerWon(char player, char[] board, int[] line)
        => line
            .Select(i => board[i])
            .All(c => c == player);

    private static bool IsInvalid(char[] board)
        => board == null || board.Length != 9;
}
