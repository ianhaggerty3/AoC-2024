using System.Text.RegularExpressions;
using MiscUtil.IO;

namespace AdventOfCode;

public class Day14 : BaseDay
{
    private readonly string _input;

    public Day14()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<Robot> robots = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var match = Regex.Matches(line, @"p\=(-*\d+),(-*\d+) v\=(-*\d+),(-*\d+)").Single();

            robots.Add(new Robot((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))));
        }

        var width = 101;
        var height = 103;

        List<int> quadrantCounts = [0, 0, 0, 0];
        foreach (var robot in robots)
        {
            var finalX = robot.P.X + robot.V.X * 100;
            var finalRelativeX = finalX % width;
            if (finalRelativeX < 0)
                finalRelativeX += width;

            var finalY = robot.P.Y + robot.V.Y * 100;
            var finalRelativeY = finalY % height;
            if (finalRelativeY < 0)
                finalRelativeY += height;

            if (finalRelativeX < width / 2)
            {
                if (finalRelativeY < height / 2)
                    quadrantCounts[0] += 1;
                else if (finalRelativeY > height / 2) quadrantCounts[1] += 1;
            }
            else if (finalRelativeX > width / 2)
            {
                if (finalRelativeY < height / 2)
                    quadrantCounts[2] += 1;
                else if (finalRelativeY > height / 2) quadrantCounts[3] += 1;
            }
        }

        var total = quadrantCounts[0] * quadrantCounts[1] * quadrantCounts[2] * quadrantCounts[3];


        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<Robot> robots = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var match = Regex.Matches(line, @"p\=(-*\d+),(-*\d+) v\=(-*\d+),(-*\d+)").Single();

            robots.Add(new Robot((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))));
        }

        var width = 101;
        var height = 103;

        var i = 1;
        while (true)
        {
            HashSet<(int X, int Y)> positions = new();

            foreach (var robot in robots) positions.Add(GetRelativePosAtTime(i, robot));

            if (positions.Count(p => HasNearby(p, positions)) > 300)
            {
                Console.WriteLine($"i = {i}");
                PrintRobots(positions);
                break;
            }

            i++;
        }

        bool HasNearby((int X, int Y) pos, HashSet<(int X, int Y)> positions)
        {
            List<(int X, int Y)> diffs = [(0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1)];

            if (diffs.Select(diff => (pos.X + diff.X, pos.Y + diff.Y)).Any(positions.Contains))
                return true;

            return false;
        }

        void PrintRobots(HashSet<(int X, int Y)> positions)
        {
            var y = 0;
            while (y < height)
            {
                var x = 0;
                var lineString = "";
                while (x < width)
                {
                    lineString += positions.Contains((x, y)) ? 'X' : '.';
                    x++;
                }

                Console.WriteLine(lineString);

                y++;
            }

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        (int, int) GetRelativePosAtTime(int t, Robot robot)
        {
            var finalX = robot.P.X + robot.V.X * t;
            var finalRelativeX = finalX % width;
            if (finalRelativeX < 0)
                finalRelativeX += width;

            var finalY = robot.P.Y + robot.V.Y * t;
            var finalRelativeY = finalY % height;
            if (finalRelativeY < 0)
                finalRelativeY += height;

            return (finalRelativeX, finalRelativeY);
        }


        return new ValueTask<string>($"{0}");
    }

    internal record Robot((int X, int Y) P, (int X, int Y) V);
}