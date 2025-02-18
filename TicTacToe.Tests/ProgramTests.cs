namespace TicTacToe.Tests;

public class ProgramTests
{
    private const char n = ' ';
    private const char x = 'x';
    private const char o = 'o';
    private static char[] DefaultBoard = default;
    private static char[] EmptyBoard = [];
    private static char[] MalformedBoard = [n];
    private static char[] BlankBoard = [n, n, n, n, n, n, n, n, n];
    private static char[] XBoard = [x, x, x, n, n, n, n, n, n];
    private static char[] OBoard = [o, o, o, n, n, n, n, n, n];
    private static char[] CatsBoard = [x, o, x, o, x, o, o, x, o];
    private static char[] PartialBoard = [x, o, x, n, n, n, n, n, n];

    public class PlayTheGame
    {
        private const char c = 'c';

        [Fact]
        public void determines_the_winner()
        {
            var xResult = Program.PlayTheGame(default, default, XBoard);
            Assert.Equal(x, xResult.Winner);
            Assert.Equal(XBoard, xResult.Board);

            var oResult = Program.PlayTheGame(default, default, OBoard);
            Assert.Equal(o, oResult.Winner);
            Assert.Equal(OBoard, oResult.Board);

            var cResult = Program.PlayTheGame(default, default, CatsBoard);
            Assert.Equal(c, cResult.Winner);
            Assert.Equal(CatsBoard, cResult.Board);
        }

        [Fact]
        public void allows_players_to_play()
        {
            var xPlayer = FakePlayer(0,1,2);
            var oPlayer = FakePlayer(3,7);

            var result = Program.PlayTheGame(xPlayer, oPlayer);

            Assert.Equal(x, result.Winner);
        }

        private Func<char, char[], int> FakePlayer(params int[] moves)
        {
            var m = moves.GetEnumerator();
            return (_, _) => { m.MoveNext(); return (int) m.Current; };
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

            Assert.Equal(Enumerable.Range(0,9), moves);
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

            Assert.True(moves.Count() < 9, "Too many moves");
            Assert.True(moves.Count() > 0, "Too few moves");
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
            Assert.True(Program.IsComplete([r, r, r, n, n, n, n, n, n]));
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
            Assert.False(Program.HasPlayerWon(x, DefaultBoard));
            Assert.False(Program.HasPlayerWon(x, EmptyBoard));
            Assert.False(Program.HasPlayerWon(x, MalformedBoard));
        }

        [Fact]
        public void blank_board_is_not_won()
        {
            Assert.False(Program.HasPlayerWon(x, BlankBoard));
        }

        [Fact]
        public void partial_board_is_not_won()
        {
            Assert.False(Program.HasPlayerWon(x, PartialBoard));
        }

        [Theory]
        [InlineData(x)]
        [InlineData(o)]
        public void finds_row_wins(char s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, s, s, n, n, n, n, n, n]));
            Assert.True(Program.HasPlayerWon(s, [n, n, n, s, s, s, n, n, n]));
            Assert.True(Program.HasPlayerWon(s, [n, n, n, n, n, n, s, s, s]));
        }

        [Theory]
        [InlineData(x)]
        [InlineData(o)]
        public void finds_column_wins(char s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, n, n, s, n, n, s, n, n]));
            Assert.True(Program.HasPlayerWon(s, [n, s, n, n, s, n, n, s, n]));
            Assert.True(Program.HasPlayerWon(s, [n, n, s, n, n, s, n, n, s]));
        }

        [Theory]
        [InlineData(x)]
        [InlineData(o)]
        public void finds_diagonal_wins(char s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, n, n, n, s, n, n, n, s]));
            Assert.True(Program.HasPlayerWon(s, [n, n, s, n, s, n, s, n, n]));
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
            var move = Program.RandomBot(x, PartialBoard);

            Assert.Contains(move, Program.AllPossibleMoves(PartialBoard));
        }
    }

    public class MinimaxBot
    {
        [Fact]
        public void chooses_a_valid_move()
        {
            var move = Program.MinimaxBot(x, PartialBoard);

            Assert.Contains(move, Program.AllPossibleMoves(PartialBoard));
        }

        [Fact]
        public void never_loses_to_random_bot()
        {
            var result = Program.PlayTheGame(Program.RandomBot, Program.MinimaxBot);

            Assert.NotEqual(x, result.Winner);
        }
    }
}
