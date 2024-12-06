using MiscUtil.IO;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string _input;

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    internal enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    
    public override ValueTask<string> Solve_1()
    {
        int i = 0;
        int iStart = 0;
        int jStart = 0;
        int height;
        int width = 0;
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
            var next = (current.Item1 + diff.Item1, current.Item2+ diff.Item2);
            if (locations.Contains(next))
            {
                return (current, GetNextDirection(direction));
            }

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
                int i = 0;
        int iStart = 0;
        int jStart = 0;
        int height;
        int width = 0;
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
        bool isStart = true;
        while (IsInRange(currentPosition) && !visited.GetValueOrDefault(currentPosition, new HashSet<Direction>())
                   .Contains(currentDirection))
        {
            if (isStart)
                isStart = false;
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
        {
            foreach (var direction in state.Value)
            {
                if (IsPotentialLoop(state.Key, direction))
                {
                    loopPositions.Add(state.Key);
                }
            }
        }
        
        
        return new ValueTask<string>($"{loopPositions.Count}");

        bool IsPotentialLoop((int, int) position, Direction direction)
        {
            var diff = GetPositionDiff(direction);
            var reverseDiff = GetReversePositionDiff(direction);
            if (direction == Direction.Up)
            {
                var loopHeight = locations.Where(l => l.Item2 == position.Item2 && l.Item1 > position.Item1)
                    .OrderBy(l => l.Item1).First();
            } else if (direction == Direction.Down)
            {
                var loopHeight = locations.Where(l => l.Item2 == position.Item2 && l.Item1 < position.Item1)
                    .OrderBy(l => l.Item1).Last();
            }
            else if (direction == Direction.Left)
            {
                var loopWidth = locations.Where(l => l.Item1 == position.Item1 && l.Item2 > position.Item2)
                    .OrderBy(l => l.Item2).First();
            } else if (direction == Direction.Right)
            {
                var loopWidth = locations.Where(l => l.Item1 == position.Item1 && l.Item2 < position.Item2)
                    .OrderBy(l => l.Item2).Last();
            }

            return false;
        }
        
        // break into functions 
        // make sure other three sides are the same
        
        // GetLoopWidth(D)
        
        
        ((int, int), Direction) GetNextState((int, int) current, Direction direction)
        {
            var diff = GetPositionDiff(direction);
            var next = (current.Item1 + diff.Item1, current.Item2+ diff.Item2);
            if (locations.Contains(next))
            {
                return (current, GetNextDirection(direction));
            }

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
}
