using MiscUtil.IO;

namespace AdventOfCode;

public class Day20 : BaseDay
{
    private readonly string _input;

    public Day20()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        var width = lines[0].Length;
        var height = lines.Count;
        
        HashSet<Position> walls = new();
        var i = 0;
        Position start = new(0, 0);
        Position end = new(0, 0);
        foreach (var line in lines)
        {
            var j = 0;
            foreach (var token in line)
            {
                if (token == '#')
                    walls.Add(new Position(j, i));
                else if (token == 'S')
                    start = new Position(j, i);
                else if (token == 'E')
                    end = new Position(j, i);
                j++;
            }

            i++;
        }

        Dictionary<Position, int> visited = [];
        List<Direction> allDirections = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];
        List<(Position A, Position B)> possibleCheats = [];
        var current = start;
        i = 0;
        while (current != end)
        {
            visited[current] = i;
            foreach (var direction in allDirections)
            {
                if (CanCheat(current, direction))
                {
                    possibleCheats.Add((current, GetCheat(current, direction)));
                }
            }
            current = GetNeighbor(current);
            i++;

        }

        visited[end] = i;

        var total = 0;
        foreach (var possibleCheat in possibleCheats)
        {
            if (visited[possibleCheat.B] - visited[possibleCheat.A] - 2 >= 100)
                total += 1;

        }

        bool CanCheat(Position p, Direction d)
        {
            var newPos = GetCheat(p, d);

            return newPos.X >= 0 && newPos.Y >= 0 && newPos.X < width && newPos.Y < height && !walls.Contains(newPos);
        }

        Position GetCheat(Position p, Direction d)
        {
            var diff = GetDiff(d);
            var newPos = Add(Add(p, diff), diff);

            return newPos;
        }

        Position GetNeighbor(Position position)
        {
            return allDirections
                .Select(d => Add(position, GetDiff(d)))
                .Single(p => !visited.ContainsKey(p) && !walls.Contains(p));
        }

        Position Add(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        Position GetDiff(Direction d)
        {
            return d switch
            {
                Direction.Up => new Position(0, -1),
                Direction.Down => new Position(0, 1),
                Direction.Left => new Position(-1, 0),
                Direction.Right => new Position(1, 0),
                _ => throw new ArgumentException(nameof(d))
            };
        }

        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
                List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        var width = lines[0].Length;
        var height = lines.Count;
        
        HashSet<Position> walls = new();
        var i = 0;
        Position start = new(0, 0);
        Position end = new(0, 0);
        foreach (var line in lines)
        {
            var j = 0;
            foreach (var token in line)
            {
                if (token == '#')
                    walls.Add(new Position(j, i));
                else if (token == 'S')
                    start = new Position(j, i);
                else if (token == 'E')
                    end = new Position(j, i);
                j++;
            }

            i++;
        }

        Dictionary<Position, int> visited = [];
        List<Direction> allDirections = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];
        var current = start;
        i = 0;
        while (current != end)
        {
            visited[current] = i;
            current = GetNeighbor(current);
            i++;

        }

        visited[end] = i;

        var total = 0;
        foreach (var p in visited.Keys)
        {
            var possibleEnds = visited.Keys.Where(v => GetDistance(p, v) <= 20);

            var oldTotal = total;
            foreach (var possibleEnd in possibleEnds)
            {
                if (visited[possibleEnd] - visited[p] - GetDistance(p, possibleEnd) >= 100)
                    total += 1;
            }
            
            Console.WriteLine($"{p} had {total - oldTotal} cheats that save ");

        }

        Position GetNeighbor(Position position)
        {
            return allDirections
                .Select(d => Add(position, GetDiff(d)))
                .Single(p => !visited.ContainsKey(p) && !walls.Contains(p));
        }

        Position Add(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        Position GetDiff(Direction d)
        {
            return d switch
            {
                Direction.Up => new Position(0, -1),
                Direction.Down => new Position(0, 1),
                Direction.Left => new Position(-1, 0),
                Direction.Right => new Position(1, 0),
                _ => throw new ArgumentException(nameof(d))
            };
        }

        return new ValueTask<string>($"{total}");

        int GetDistance(Position p, Position v)
        {
            return Math.Abs(p.X - v.X) + Math.Abs(p.Y - v.Y);
        }
    }

    private record Position(int X, int Y);

    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}