using MiscUtil.IO;

namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly string _input;

    public Day08()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<char, List<(int, int)>> antennasTypes = new();
        HashSet<(int, int)> antinodes = new();

        var i = 0;
        var width = 0;
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var j = 0;
            foreach (var token in line)
            {
                if (token != '.')
                    antennasTypes[token] = antennasTypes.GetValueOrDefault(token, new List<(int, int)>()).Append((i, j))
                        .ToList();
                j++;
            }

            width = j;
            i++;
        }

        var height = i;

        foreach (var antennaType in antennasTypes)
        {
            i = 0;
            foreach (var first in antennaType.Value)
            {
                var j = i + 1;
                foreach (var second in antennaType.Value[j..])
                {
                    foreach (var antinode in GetAninodes(first, second))
                        if (antinode.Item1 >= 0 && antinode.Item1 < height && antinode.Item2 >= 0 &&
                            antinode.Item2 < width)
                            antinodes.Add(antinode);

                    j++;
                }

                i++;
            }
        }

        List<(int, int)> GetAninodes((int, int) first, (int, int) second)
        {
            var diff = (first.Item1 - second.Item1, first.Item2 - second.Item2);
            var firstNode = (first.Item1 + diff.Item1, first.Item2 + diff.Item2);
            var secondNode = (second.Item1 - diff.Item1, second.Item2 - diff.Item2);
            return [firstNode, secondNode];
        }

        return new ValueTask<string>($"{antinodes.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<char, List<(int, int)>> antennasTypes = new();
        HashSet<(int, int)> antinodes = new();

        var i = 0;
        var width = 0;
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var j = 0;
            foreach (var token in line)
            {
                if (token != '.')
                    antennasTypes[token] = antennasTypes.GetValueOrDefault(token, new List<(int, int)>()).Append((i, j))
                        .ToList();
                j++;
            }

            width = j;
            i++;
        }

        var height = i;

        foreach (var antennaType in antennasTypes)
        {
            i = 0;
            foreach (var first in antennaType.Value)
            {
                var j = i + 1;
                foreach (var second in antennaType.Value[j..])
                {
                    foreach (var antinode in GetAninodes(first, second))

                        antinodes.Add(antinode);

                    j++;
                }

                i++;
            }
        }

        List<(int, int)> GetAninodes((int, int) first, (int, int) second)
        {
            var diff = (first.Item1 - second.Item1, first.Item2 - second.Item2);
            var ret = new List<(int, int)>();

            var current = first;
            while (IsInRage(current))
            {
                ret.Add(current);
                current = (current.Item1 + diff.Item1, current.Item2 + diff.Item2);
            }
            
            current = second;
            while (IsInRage(current))
            {
                ret.Add(current);
                current = (current.Item1 - diff.Item1, current.Item2 - diff.Item2);
            }
            
            return ret;
        }

        bool IsInRage((int, int) antinode)
        {
            return antinode.Item1 >= 0 && antinode.Item1 < height && antinode.Item2 >= 0 &&
                   antinode.Item2 < width;
        }

        return new ValueTask<string>($"{antinodes.Count}");
    }
}