namespace TicTacToe.Tests;

public class ProgramTests
{
    private const string n = null;
    private const string x = "x";
    private const string o = "o";
    private static string[] DefaultBoard = default;
    private static string[] EmptyBoard = [];
    private static string[] MalformedBoard = [n];
    private static string[] BlankBoard = [n, n, n, n, n, n, n, n, n];
    private static string[] XBoard = [x, x, x, n, n, n, n, n, n];
    private static string[] OBoard = [o, o, o, n, n, n, n, n, n];
    private static string[] CatsBoard = [x, o, x, o, x, o, o, x, o];
    private static string[] PartialBoard = [x, o, x, n, n, n, n, n, n];

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
            var r = "random symbol";
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
        public void finds_row_wins(string s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, s, s, n, n, n, n, n, n]));
            Assert.True(Program.HasPlayerWon(s, [n, n, n, s, s, s, n, n, n]));
            Assert.True(Program.HasPlayerWon(s, [n, n, n, n, n, n, s, s, s]));
        }

        [Theory]
        [InlineData(x)] 
        [InlineData(o)] 
        public void finds_column_wins(string s)
        {
            Assert.True(Program.HasPlayerWon(s, [s, n, n, s, n, n, s, n, n]));
            Assert.True(Program.HasPlayerWon(s, [n, s, n, n, s, n, n, s, n]));
            Assert.True(Program.HasPlayerWon(s, [n, n, s, n, n, s, n, n, s]));
        }

        [Theory]
        [InlineData(x)] 
        [InlineData(o)] 
        public void finds_diagonal_wins(string s)
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
}
