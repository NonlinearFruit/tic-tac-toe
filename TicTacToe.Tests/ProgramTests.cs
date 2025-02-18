namespace TicTacToe.Tests;

public class ProgramTests
{
    private const char N = ' ';
    private const char X = 'x';
    private const char O = 'o';
    private static readonly char[] DefaultBoard = default;
    private static readonly char[] EmptyBoard = [];
    private static readonly char[] MalformedBoard = [N];
    private static readonly char[] BlankBoard = [N, N, N, N, N, N, N, N, N];
    private static readonly char[] XBoard = [X, X, X, N, N, N, N, N, N];
    private static readonly char[] OBoard = [O, O, O, N, N, N, N, N, N];
    private static readonly char[] CatsBoard = [X, O, X, O, X, O, O, X, O];
    private static readonly char[] PartialBoard = [X, O, X, N, N, N, N, N, N];

    public class PlayTheGame
    {
        private const char C = 'c';

        [Fact]
        public void determines_the_winner()
        {
            var xResult = Program.PlayTheGame(default, default, XBoard);
            Assert.Equal(X, xResult.Winner);
            Assert.Equal(XBoard, xResult.Board);

            var oResult = Program.PlayTheGame(default, default, OBoard);
            Assert.Equal(O, oResult.Winner);
            Assert.Equal(OBoard, oResult.Board);

            var cResult = Program.PlayTheGame(default, default, CatsBoard);
            Assert.Equal(C, cResult.Winner);
            Assert.Equal(CatsBoard, cResult.Board);
        }

        [Fact]
        public void allows_players_to_play()
        {
            var xPlayer = FakePlayer(0, 1, 2);
            var oPlayer = FakePlayer(3, 7);

            var result = Program.PlayTheGame(xPlayer, oPlayer);

            Assert.Equal(X, result.Winner);
        }

        private Func<char, char[], int> FakePlayer(params int[] moves)
        {
            var m = moves.GetEnumerator();
            return (_, _) => { m.MoveNext(); return (int)m.Current!; };
        }
    }

    public class AllPossibleMoves
    {
        [Fact]
        public void degenerate_boards_have_no_possible_moves()
        {
            Assert.Empty(Program.AllPossibleMoves(DefaultBoard));
            Assert.Empty(Program.AllPossibleMoves(EmptyBoard));
        }

        [Fact]
        public void blank_board_has_all_possible_moves()
        {
            var moves = Program.AllPossibleMoves(BlankBoard);

            Assert.Equal(Enumerable.Range(0, 9), moves);
        }

        [Fact]
        public void cats_board_has_no_possible_moves()
        {
            var moves = Program.AllPossibleMoves(CatsBoard);

            Assert.Empty(moves);
        }

        [Fact]
        public void partially_filled_board_some_possible_moves()
        {
            var moves = Program.AllPossibleMoves(PartialBoard);

            Assert.True(moves.Count < 9, "Too many moves");
            Assert.True(moves.Count > 0, "Too few moves");
        }
    }

    public class IsComplete
    {
        [Fact]
        public void degenerate_boards_are_complete()
        {
            Assert.True(Program.IsComplete(DefaultBoard));
            Assert.True(Program.IsComplete(EmptyBoard));
            Assert.True(Program.IsComplete(MalformedBoard));
        }

        [Fact]
        public void blank_board_is_not_complete()
        {
            Assert.False(Program.IsComplete(BlankBoard));
        }

        [Fact]
        public void board_is_complete_when_x_wins()
        {
            Assert.True(Program.IsComplete(XBoard));
        }

        [Fact]
        public void board_is_complete_when_o_wins()
        {
            Assert.True(Program.IsComplete(OBoard));
        }

        [Fact]
        public void board_is_complete_when_random_symbol_wins()
        {
            var r = 'r';
            Assert.True(Program.IsComplete([r, r, r, N, N, N, N, N, N]));
        }

