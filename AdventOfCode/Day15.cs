using MiscUtil.IO;

namespace AdventOfCode;

public class Day15 : BaseDay
{
    private readonly string _input;

    public Day15()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        HashSet<(int X, int Y)> walls = new();
        HashSet<(int X, int Y)> boxes = new();
        (int X, int Y) robot = (0, 0);
        List<Direction> directions = new();

        var width = 0;
        var height = 0;

        var i = 0;
        while (lines[i].Contains('#'))
        {
            var j = 0;
            foreach (var token in lines[i])
            {
                if (token == '#')
                    walls.Add((j, i));
                else if (token == 'O')
                    boxes.Add((j, i));
                else if (token == '@')
                    robot = (j, i);
                j++;
            }

            width = j;

            i++;
        }

        height = i;

        i++;
        while (i < lines.Count)
        {
            foreach (var token in lines[i])
            {
                if (token == '^')
                    directions.Add(Direction.Up);
                if (token == 'v')
                    directions.Add(Direction.Down);
                if (token == '<')
                    directions.Add(Direction.Left);
                if (token == '>')
                    directions.Add(Direction.Right);
            }

            i++;
        }

        // PrintMap();

        foreach (var direction in directions) SimulateMove(direction);

        // PrintMap();
        void SimulateMove(Direction direction)
        {
            var diff = DirectionDiff(direction);
            var start = (robot.X + diff.X, robot.Y + diff.Y);
            (int X, int Y) current = start;

            while (!walls.Contains(current))
            {
                if (!boxes.Contains(current))
                {
                    robot = start;
                    boxes.Add(current);
                    boxes.Remove(start);
                    return;
                }

                current = (current.X + diff.X, current.Y + diff.Y);
            }

            // if (start != current)
            // {
            //     
            // }
            // else if (!walls.Contains(current) && !boxes.Contains(current))
            // {
            //     robot = start;
            // }
        }

        (int X, int Y) DirectionDiff(Direction direction)
        {
            return direction switch
            {
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0)
            };
        }

        void PrintMap()
        {
            foreach (var i in Enumerable.Range(0, height))
            {
                var lineString = "";
                foreach (var j in Enumerable.Range(0, width))
                    if (walls.Contains((j, i)))
                        lineString += "#";

                    else if (boxes.Contains((j, i)))
                        lineString += "O";
                    else if (robot == (j, i))
                        lineString += "@";
                    else
                        lineString += ".";

                Console.WriteLine(lineString);
            }
        }

        long total = boxes.Select(box => 100 * box.Y + box.X).Sum();


        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var newLine = line.Replace("#", "##").Replace(".", "..").Replace("@", "@.").Replace("O", "[]");
            lines.Add(newLine);
        }

        HashSet<(int X, int Y)> walls = new();
        HashSet<Box> boxes = new();
        (int X, int Y) robot = (0, 0);
        List<Direction> directions = new();

        var width = 0;
        var height = 0;

        var i = 0;
        while (lines[i].Contains('#'))
        {
            var j = 0;
            foreach (var token in lines[i])
            {
                if (token == '#')
                    walls.Add((j, i));
                else if (token == '[')
                    boxes.Add(new Box((j, i), (j + 1, i)));
                else if (token == '@')
                    robot = (j, i);
                j++;
            }

            width = j;

            i++;
        }

        height = i;

        i++;
        while (i < lines.Count)
        {
            foreach (var token in lines[i])
            {
                if (token == '^')
                    directions.Add(Direction.Up);
                if (token == 'v')
                    directions.Add(Direction.Down);
                if (token == '<')
                    directions.Add(Direction.Left);
                if (token == '>')
                    directions.Add(Direction.Right);
            }

            i++;
        }

        PrintMap();

        foreach (var direction in directions) SimulateMove(direction);

        PrintMap();
        void SimulateMove(Direction direction)
        {
            var diff = DirectionDiff(direction);
            var start = (robot.X + diff.X, robot.Y + diff.Y);
            (int X, int Y) current = start;

            if (!walls.Contains(current))
            {
                var box = boxes.FirstOrDefault(b => b.Left == current || b.Right == current);

                if (box is null)
                    robot = start;
                else if (DoesBoxMove(direction, box))
                {
                    robot = start;
                    SimulateBoxMove(direction, box);
                }
            }
        }

        bool DoesBoxMove(Direction direction, Box box)
        {
            var diff = DirectionDiff(direction);

            var newLeft = Add(box.Left, diff);
            var newRight = Add(box.Right, diff);

            if (!walls.Contains(newLeft) && !walls.Contains(newRight))
            {
                var leftBox = boxes.Except([box]).FirstOrDefault(b => b.Left == newLeft || b.Right == newLeft);
                var rightBox = boxes.Except([box]).FirstOrDefault(b => b.Left == newRight || b.Right == newRight);

                var leftResult = true;
                if (leftBox is not null) leftResult = DoesBoxMove(direction, leftBox);

                var rightResult = true;
                if (rightBox is not null && rightBox != leftBox) rightResult = DoesBoxMove(direction, rightBox);

                return leftResult && rightResult;
            }

            return false;
        }
        
        void SimulateBoxMove(Direction direction, Box box)
        {
            var diff = DirectionDiff(direction);

            var newLeft = Add(box.Left, diff);
            var newRight = Add(box.Right, diff);

            if (!walls.Contains(newLeft) && !walls.Contains(newRight))
            {
                var leftBox = boxes.Except([box]).FirstOrDefault(b => b.Left == newLeft || b.Right == newLeft);
                var rightBox = boxes.Except([box]).FirstOrDefault(b => b.Left == newRight || b.Right == newRight);

                if (leftBox is not null) SimulateBoxMove(direction, leftBox);

                if (rightBox is not null && rightBox != leftBox) SimulateBoxMove(direction, rightBox);

                boxes.Remove(box);
                boxes.Add(new Box(newLeft, newRight));
            }
        }

        (int X, int Y) DirectionDiff(Direction direction)
        {
            return direction switch
            {
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0)
            };
        }

        (int X, int Y) Add((int X, int Y) a, (int X, int Y) b)
        {
            return (a.X + b.X, a.Y + b.Y);
        }

        void PrintMap()
        {
            foreach (var i in Enumerable.Range(0, height))
            {
                var lineString = "";
                foreach (var j in Enumerable.Range(0, width))
                    if (walls.Contains((j, i)))
                        lineString += "#";

                    else if (boxes.Select(b => b.Left).Contains((j, i)))
                        lineString += "[";
                    else if (boxes.Select(b => b.Right).Contains((j, i)))
                        lineString += "]";
                    else if (robot == (j, i))
                        lineString += "@";
                    else
                        lineString += ".";

                Console.WriteLine(lineString);
            }
        }

        long total = boxes.Select(box => 100 * box.Left.Y + box.Left.X).Sum();


        return new ValueTask<string>($"{total}");
    }

    internal record Box((int X, int Y) Left, (int X, int Y) Right);

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}