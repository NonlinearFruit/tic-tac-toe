<a href="https://github.com/NonlinearFruit/tic-tac-toe/actions?query=workflow%3Atest"><img alt="build" src="https://img.shields.io/github/actions/workflow/status/NonlinearFruit/tic-tac-toe/test.yml?branch=master"/></a>

# Requirements

Goal: Build a simple Tic Tac Toe program

- [x] Text-based UI
- [x] Play by the rules
- [x] Unbeatable bot
- [x] Have fun!

# Setup

NOTE: Requires .NET 9 ([docs](https://learn.microsoft.com/en-us/dotnet/core/install/))

```sh
git clone git@github.com:NonlinearFruit/tic-tac-toe.git
cd tic-tac-toe/
dotnet test
dotnet run --project TicTacToe
```

# How to Play

1. When the game launches, press enter to start
2. You are player `x` and you go first
2. Type the cell you want to play in and hit enter
    ```
           |   |
        ---+---+---
           |   |
        ---+---+---
           |   |

    Cell [1,2,3,4,5,6,7,8,9]:
    ```

NOTE: The cell numbers are as follows:
```
     1 | 2 | 3
    ---+---+---
     4 | 5 | 6
    ---+---+---
     7 | 8 | 9
```

NOTE: When the winner is `c` that is a draw or cat's game

# Related

- [NonlinearFruit/ultimate-tic-tac-toe](https://github.com/NonlinearFruit/ultimate-tic-tac-toe)