        [Fact]
        public void board_is_complete_when_cat_wins()
        {
            Assert.True(Program.IsComplete(CatsBoard));
        }

        [Fact]
        public void board_is_not_complete_when_partially_played()
        {
            Assert.False(Program.IsComplete(PartialBoard));
        }
    }

    public class HasPlayerWon
    {
        [Fact]
        public void degenerate_boards_are_not_won()
        {
            Assert.False(Program.HasPlayerWon(X, DefaultBoard));
            Assert.False(Program.HasPlayerWon(X, EmptyBoard));
            Assert.False(Program.HasPlayerWon(X, MalformedBoard));
        }

        [Fact]
        public void blank_board_is_not_won()
        {
            Assert.False(Program.HasPlayerWon(X, BlankBoard));
        }

        [Fact]
        public void partial_board_is_not_won()
        {
            Assert.False(Program.HasPlayerWon(X, PartialBoard));
        }

        [Theory]
        [InlineData(X)]
        [InlineData(O)]
        public void finds_row_wins(char s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, s, s, N, N, N, N, N, N]));
            Assert.True(Program.HasPlayerWon(s, [N, N, N, s, s, s, N, N, N]));
            Assert.True(Program.HasPlayerWon(s, [N, N, N, N, N, N, s, s, s]));
        }

        [Theory]
        [InlineData(X)]
        [InlineData(O)]
        public void finds_column_wins(char s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, N, N, s, N, N, s, N, N]));
            Assert.True(Program.HasPlayerWon(s, [N, s, N, N, s, N, N, s, N]));
            Assert.True(Program.HasPlayerWon(s, [N, N, s, N, N, s, N, N, s]));
        }

        [Theory]
        [InlineData(X)]
        [InlineData(O)]
        public void finds_diagonal_wins(char s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, N, N, N, s, N, N, N, s]));
            Assert.True(Program.HasPlayerWon(s, [N, N, s, N, s, N, s, N, N]));
        }
    }

    public class IsCatsGame
    {
        [Fact]
        public void degenerate_boards_are_not_cats_game()
        {
            Assert.False(Program.IsCatsGame(DefaultBoard));
            Assert.False(Program.IsCatsGame(EmptyBoard));
            Assert.False(Program.IsCatsGame(MalformedBoard));
        }

        [Fact]
        public void blank_board_is_not_cats_game()
        {
            Assert.False(Program.IsCatsGame(BlankBoard));
        }

        [Fact]
        public void board_is_not_cats_game_when_partially_played()
        {
            Assert.False(Program.IsCatsGame(PartialBoard));
        }

        [Fact]
        public void drawn_board_is_cats_game()
        {
            Assert.True(Program.IsCatsGame(CatsBoard));
        }
    }

    public class IsValidInput
    {
        [Fact]
        public void bad_input_is_not_valid()
        {
            Assert.False(Program.IsValidInput("", [1]));
            Assert.False(Program.IsValidInput("letters", [1]));
            Assert.False(Program.IsValidInput("4", [1]));
        }

        [Fact]
        public void input_one_larger_than_a_possible_move_is_valid()
        {
            Assert.True(Program.IsValidInput("2", [1]));
        }
    }

    public class RandomBot
    {
        [Fact]
        public void chooses_a_valid_move()
        {
            var move = Program.RandomBot(X, PartialBoard);

            Assert.Contains(move, Program.AllPossibleMoves(PartialBoard));
        }
    }

    public class MinimaxBot
    {
        [Fact]
        public void chooses_a_valid_move()
        {
            var move = Program.MinimaxBot(X, PartialBoard);

            Assert.Contains(move, Program.AllPossibleMoves(PartialBoard));
        }

        [Fact]
        public void never_loses_to_random_bot()
        {
            var result = Program.PlayTheGame(Program.RandomBot, Program.MinimaxBot);

            Assert.NotEqual(X, result.Winner);
        }
    }
}
