using MiscUtil.IO;

namespace AdventOfCode;

public class Day18 : BaseDay
{
    private readonly string _input;

    public Day18()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<Point> bytes = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var pair = line.Split(',').Select(int.Parse).ToList();

            bytes.Add(new Point(pair[0], pair[1]));
        }

        var width = 71;
        var height = 71;

        var activeBytes = bytes[..1024];

        var start = new Point(0, 0);
        var visited = new HashSet<Point>();
        var neighbors = new Dictionary<Point, int> { { start, 0 } };
        var end = new Point(width - 1, height - 1);

        var finalCost = 0;
        while (neighbors.Count != 0)
        {
            var current = neighbors.OrderBy(p => p.Value).First();
            visited.Add(current.Key);
            neighbors.Remove(current.Key);

            if (current.Key == end)
            {
                finalCost = current.Value;
                break;
            }

            foreach (var neighbor in GetNeighbors(current.Key))
            {
                if (visited.Contains(neighbor))
                    continue;

                var newCost = current.Value + 1;
                if (neighbors.GetValueOrDefault(neighbor, int.MaxValue) > newCost) neighbors[neighbor] = newCost;
            }
        }

        List<Point> GetNeighbors(Point p)
        {
            List<Direction> allDirections = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];
            return allDirections.Select(d => GetDiff(d)).Select(d => Add(p, d)).Where(n =>
                !activeBytes.Contains(n) && n.X >= 0 && n.Y >= 0 && n.X < width && n.Y < height).ToList();
        }

        Point Add(Point a, (int X, int Y) b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }


        (int X, int Y) GetDiff(Direction d)
        {
            return d switch
            {
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0),
                _ => throw new ArgumentException(nameof(d))
            };
        }

        return new ValueTask<string>($"{finalCost}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<Point> bytes = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var pair = line.Split(',').Select(int.Parse).ToList();

            bytes.Add(new Point(pair[0], pair[1]));
        }

        var currentByte = 1024;
        while (!HasPath(bytes[..currentByte], bytes[currentByte-1])) currentByte++;

        Console.WriteLine($"currentByte = {currentByte}");
        
        bool HasPath(List<Point> activeBytes, Point start)
        {
            var width = 71;
            var height = 71;
            var visited = new HashSet<Point>();
            var neighbors = new HashSet<Point> { start };

            var leftEdge = false;
            var topEdge = false;
            var rightEdge = false;
            var bottomEdge = false;

            while (neighbors.Count != 0)
            {
                var current = neighbors.First();
                visited.Add(current);
                neighbors.Remove(current);

                if (current.X == 0) leftEdge = true;
                
                if (current.X == width - 1) rightEdge = true;
                
                if (current.Y == 0) topEdge = true;
                
                if (current.Y == height - 1) bottomEdge = true;
                

                foreach (var neighbor in GetNeighbors(current, activeBytes))
                {
                    if (visited.Contains(neighbor))
                        continue;

                    neighbors.Add(neighbor);
                }
            }

            return (leftEdge && topEdge) || (rightEdge && bottomEdge);
        }


        List<Point> GetNeighbors(Point p, List<Point> activeBytes)
        {
            return GetAllDiffs().Select(d => Add(p, d)).Where(activeBytes.Contains).ToList();
        }

        Point Add(Point a, (int X, int Y) b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }


        List<(int X, int Y)> GetAllDiffs()
        {
            return [(0, -1), (0, 1), (-1, 0), (1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1)];
        }

        return new ValueTask<string>($"{bytes[currentByte-1].X},{bytes[currentByte-1].Y}");
    }

    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    private record Point(int X, int Y);
}