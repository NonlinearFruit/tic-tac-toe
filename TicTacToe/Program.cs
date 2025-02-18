namespace TicTacToe;

public class Program
{
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

    public static bool IsComplete(string[] board)
    {
        if (IsInvalid(board)) return true;
        return board
            .Where(c => c is not null)
            .Distinct()
            .Where(p => HasPlayerWon(p, board))
            .Any() || IsCatsGame(board);
    }

    public static bool HasPlayerWon(string player, string[] board)
    {
        if (IsInvalid(board)) return false;
        return Lines
            .Where(l => HasPlayerWon(player, board, l))
            .Any();
    }

    public static bool IsCatsGame(string[] board)
    {
        if (IsInvalid(board)) return false;
        return board.Any() && board.All(c => c is not null);
    }

    private static bool HasPlayerWon(string player, string[] board, int[] line)
        => line
            .Select(i => board[i])
            .All(c => c == player);

    private static bool IsInvalid(string[] board)
        => board == null || board.Length != 9;
}
