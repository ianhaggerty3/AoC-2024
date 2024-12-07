using MiscUtil.IO;

namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string _input;

    public Day07()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<(int, long), List<int>> equations = new();
        int i = 0;
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var parts = line.Split(':');
            var testValue = long.Parse(parts[0]);
            var operands = parts[1].Split(' ').Where(p => p.Length != 0).Select(int.Parse).ToList();
            equations.Add((i, testValue), operands);
            i++;
        }

        long total = 0;
        foreach (var equation in equations)
        {
            total += IsBalanced(equation.Key.Item2, equation.Value[0], equation.Value[1..]) ? equation.Key.Item2 : 0;
        }
        
        return new ValueTask<string>($"{total}");

        bool IsBalanced(long expectedTotal, long currentTotal, List<int> operands)
        {
            if (operands.Count == 0)
                return expectedTotal == currentTotal;

            if (currentTotal > expectedTotal)
                return false;

            return IsBalanced(expectedTotal, currentTotal * operands[0], operands[1..]) ||
                   IsBalanced(expectedTotal, currentTotal + operands[0], operands[1..]);
        }
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<(int, long), List<int>> equations = new();
        int i = 0;
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var parts = line.Split(':');
            var testValue = long.Parse(parts[0]);
            var operands = parts[1].Split(' ').Where(p => p.Length != 0).Select(int.Parse).ToList();
            equations.Add((i, testValue), operands);
            i++;
        }

        long total = 0;
        foreach (var equation in equations)
        {
            total += IsBalanced(equation.Key.Item2, equation.Value[0], equation.Value[1..]) ? equation.Key.Item2 : 0;
        }
        
        return new ValueTask<string>($"{total}");

        bool IsBalanced(long expectedTotal, long currentTotal, List<int> operands)
        {
            if (operands.Count == 0)
                return expectedTotal == currentTotal;

            if (currentTotal > expectedTotal)
                return false;

            long concatTotal = long.Parse($"{currentTotal}{operands[0]}");

            return IsBalanced(expectedTotal, currentTotal * operands[0], operands[1..]) ||
                   IsBalanced(expectedTotal, currentTotal + operands[0], operands[1..]) ||
                   IsBalanced(expectedTotal, concatTotal, operands[1..]);
        }

    }
}