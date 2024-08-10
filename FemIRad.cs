namespace ConsoleGames;

public class FemIRad {
    private static readonly char[] _field = [
        '1', '2', '3', '4', '5',
        '6', '7', '8', '9', '0',
        'q', 'w', 'e', 'r', 't',
        'a', 's', 'd', 'f', 'g',
        'z', 'x', 'c', 'v', 'b'
    ];

    private char[] _playField = (char[])_field.Clone();

    private static char[] _markers = ['O', 'X'];

    private const int Lives = 8;
    private int _turn = 0;
    private int[] _scores = [0, 0];

    private string[] _playerNames = new string[2];

    private Dictionary<char, List<char>> _played = new() {
        { _markers[0], [] },
        { _markers[1], [] }
    };

    public void Play() {
        GetPlayerNames();
        while (true) {
            Array.Reverse(_playerNames);
            Array.Reverse(_markers);
            Array.Reverse(_scores);
            while (true) {
                if (Winner(_turn % 2 != 0 ? _markers[0] : _markers[1])) {
                    SetPoints();
                    Console.WriteLine($"We have a winner! {_playerNames[(_turn % 2 != 0 ? 0 : 1)]}");
                    break;
                }

                char mark = GetMark();

                Turn(mark);

                _turn++;
            }

            _turn = 0;
            _playField = (char[])_field.Clone();
            _played[_markers[0]].Clear();
            _played[_markers[1]].Clear();
        }
    }

    private void PrintScores() {
        int index1 = Array.IndexOf(_markers, 'X');
        int index2 = Array.IndexOf(_markers, 'O');
        Console.WriteLine($"{_playerNames[index1]}: {_scores[index1]}. {_playerNames[index2]}: {_scores[index2]}.");
    }

    private void GetPlayerNames() {
        for (var i = 0; i < 2; i++) {
            Console.Write($"Spelare {i + 1} namn -> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) {
                i--;
                Console.Clear();
                Console.WriteLine("Felaktig input, försök igen.");
                continue;
            }

            input = TitleCaseAndTrim(input);
            if (i == 0)
                _playerNames[1] = input;
            if (i == 1)
                _playerNames[0] = input;
        }
    }

    static string TitleCaseAndTrim(string input) =>
        $"{char.ToUpper(input.Trim()[0])}{input.Trim()[1..].TrimStart().ToLower()}";

    private void SetPoints() =>
        _scores[(_turn % 2 != 0 ? 0 : 1)]++;

    private char GetMark() {
        char mark;
        Console.Clear();
        while (true) {
            PrintScores();
            PrintField();
            Console.Write($"{(_turn % 2 == 0 ? _playerNames[0] : _playerNames[1])} -> ");
            mark = Console.ReadKey(true).KeyChar;
            if (!_playField.Contains(mark)) {
                Console.Clear();
                Console.WriteLine("Invalid input");
            }
            else
                break;
        }

        return mark;
    }

    private bool Winner(char mark) {
        var win = false;
        for (var i = 0; i < _playField.Length; i += 5)
            if (_playField[i].Equals(mark))
                if (_playField[i].Equals(mark) && _playField[i + 1].Equals(mark) &&
                    _playField[i + 2].Equals(mark) && _playField[i + 3].Equals(mark) &&
                    _playField[i + 4].Equals(mark))
                    win = true;

        for (var i = 0; i < _playField.Length / 5; i++)
            if (_playField[i].Equals(mark))
                if (_playField[i].Equals(mark) && _playField[i + 5].Equals(mark) &&
                    _playField[i + 10].Equals(mark) && _playField[i + 15].Equals(mark) &&
                    _playField[i + 20].Equals(mark))
                    win = true;

        if (_playField[12].Equals(mark))
            if (_playField[0].Equals(mark) && _playField[6].Equals(mark) &&
                _playField[12].Equals(mark) && _playField[18].Equals(mark) &&
                _playField[24].Equals(mark) || _playField[4].Equals(mark) &&
                _playField[8].Equals(mark) && _playField[12].Equals(mark) &&
                _playField[16].Equals(mark) && _playField[20].Equals(mark))
                win = true;

        return win;
    }

    private void Turn(char fieldMark) {
        char mark = (_turn % 2 == 0 ? _markers[0] : _markers[1]);
        int indexOfFieldMark = Array.IndexOf(_field, fieldMark);
        ReturnFieldMark(mark);
        _played[mark].Add(_field[indexOfFieldMark]);
        _playField[indexOfFieldMark] = mark;
    }

    private void ReturnFieldMark(char mark) {
        if (_played[mark].Count < Lives) return;
        char playedMark = _played[mark][0];
        int index = Array.IndexOf(_field, playedMark);
        _playField[index] = playedMark;
        _played[mark].RemoveAt(0);
    }

    private void PrintField() {
        for (var i = 0; i < _playField.Length; i += 5) {
            for (var j = 0; j < 5; j++) {
                char mark = _playField[i + j];
                Console.ForegroundColor = GetColor();
                Console.Write("[");
                if (mark.Equals('X')) {
                    Console.ForegroundColor = GetColor("Red");
                    Console.Write(mark);
                }
                else if (mark.Equals('O')) {
                    Console.ForegroundColor = GetColor("Blue");
                    Console.Write(mark);
                }
                else
                    Console.Write(mark);

                Console.ForegroundColor = GetColor();
                Console.Write("]");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }

    ConsoleColor GetColor(string color = "white") =>
        color.Equals("Red") ? ConsoleColor.Red : color.Equals("Blue") ? ConsoleColor.Blue : ConsoleColor.White;
}