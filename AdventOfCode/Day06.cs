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
        var isStart = true;
        while (IsInRange(currentPosition) && !visited.GetValueOrDefault(currentPosition, new HashSet<Direction>())
                   .Contains(currentDirection))
        {
            if (isStart)
            {
                isStart = false;
            }
            else
            {
                var directionHistory = visited.GetValueOrDefault(currentPosition, new HashSet<Direction>());
                directionHistory.Add(currentDirection);
                visited[currentPosition] = directionHistory;
            }

            var nextState = GetNextState(currentPosition, currentDirection);
            currentPosition = nextState.Item1;
            currentDirection = nextState.Item2;
        }

        var loopPositions = new HashSet<(int, int)>();
        foreach (var state in visited)
        foreach (var direction in state.Value)
            if (IsPotentialLoop(state.Key, direction))
                loopPositions.Add(state.Key);


        return new ValueTask<string>($"{loopPositions.Count}");

        bool IsPotentialLoop((int, int) position, Direction direction)
        {
            if (direction == Direction.Up)
            {
                var topLeft = (position.Item1 + 1, position.Item2);
                var topRightObstruction = GetNearestLocationToRight(topLeft);
                if (topRightObstruction is null)
                    return false;
                var topRight = (topRightObstruction.Value.Item1, topRightObstruction.Value.Item2 - 1);

                var bottomRightObstruction = GetNearestLocationBelow(topRight);

                if (bottomRightObstruction is null)
                    return false;
                var bottomRight = (bottomRightObstruction.Value.Item1 - 1, bottomRightObstruction.Value.Item2);

                var bottomLeftObstruction = GetNearestLocationToLeft(bottomRight);
                if (bottomLeftObstruction is null)
                    return false;

                var bottomLeft = (bottomLeftObstruction.Value.Item1, bottomLeftObstruction.Value.Item2 + 1);
                var topLeftObstruction = GetNearestLocationAbove(bottomLeft);
                if (topLeftObstruction is not null && topLeftObstruction.Value.Item1 > position.Item1)
                    return false;
                
                Console.WriteLine("Reporting true for up");
            }
            else if (direction == Direction.Down)
            {
                var bottomRight = (position.Item1 - 1, position.Item2);
                
                var bottomLeftObstruction = GetNearestLocationToLeft(bottomRight);
                if (bottomLeftObstruction is null)
                    return false;
                
                var bottomLeft = (bottomLeftObstruction.Value.Item1, bottomLeftObstruction.Value.Item2 + 1);
                var topLeftObstruction = GetNearestLocationAbove(bottomLeft);
                if (topLeftObstruction is null )
                    return false;
                
                var topLeft = (topLeftObstruction.Value.Item1 + 1, topLeftObstruction.Value.Item2);
                var topRightObstruction = GetNearestLocationToRight(topLeft);
                if (topRightObstruction is null)
                    return false;
                var topRight = (topRightObstruction.Value.Item1, topRightObstruction.Value.Item2 - 1);

                var bottomRightObstruction = GetNearestLocationBelow(topRight);

                if (bottomRightObstruction is not null && bottomRightObstruction.Value.Item1 < position.Item1)
                    return false;
                
                Console.WriteLine("Reporting true for down");
            }
            else if (direction == Direction.Left)
            {
                var bottomLeft = (position.Item1, position.Item2 + 1);
                
                var topLeftObstruction = GetNearestLocationAbove(bottomLeft);
                if (topLeftObstruction is null )
                    return false;
                
                var topLeft = (topLeftObstruction.Value.Item1 + 1, topLeftObstruction.Value.Item2);
                var topRightObstruction = GetNearestLocationToRight(topLeft);
                if (topRightObstruction is null)
                    return false;
                var topRight = (topRightObstruction.Value.Item1, topRightObstruction.Value.Item2 - 1);

                var bottomRightObstruction = GetNearestLocationBelow(topRight);

                if (bottomRightObstruction is null)
                    return false;
                
                var bottomRight = (bottomRightObstruction.Value.Item1 - 1, bottomRightObstruction.Value.Item2);
                
                var bottomLeftObstruction = GetNearestLocationToLeft(bottomRight);
                if (bottomLeftObstruction is not null && bottomLeftObstruction.Value.Item2 > position.Item2)
                    return false;
                
                Console.WriteLine("Reporting true for left");
            }
            else if (direction == Direction.Right)
            {
                var topRight = (position.Item1, position.Item2 - 1);
                
                var bottomRightObstruction = GetNearestLocationBelow(topRight);
                if (bottomRightObstruction is null)
                    return false;
                
                var bottomRight = (bottomRightObstruction.Value.Item1 - 1, bottomRightObstruction.Value.Item2);
                var bottomLeftObstruction = GetNearestLocationToLeft(bottomRight);
                if (bottomLeftObstruction is null)
                    return false;
                
                var bottomLeft = (bottomLeftObstruction.Value.Item1, bottomLeftObstruction.Value.Item2 + 1);
                var topLeftObstruction = GetNearestLocationAbove(bottomLeft);
                if (topLeftObstruction is null )
                    return false;
                
                var topLeft = (topLeftObstruction.Value.Item1 + 1, topLeftObstruction.Value.Item2);
                var topRightObstruction = GetNearestLocationToRight(topLeft);
                if (topRightObstruction is not null && topRightObstruction.Value.Item2 < position.Item2)
                    return false;
                
                Console.WriteLine("Reporting true for right");
            }

            return true;
        }

        (int, int)? GetNearestLocationAbove((int, int) position)
        {
            return locations
                .Where(l => l.Item2 == position.Item2 && l.Item1 < position.Item1)
                .OrderBy(l => l.Item1).LastOrDefault();
        }

        (int, int)? GetNearestLocationBelow((int, int) position)
        {
            return locations
                .Where(l => l.Item2 == position.Item2 && l.Item1 > position.Item1)
                .OrderBy(l => l.Item1).FirstOrDefault();
        }

        (int, int)? GetNearestLocationToRight((int, int) position)
        {
            return locations
                .Where(l => l.Item1 == position.Item1 && l.Item2 > position.Item2)
                .OrderBy(l => l.Item2).FirstOrDefault();
        }

        (int, int)? GetNearestLocationToLeft((int, int) position)
        {
            return locations.Where(l => l.Item1 == position.Item1 && l.Item2 < position.Item2)
                .OrderBy(l => l.Item2).LastOrDefault();
        }

        // break into functions 
        // make sure other three sides are the same

        // GetLoopWidth(D)


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