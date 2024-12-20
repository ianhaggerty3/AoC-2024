using MiscUtil.IO;

namespace AdventOfCode;

public class Day19 : BaseDay
{
    private readonly string _input;

    public Day19()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        var towels = lines[0].Split(", ").OrderBy(t => t.Length).Reverse().ToList();
        var designs = lines[2..];

        var total = designs.Count(IsPossible);

        return new ValueTask<string>($"{total}");

        bool IsPossible(string design)
        {
            HashSet<int> invalidPositions = [];

            var validTowels = towels.Where(design.Contains).ToList();
            return IsPossibleRecursive(design, 0, validTowels);


            bool IsPossibleRecursive(string design, int position, List<string> validTowels)
            {
                if (position >= design.Length)
                    return true;

                if (invalidPositions.Contains(position))
                    return false;

                foreach (var towel in validTowels)
                    if (design[position..].StartsWith(towel) &&
                        IsPossibleRecursive(design, position + towel.Length, validTowels))
                        return true;

                invalidPositions.Add(position);

                return false;
            }
        }
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        var towels = lines[0].Split(", ").OrderBy(t => t.Length).Reverse().ToList();
        var designs = lines[2..];

        long total = designs.Sum(CountPossible);

        return new ValueTask<string>($"{total}");

        long CountPossible(string design)
        {
            HashSet<int> invalidPositions = [];
            Dictionary<int, long> validPositions = [];

            var validTowels = towels.Where(design.Contains).ToList();
            return CountPossibleRecursive(design, 0, validTowels);

            long CountPossibleRecursive(string design, int position, List<string> validTowels)
            {
                if (position >= design.Length)
                    return 1;

                if (invalidPositions.Contains(position))
                    return 0;

                if (validPositions.ContainsKey(position))
                    return validPositions[position];

                long total = 0;
                foreach (var towel in validTowels)
                    if (design[position..].StartsWith(towel))
                        total += CountPossibleRecursive(design, position + towel.Length, validTowels);


                if (total == 0)
                    invalidPositions.Add(position);
                else
                    validPositions.Add(position, total);

                return total;
            }
        }
    }
}