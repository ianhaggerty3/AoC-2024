using MiscUtil.IO;

namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly string _input;

    public Day10()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<(int, int), int> map = new();
        int i = 0;
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            int j = 0;
            foreach (var token in line)
            {
                map[(i, j)] = int.Parse([token]);
                j++;
            }

            i++;
        }

        var startingPositions = map.Where(p => p.Value == 0);

        var total = 0;
        foreach (var startingPosition in startingPositions) total += GetScore(startingPosition.Key).Count;

        HashSet<(int, int)> GetScore((int, int) position)
        {
            var currentPosition = position;
            while (true)
            {
                var currentValue = map[currentPosition];

                if (currentValue == 9)
                    return [currentPosition];

                var next = currentValue + 1;
                var nextPositions = map.Where(p => p.Value == next && Diff(p.Key, currentPosition) == 1).ToList();

                if (nextPositions.Count == 0) return [];
                if (nextPositions.Count == 1)
                    currentPosition = nextPositions[0].Key;
                else if (nextPositions.Count > 1)
                    return nextPositions.Select(pos => GetScore(pos.Key)).SelectMany(p => p).ToHashSet();
            }
        }

        int Diff((int, int) a, (int, int) b)
        {
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
        }
        
        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<(int, int), int> map = new();
        int i = 0;
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            int j = 0;
            foreach (var token in line)
            {
                map[(i, j)] = int.Parse([token]);
                j++;
            }

            i++;
        }

        var startingPositions = map.Where(p => p.Value == 0);

        var total = 0;
        foreach (var startingPosition in startingPositions)
        {
            total += GetScore(startingPosition.Key);
        }

        int GetScore((int, int) position)
        {
            var currentPosition = position;
            while (true)
            {
                var currentValue = map[currentPosition];

                if (currentValue == 9)
                    return 1;
                
                var next = currentValue + 1;
                var nextPositions = map.Where(p => p.Value == next && (Diff(p.Key, currentPosition) == 1)).ToList();

                if (nextPositions.Count == 0)
                {
                    return 0;
                }
                if (nextPositions.Count == 1)
                {
                    currentPosition = nextPositions[0].Key;
                }
                else if (nextPositions.Count > 1)
                {
                    return nextPositions.Sum(pos => GetScore(pos.Key));
                }
            }
        }

        int Diff((int, int) a, (int, int) b)
        {
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
        }
        
        return new ValueTask<string>($"{total}");
    }
}
