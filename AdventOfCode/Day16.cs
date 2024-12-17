using MiscUtil.IO;

namespace AdventOfCode;

public class Day16 : BaseDay
{
    private readonly string _input;

    public Day16()
    {
        _input = File.ReadAllText(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        HashSet<(int X, int Y)> walls = new();
        var i = 0;
        (int X, int Y) start = (0, 0);
        (int X, int Y) end = (0, 0);
        foreach (var line in lines)
        {
            var j = 0;
            foreach (var token in line)
            {
                if (token == '#')
                    walls.Add((j, i));
                else if (token == 'S')
                    start = (j, i);
                else if (token == 'E')
                    end = (j, i);
                j++;
            }

            i++;
        }

        var potential = new Dictionary<State, long> { { new State(start.X, start.Y, Direction.Right), 0 } };
        var visited = new HashSet<State>();
        var finalScore = long.MaxValue;

        while (potential.Count != 0)
        {
            var current = potential.MinBy(p => p.Value);
            potential.Remove(current.Key);
            visited.Add(current.Key);

            if (IsEnd(current.Key))
            {
                if (finalScore > current.Value)
                    finalScore = current.Value;
                else if (finalScore < current.Value)
                    break;
            }

            var neighbors = GetNeighbors(current.Key);
            foreach (var neighbor in neighbors)
            {
                if (visited.Contains(neighbor)) continue;

                var newCost = current.Value + GetCost(current.Key, neighbor);
                potential[neighbor] = Math.Min(potential.GetValueOrDefault(neighbor, long.MaxValue), newCost);
            }
        }

        bool IsEnd(State state)
        {
            return state.X == end.X && state.Y == end.Y;
        }

        long GetCost(State oldState, State newState)
        {
            return oldState.Direction == newState.Direction ? 1 : 1000;
        }

        List<State> GetNeighbors(State state)
        {
            List<Direction> allDirections = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];

            return allDirections.Where(d => d != state.Direction + 2 % 4).Select(d =>
                {
                    var diff = GetDiff(d);
                    var newPos = d == state.Direction ? Add(diff, (state.X, state.Y)) : (state.X, state.Y);

                    return walls.Contains(newPos) ? null : new State(newPos.X, newPos.Y, d);
                })
                .WhereNotNull().ToList();
        }

        (int X, int Y) Add((int X, int Y) a, (int X, int Y) b)
        {
            return (a.X + b.X, a.Y + b.Y);
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


        return new ValueTask<string>($"{finalScore}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        HashSet<(int X, int Y)> walls = new();
        var i = 0;
        (int X, int Y) start = (0, 0);
        (int X, int Y) end = (0, 0);
        var width = 0;
        var height = lines.Count;
        foreach (var line in lines)
        {
            var j = 0;
            foreach (var token in line)
            {
                if (token == '#')
                    walls.Add((j, i));
                else if (token == 'S')
                    start = (j, i);
                else if (token == 'E')
                    end = (j, i);
                j++;
            }

            width = j;

            i++;
        }

        var potential = new Dictionary<State, long> { { new State(start.X, start.Y, Direction.Right), 0 } };
        var allCosts = new Dictionary<State, long> { { new State(start.X, start.Y, Direction.Right), 0 } };
        var visited = new HashSet<State>();
        var finalScore = long.MaxValue;

        while (potential.Count != 0)
        {
            var current = potential.MinBy(p => p.Value);
            potential.Remove(current.Key);
            visited.Add(current.Key);

            if (IsEnd(current.Key))
            {
                if (finalScore > current.Value)
                    finalScore = current.Value;
                else if (finalScore < current.Value)
                    break;
            }

            var neighbors = GetNeighbors(current.Key);
            foreach (var neighbor in neighbors)
            {
                if (visited.Contains(neighbor)) continue;

                var newCost = current.Value + GetCost(current.Key, neighbor);
                if (newCost < potential.GetValueOrDefault(neighbor, long.MaxValue))
                {
                    potential[neighbor] = newCost;
                    allCosts[neighbor] = newCost;
                }
            }
        }

        HashSet<State> bestPath = new();
        HasPathToEnd(new State(start.X, start.Y, 0));
        foreach (var state in visited) HasPathToEnd(state);

        PrintPaths();
        
        bool HasPathToEnd(State state)
        {
            if (IsEnd(state))
            {
                bestPath.Add(state);
                return true;
            }

            if (bestPath.Contains(state))
                return true;

            if (allCosts[state] > finalScore) return false;

            var neighbors = GetNeighbors(state).Where(n => allCosts.ContainsKey(n)).Where(n =>
            {
                var neighborCost = allCosts[n];
                var currentCost = allCosts[state];
                var diff = neighborCost - currentCost;
                return diff == 1 || diff == 1000;
            });
            
            var ret = neighbors.Any(HasPathToEnd);

            if (ret) bestPath.Add(state);

            return ret;
        }

        void PrintPaths()
        {
            foreach (var i in Enumerable.Range(0, height))
            {
                var lineString = "";
                foreach (var j in Enumerable.Range(0, width))
                    if (walls.Contains((j, i)))
                        lineString += '#';
                    else if (bestPath.Count(s => (s.X, s.Y) == (j, i)) != 0)
                        lineString += 'O';
                    else
                        lineString += '.';
                Console.WriteLine(lineString);
            }
        }

        bool IsEnd(State state)
        {
            return state.X == end.X && state.Y == end.Y;
        }

        long GetCost(State oldState, State newState)
        {
            return oldState.Direction == newState.Direction ? 1 : 1000;
        }

        List<State> GetNeighbors(State state)
        {
            List<Direction> allDirections = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];

            return allDirections.Where(d => d != state.Direction + 2 % 4).Select(d =>
                {
                    var diff = GetDiff(d);
                    var newPos = d == state.Direction ? Add(diff, (state.X, state.Y)) : (state.X, state.Y);

                    return walls.Contains(newPos) ? null : new State(newPos.X, newPos.Y, d);
                })
                .WhereNotNull().ToList();
        }

        (int X, int Y) Add((int X, int Y) a, (int X, int Y) b)
        {
            return (a.X + b.X, a.Y + b.Y);
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


        return new ValueTask<string>($"{bestPath.Select(s => (s.X, s.Y)).Distinct().Count()}");
    }

    private record State(int X, int Y, Direction Direction);

    internal enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}

public static class Extension
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> o) where T : class
    {
        return o.Where(x => x != null)!;
    }
}