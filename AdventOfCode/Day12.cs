using MiscUtil.IO;

namespace AdventOfCode;

public class Day12 : BaseDay
{
    private readonly string _input;

    public Day12()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<string> lines = new();
        HashSet<char> tokens = new();
        Dictionary<char, List<List<(int, int)>>> tokenLocations = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            lines.Add(line);
            tokens.UnionWith(line);
        }
        var height = lines.Count;
        var width = lines[0].Length;

        var i = 0;
        foreach (var line in lines)
        {
            var j = 0;

            foreach (var token in line)
            {
                var groups = tokenLocations.GetValueOrDefault(token, []);

                if (!groups.Any(g => g.Contains((i, j)))) groups.Add(GetConnected((i, j)));

                tokenLocations[token] = groups;
                j++;
            }

            i++;
        }

        var total = 0;
        foreach (var token in tokens)
        foreach (var group in tokenLocations[token])
        {
            var perimiter = 0;
            foreach (var location in group) perimiter += NumAround(location, token);

            total += perimiter * group.Count;
        }

        return new ValueTask<string>($"{total}");

        List<(int, int)> GetConnected((int, int) position)
        {
            var token = lines[position.Item1][position.Item2];
            List<(int, int)> ret = [position];
            List<(int, int)> directions = [(0, 1), (0, -1), (1, 0), (-1, 0)];
            var next = directions.Select(d => Add(d, position)).Where(p =>
                p.Item1 >= 0 && p.Item1 < height && p.Item2 >= 0 &&
                p.Item2 < width).Where(p => lines[p.Item1][p.Item2] == token).ToList();

            while (next.Count != 0)
            {
                var current = next[0];
                next.RemoveAt(0);
                var potentialNext = directions.Select(d => Add(d, current)).Where(p =>
                    p.Item1 >= 0 && p.Item1 < height && p.Item2 >= 0 &&
                    p.Item2 < width).Where(p => lines[p.Item1][p.Item2] == token).Where(p => !ret.Contains(p));

                next = next.Concat(potentialNext).Distinct().ToList();

                ret.Add(current);
            }

            return ret.Distinct().ToList();
        }

        (int, int) Add((int, int) a, (int, int) b)
        {
            return (a.Item1 + b.Item1, a.Item2 + b.Item2);
        }

        int NumAround((int, int) position, char token)
        {
            List<(int, int)> directions = [(0, 1), (0, -1), (1, 0), (-1, 0)];
            var total = 0;
            foreach (var direction in directions)
            {
                var nextPosition = (position.Item1 + direction.Item1, position.Item2 + direction.Item2);

                if (nextPosition.Item1 >= 0 && nextPosition.Item1 < height && nextPosition.Item2 >= 0 &&
                    nextPosition.Item2 < width)
                {
                    if (lines[nextPosition.Item1][nextPosition.Item2] != token)
                        total += 1;
                }
                else
                {
                    total += 1;
                }
            }
            
            return total;
        }
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> lines = new();
        HashSet<char> tokens = new();
        Dictionary<char, List<List<(int, int)>>> tokenLocations = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            lines.Add(line);
            tokens.UnionWith(line);
        }
        var height = lines.Count;
        var width = lines[0].Length;

        var i = 0;
        foreach (var line in lines)
        {
            var j = 0;

            foreach (var token in line)
            {
                var groups = tokenLocations.GetValueOrDefault(token, []);

                if (!groups.Any(g => g.Contains((i, j)))) groups.Add(GetConnected((i, j)));

                tokenLocations[token] = groups;
                j++;
            }

            i++;
        }

        var total = 0;
        foreach (var token in tokens)
        foreach (var group in tokenLocations[token])
        {
            var numSides = NumSides(group);
            total += numSides * group.Count;
        }

        int NumSides(List<(int, int)> group)
        {
            var total = 0;
            var leftSides = group.Select(p => Add(p, (0, -1))).Where(p => !group.Contains(p));
            var rightSides = group.Select(p => Add(p, (0, 1))).Where(p => !group.Contains(p));
            var topSides = group.Select(p => Add(p, (-1, 0))).Where(p => !group.Contains(p));
            var bottomSides = group.Select(p => Add(p, (1, 0))).Where(p => !group.Contains(p));

            return CountVertical(leftSides) + CountVertical(rightSides) + CountHorizontal(topSides) +
                   CountHorizontal(bottomSides);
            
            int CountVertical(IEnumerable<(int, int)> vertical)
            {
                var current = (-999, -999);
                var total = 0;
                foreach (var pos in vertical.OrderBy(item => item.Item2).ThenBy(item => item.Item1))
                {
                    if (pos.Item2 != current.Item2 || Math.Abs(pos.Item1 - current.Item1) > 1)
                    {
                        total += 1;
                    }

                    current = pos;
                }
                
                return total;
            }
            
            int CountHorizontal(IEnumerable<(int, int)> horizontal)
            {
                var current = (-999, -999);
                var total = 0;
                foreach (var pos in horizontal.OrderBy(item => item.Item1).ThenBy(item => item.Item2))
                {
                    if (pos.Item1 != current.Item1 || Math.Abs(pos.Item2 - current.Item2) > 1)
                    {
                        total += 1;
                    }

                    current = pos;
                }

                return total;
            }
            

        }
        
        return new ValueTask<string>($"{total}");

        List<(int, int)> GetConnected((int, int) position)
        {
            var token = lines[position.Item1][position.Item2];
            List<(int, int)> ret = [position];
            List<(int, int)> directions = [(0, 1), (0, -1), (1, 0), (-1, 0)];
            var next = directions.Select(d => Add(d, position)).Where(p =>
                p.Item1 >= 0 && p.Item1 < height && p.Item2 >= 0 &&
                p.Item2 < width).Where(p => lines[p.Item1][p.Item2] == token).ToList();

            while (next.Count != 0)
            {
                var current = next[0];
                next.RemoveAt(0);
                var potentialNext = directions.Select(d => Add(d, current)).Where(p =>
                    p.Item1 >= 0 && p.Item1 < height && p.Item2 >= 0 &&
                    p.Item2 < width).Where(p => lines[p.Item1][p.Item2] == token).Where(p => !ret.Contains(p));

                next = next.Concat(potentialNext).Distinct().ToList();

                ret.Add(current);
            }

            return ret.Distinct().ToList();
        }

        (int, int) Add((int, int) a, (int, int) b)
        {
            return (a.Item1 + b.Item1, a.Item2 + b.Item2);
        }
    }
}