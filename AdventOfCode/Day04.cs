using MiscUtil.IO;

namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string _input;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        (int, int)[] directions = [(0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (-1, 1), (1, -1), (-1, -1)];
        char[] searchTerm = ['X', 'M', 'A', 'S'];
        List<string> lines = [];

        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            lines.Add(line);
        }

        var total = 0;
        foreach (var i in Enumerable.Range(0, lines.Count))
        {
            foreach (var j in Enumerable.Range(0, lines[0].Length))
            {
                total += NumMatches(i, j);
            }
        }

        int NumMatches(int i, int j)
        {
            var matches = 0;
            foreach (var direction in directions)
            {
                matches += DirectionMatches(i, j, 0, direction) ? 1 : 0;
            }
            return matches;
        }

        bool DirectionMatches(int i, int j, int count, (int, int) direction)
        {
            if (count > 3)
                return true;

            if (i >= lines.Count || j >= lines[0].Length || i < 0 || j < 0)
                return false;

            if (lines[i][j] != searchTerm[count])
                return false;

            return DirectionMatches(i + direction.Item1, j + direction.Item2, count + 1, direction);
        }
        
        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> lines = [];

        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            lines.Add(line);
        }

        var total = 0;
        foreach (var i in Enumerable.Range(0, lines.Count))
        {
            foreach (var j in Enumerable.Range(0, lines[0].Length))
            {
                total += IsX(i, j) ? 1 : 0;
                if (IsX(i, j))
                    Console.WriteLine($"{i}, {j}");
            }
        }
        
        bool IsX(int i, int j)
        {
            if (lines[i][j] != 'A')
                return false;

            if (i - 1 < 0 || i + 1 >= lines.Count || j - 1 < 0 || j + 1 >= lines[0].Length)
                return false;

            var topLeft = lines[i - 1][j - 1];
            var topRight = lines[i - 1][j + 1];
            var bottomLeft = lines[i + 1][j - 1];
            var bottomRight = lines[i + 1][j + 1];

            return (topLeft == 'M' && bottomRight == 'S' || topLeft == 'S' && bottomRight == 'M') &&
                   (topRight == 'M' && bottomLeft == 'S' || topRight == 'S' && bottomLeft == 'M');
        }
        
        return new ValueTask<string>($"{total}");
    }
}
