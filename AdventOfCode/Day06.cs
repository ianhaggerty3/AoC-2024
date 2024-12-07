using MiscUtil.IO;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string _input;

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var i = 0;
        var iStart = 0;
        var jStart = 0;
        int height;
        var width = 0;
        HashSet<(int, int)> locations = new();
        Dictionary<(int, int), HashSet<Direction>> visited = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            for (var j = 0; j < line.Length; j++)
            {
                var token = line[j];
                if (token == '#')
                    locations.Add((i, j));
                if (token == '^')
                {
                    iStart = i;
                    jStart = j;
                }
            }

            width = line.Length;

            i++;
        }

        height = i;

        var currentPosition = (iStart, jStart);
        var currentDirection = Direction.Up;
        while (IsInRange(currentPosition) && !visited.GetValueOrDefault(currentPosition, new HashSet<Direction>())
                   .Contains(currentDirection))
        {
            var directionHistory = visited.GetValueOrDefault(currentPosition, new HashSet<Direction>());
            directionHistory.Add(currentDirection);
            visited[currentPosition] = directionHistory;
            var nextState = GetNextState(currentPosition, currentDirection);
            currentPosition = nextState.Item1;
            currentDirection = nextState.Item2;
        }


        return new ValueTask<string>($"{visited.Keys.Count}");

        ((int, int), Direction) GetNextState((int, int) current, Direction direction)
        {
            var diff = GetPositionDiff(direction);
            var next = (current.Item1 + diff.Item1, current.Item2 + diff.Item2);
            if (locations.Contains(next)) return (current, GetNextDirection(direction));

            return (next, direction);
        }

        bool IsInRange((int, int) position)
        {
            return position.Item1 >= 0 && position.Item1 < height && position.Item2 >= 0 && position.Item2 < width;
        }

        (int, int) GetPositionDiff(Direction direction)
        {
            return direction switch
            {
                Direction.Up => (-1, 0),
                Direction.Right => (0, 1),
                Direction.Down => (1, 0),
                Direction.Left => (0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        Direction GetNextDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }
    }

    public override ValueTask<string> Solve_2()
    {
        var i = 0;
        var iStart = 0;
        var jStart = 0;
        int height;
        var width = 0;
        HashSet<(int, int)> locations = new();
        Dictionary<(int, int), Direction> visited = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            for (var j = 0; j < line.Length; j++)
            {
                var token = line[j];
                if (token == '#')
                    locations.Add((i, j));
                if (token == '^')
                {
                    iStart = i;
                    jStart = j;
                }
            }

            width = line.Length;

            i++;
        }

        height = i;

        var currentPosition = (iStart, jStart);
        var currentDirection = Direction.Up;
        while (IsInRange(currentPosition))
        {
            visited.TryAdd(currentPosition, currentDirection);

            var nextState = GetNextState(currentPosition, currentDirection, locations);
            currentPosition = nextState.Item1;
            currentDirection = nextState.Item2;
        }

        var loopPositions = new HashSet<(int, int)>();
        foreach (var state in visited.Where(p => p.Key != (iStart, jStart)))
        {
            var obstructions = locations.Concat([state.Key]).ToHashSet();
            var reverseDiff = GetReversePositionDiff(state.Value);
            var start = (state.Key.Item1 + reverseDiff.Item1, state.Key.Item2 + reverseDiff.Item2);
            if (SimulationEndInLoop(obstructions, start, state.Value))
                loopPositions.Add(state.Key);
        }


        return new ValueTask<string>($"{loopPositions.Count}");

        bool SimulationEndInLoop(HashSet<(int, int)> obstructions, (int, int) startingPosition,
            Direction startingDirection)
        {
            var currentPosition = startingPosition;
            var currentDirection = startingDirection;
            Dictionary<(int, int), HashSet<Direction>> visited = new();

            while (IsInRange(currentPosition))
            {
                if (visited.GetValueOrDefault(currentPosition, new HashSet<Direction>())
                    .Contains(currentDirection))
                    return true;

                var directionHistory = visited.GetValueOrDefault(currentPosition, new HashSet<Direction>());
                directionHistory.Add(currentDirection);
                visited[currentPosition] = directionHistory;

                var nextState = GetNextState(currentPosition, currentDirection, obstructions);
                currentPosition = nextState.Item1;
                currentDirection = nextState.Item2;
            }

            return false;
        }

        ((int, int), Direction) GetNextState((int, int) current, Direction direction, HashSet<(int, int)> obstructions)
        {
            var diff = GetPositionDiff(direction);
            var next = (current.Item1 + diff.Item1, current.Item2 + diff.Item2);
            if (obstructions.Contains(next)) return (current, GetNextDirection(direction));

            return (next, direction);
        }

        bool IsInRange((int, int) position)
        {
            return position.Item1 >= 0 && position.Item1 < height && position.Item2 >= 0 && position.Item2 < width;
        }

        (int, int) GetPositionDiff(Direction direction)
        {
            return direction switch
            {
                Direction.Up => (-1, 0),
                Direction.Right => (0, 1),
                Direction.Down => (1, 0),
                Direction.Left => (0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        (int, int) GetReversePositionDiff(Direction direction)
        {
            return direction switch
            {
                Direction.Up => (1, 0),
                Direction.Right => (0, -1),
                Direction.Down => (-1, 0),
                Direction.Left => (0, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        Direction GetNextDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }
    }

    internal enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}