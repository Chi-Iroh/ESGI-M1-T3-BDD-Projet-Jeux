using GamesBase;
using Reqnroll;

namespace GamesLibrary;

public class TicTacToe : IGame
{
    private char[][] grid;

    private static readonly char[][] EMPTY_GRID = [
        [' ', ' ', ' '],
        [' ', ' ', ' '],
        [' ', ' ', ' ']
    ];

    private char player = ' ';

    public TicTacToe(char[][]? grid)
    {
        if (grid is null)
        {
            this.grid = EMPTY_GRID;
        } else
        {
            this.grid = grid;
        }
    }

    public IGame EmptyInit()
    {
        return new TicTacToe(null);
    }

    private static char[][] ReadState(Table table)
    {
        char[][] grid = EMPTY_GRID;

        if (table.RowCount != 3)
        {
            throw new Exception("Exactly 3 rows are needed to initialize Tic Tac Toe !");
        }

        int i = 0;
        foreach (DataTableRow row in table.Rows)
        {
            if (row.Count != 3)
            {
                throw new Exception("Exactly 3 columns are needed to initialize Tic Tac Toe !");
            }
            for (int j = 0; j < 3; j++)
            {
                if (row[j] == "")
                {
                    grid[i][j] = ' ';
                }
                else if (row[j].Count() == 1)
                {
                    grid[i][j] = row[j][0];
                }
                else
                {
                    throw new Exception($"Each cell must have only one character, got '{row[j]}' !");
                }
            }
            i++;
        }
        return grid;
    }

    public IGame InitFromState(Table table)
    {
        return new TicTacToe(ReadState(table));
    }

    public void SetPlayer(string player)
    {
        if (player != "x" && player != "o")
        {
            throw new Exception("Player must be o or x !");
        }
        this.player = player[0];
    }

    private void InvertPlayer()
    {
        if (this.player == ' ')
        {
            throw new Exception("Invalid player !");
        }
        this.player = (this.player == 'o') ? 'x' : 'o';
    }

    public void Play(string move)
    {
        var moves = new Dictionary<string, (int, int)>
        {
            { "at the top left", (0, 0) },
            { "at the bottom right", (2, 2) }
        };

        if (moves.ContainsKey(move))
        {
            (int, int) pos = moves[move];
            if (grid[pos.Item1][pos.Item2] != ' ')
            {
                throw new Exception("Cell not empty !");
            }
            grid[pos.Item1][pos.Item2] = player;
            this.InvertPlayer();
        } else
        {
            throw new Exception("Invalid move !");
        }
    }

    public bool CompareStateWith(Table table)
    {
        return grid == ReadState(table);
    }

    private enum WinnerEnum
    {
        O,
        X,
        Tie
    };

    private WinnerEnum? WinnerOf3(char[] cells)
    {
        if (cells.All(c => c == cells[0]) && cells[0] != ' ')
        {
            return cells[0] == 'o' ? WinnerEnum.O : WinnerEnum.X;
        }
        return null;
    }

    private WinnerEnum? _Winner()
    {
        WinnerEnum? winner = null;
        for (int i = 0; i < 3; i++)
        {
            winner = WinnerOf3(grid[i]);
            if (winner is not null)
            {
                return winner;
            }

            char[] col = [grid[0][i], grid[1][i], grid[2][i]];
            winner = WinnerOf3(col);
            if (winner is not null)
            {
                return winner;
            }
        }

        winner = WinnerOf3([grid[0][0], grid[1][1], grid[2][2]]);
        if (winner is not null)
        {
            return winner;
        }
        winner = WinnerOf3([grid[0][2], grid[1][1], grid[2][0]]);
        if (winner is not null)
        {
            return winner;
        }

        return grid.Any(row => row.Contains(' ')) ? null : WinnerEnum.Tie;
    }

    public bool Finished()
    {
        return _Winner() is not null;
    }

    public bool IsTie()
    {
        return _Winner() == WinnerEnum.Tie;
    }

    public string Winner()
    {
        switch (_Winner())
        {
            case null:
            case WinnerEnum.Tie:
                throw new Exception("No winner !");

            case WinnerEnum.O:
                return "o";

            case WinnerEnum.X:
                return "x";

            default:
                throw new Exception("The impossible happened !");
        }
    }
}
