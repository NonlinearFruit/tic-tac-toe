using System.Collections.Concurrent;
using Board = char[];
using Player = System.Func<char, char[], int>;

namespace TicTacToe;

public static class Program
{
    private const char nil = ' ';
    public record GameResult(char Winner, Board Board);
    private record Game(char CurrentPlayer, Player X, Player O, Board Board);
    private static Board BlankBoard = [nil, nil, nil, nil, nil, nil, nil, nil, nil];
    private static IEnumerable<int[]> Lines = new Dictionary<string, int[][]>
    {
        ["rows"] = [[0, 1, 2], [3, 4, 5], [6, 7, 8]],
        ["columns"] = [[0, 3, 6], [1, 4, 7], [2, 5, 8]],
        ["diagonals"] = [[0, 4, 8], [2, 4, 6]]
    }.Values.SelectMany(v => v);

    public static void Main(string[] args)
    {
        Console.WriteLine("Tic Tac Toe");
        Console.WriteLine(" <<press enter to start>>");
        Console.ReadLine();
        Console.Clear();
        var result = PlayTheGame(HumanPlayer, MinimaxBot);
        Console.Clear();
        Print(result.Board);
        Console.WriteLine($"Winner: {result.Winner}");
    }

    public static GameResult PlayTheGame(Player x, Player o, Board board = null)
        => PlayTheGame(new(
                    'x',
                    x,
                    o,
                    board ?? BlankBoard));

    private static GameResult PlayTheGame(Game game)
        => HasPlayerWon('x', game.Board) ? new('x', game.Board)
            : HasPlayerWon('o', game.Board) ? new('o', game.Board)
            : IsCatsGame(game.Board) ? new('c', game.Board)
            : PlayTheGame(new(
                        GetOpponent(game.CurrentPlayer),
                        game.X,
                        game.O,
                        GetBoardWithNextMove(game)));

    private static char GetOpponent(char symbol)
        => symbol == 'x'
            ? 'o'
            : 'x';

    private static Board GetBoardWithNextMove(Game game)
    {
        var move = GetNextMove(game);
        var newBoard = (Board) game.Board.Clone();
        newBoard[move] = game.CurrentPlayer;
        return newBoard;
    }

    private static int GetNextMove(Game game)
        => game.CurrentPlayer == 'x'
            ? game.X(game.CurrentPlayer, game.Board)
            : game.O(game.CurrentPlayer, game.Board);

    public static IEnumerable<int> AllPossibleMoves(Board board)
    {
        if (IsInvalid(board)) return Array.Empty<int>();
        return board
            .Select((c,i) => new { Index = i, Cell = c })
            .Where(o => o.Cell == nil)
            .Select(o => o.Index);
    }

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

    public static int RandomBot(char mySymbol, Board board)
    {
        var moves = AllPossibleMoves(board);
        return moves.ElementAt(Random.Shared.Next(moves.Count()));
    }

    public static int MinimaxBot(char mySymbol, Board board)
    {
        var moves = AllPossibleMoves(board);
        var opponent = GetOpponent(mySymbol);
        var orderedEnumerable = moves
            .Select(m => new
            {
                Move = m,
                Score = ScoreTheMove(mySymbol, board, m)
            })
            .OrderByDescending(o => o.Score)
            .ToList();
        return orderedEnumerable
            .Select(o => o.Move)
            .First();
    }

    private static int ScoreTheMove(char mySymbol, Board board, int move)
    {
        var opponent = GetOpponent(mySymbol);
        var newBoard = (Board) board.Clone();
        newBoard[move] = mySymbol;
        var result = PlayTheGame(new(opponent, MinimaxBot, MinimaxBot, newBoard));
        return result.Winner == mySymbol ? 1
            : result.Winner == opponent ? -1
            : 0;
    }

    private static int HumanPlayer(char mySymbol, Board board)
    {
        var possibleMoves = AllPossibleMoves(board);
        Console.Clear();
        Print(board);
        var input = PromptUser(possibleMoves);
        while (!IsValidInput(input, possibleMoves))
            input = PromptUser(possibleMoves);
        return int.Parse(input) - 1;
    }

    private static void Print(Board b)
        => Console.WriteLine($"""

                 {b[0]} | {b[1]} | {b[2]}
                ---+---+---
                 {b[3]} | {b[4]} | {b[5]}
                ---+---+---
                 {b[6]} | {b[7]} | {b[8]}

            """);

    private static string PromptUser(IEnumerable<int> possibleMoves)
    {
        Console.Write($"Cell [{string.Join(",", possibleMoves.Select(m => m + 1))}]: ");
        return Console.ReadLine();
    }

    public static bool IsValidInput(string input, IEnumerable<int> possibleMoves)
    {
        return int.TryParse(input, out var move) && possibleMoves.Contains(move-1);
    }

    private static bool HasPlayerWon(char player, Board board, int[] line)
        => line
            .Select(i => board[i])
            .All(c => c == player);

    private static bool IsFilled(char cell)
        => cell != nil;

    private static bool IsInvalid(Board board)
        => !IsValid(board);

    private static bool IsValid(Board board)
        => board?.Length == 9;
}
