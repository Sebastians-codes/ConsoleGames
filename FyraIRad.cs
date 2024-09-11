namespace ConsoleGames;

public class FyraIRad
{
    private char[,] _field = new char[6, 6];
    private char[] _markers = ['X', 'O'];
    private int[] _scores = [0, 0, 0];
    private string[] _playerNames = new string[2];
    private int _turn = 0;
    private const int Delay = 25;

    public void Play()
    {
        GetPlayerNames();
        do
        {
            if (_scores[0] + _scores[1] + _scores[2] % 2 != 0 && _scores[0] + _scores[1] + _scores[2] > 0)
            {
                Array.Reverse(_playerNames);
                Array.Reverse(_markers);
            }
            else if (_scores[0] + _scores[1] + _scores[2] % 2 == 0 && _scores[0] + _scores[1] + _scores[2] > 0)
            {
                Array.Reverse(_playerNames);
                Array.Reverse(_markers);
            }

            while (true)
            {
                PrintField();
                int index = GetFieldIndex();
                char mark = (_turn % 2 == 0 ? _markers[0] : _markers[1]);

                AllocateFields(index, mark);

                if (CheckWin(mark))
                {
                    Console.WriteLine($"\n{_playerNames[_turn % 2 == 0 ? 1 : 0]} Vann!");
                    _scores[mark.Equals('X') ? 0 : 1]++;
                    ResetBoard();
                    Console.ReadKey();
                    break;
                }

                if (Draw())
                {
                    Console.WriteLine("Oavgjort");
                    _scores[2]++;
                    ResetBoard();
                    Console.ReadKey();
                    break;
                }
            }
        } while (true);
    }

    private void PrintScores()
    {
        if (_scores[0] + _scores[1] + _scores[2] <= 0) return;
        string player1 = _playerNames[Array.IndexOf(_markers, 'X')];
        string player2 = _playerNames[Array.IndexOf(_markers, 'O')];
        Console.WriteLine(
            $"{player1} Poäng: {_scores[0]}\n" +
            $"{player2} Poäng: {_scores[1]}\n" +
            $"Draws {_scores[2]}");
    }

    private void ResetBoard()
    {
        _field = new char[6, 6];
        _turn = 0;
    }

    private bool Draw()
    {
        for (var i = 0; i < _field.GetLength(0); i++)
            for (var j = 0; j < _field.GetLength(1); j++)
                if (_field[i, j].Equals('\0'))
                    return false;
        return true;
    }

    private bool CheckWin(char player)
    {
        // Check horizontal
        for (var i = 0; i < _field.GetLength(0); i++)
            for (var j = 0; j < _field.GetLength(1) - 3; j++)
                if (_field[i, j] == player && _field[i, j + 1] == player && _field[i, j + 2] == player &&
                    _field[i, j + 3] == player)
                {
                    return true;
                }

        // Check vertical
        for (var i = 0; i < _field.GetLength(0) - 3; i++)
            for (var j = 0; j < _field.GetLength(1); j++)
                if (_field[i, j] == player && _field[i + 1, j] == player && _field[i + 2, j] == player &&
                    _field[i + 3, j] == player)
                {
                    return true;
                }

        // Check diagonal (top-left to bottom-right)
        for (var i = 0; i < _field.GetLength(0) - 3; i++)
            for (var j = 0; j < _field.GetLength(1) - 3; j++)
                if (_field[i, j] == player && _field[i + 1, j + 1] == player && _field[i + 2, j + 2] == player &&
                    _field[i + 3, j + 3] == player)
                {
                    return true;
                }

        // Check diagonal (bottom-left to top-right)
        for (var i = 3; i < _field.GetLength(0); i++)
            for (var j = 0; j < _field.GetLength(1) - 3; j++)
                if (_field[i, j] == player && _field[i - 1, j + 1] == player && _field[i - 2, j + 2] == player &&
                    _field[i - 3, j + 3] == player)
                {
                    return true;
                }

        return false;
    }

    private void PrintField()
    {
        Console.Clear();
        PrintScores();
        Console.WriteLine(" 1  2  3  4  5  6");
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int j = 0; j < _field.GetLength(1); j++)
            {
                char ind = _field[j, i];
                Console.Write("[");
                if (ind == 'X')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (ind == 'O')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.Write(ind == '\0' ? " " : ind.ToString());
                Console.ResetColor();
                Console.Write("]");
            }

            Console.WriteLine();
        }

        Console.Write($"{(_turn % 2 == 0 ? _playerNames[0] : _playerNames[1])} Välj ett rör -> ");
    }

    // default value for char is \0
    private void AllocateFields(int index, char player)
    {
        if (!_field[index, 0].Equals('\0')) return;
        _field[index, 0] = player;
        PrintField();
        Thread.Sleep(Delay);
        for (int i = 0; i < _field.GetLength(0) - 1; i++)
        {
            if (_field[index, i + 1].Equals('\0'))
            {
                _field[index, i] = '\0';
                _field[index, i + 1] = player;
                PrintField();
                Thread.Sleep(Delay);
            }
            else break;
        }

        _turn++;
    }

    private int GetFieldIndex()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int num) && num is > 0 and < 7)
                return num - 1;
        }
    }

    private void GetPlayerNames()
    {
        for (var i = 0; i < 2; i++)
        {
            Console.Write($"Spelare {i + 1} namn -> ");
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                _playerNames[i] = $"{char.ToUpper(input.Trim()[0])}{input.Trim()[1..].ToLower()}";
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Felaktig input prova igen.");
                i--;
            }
        }
    }
}