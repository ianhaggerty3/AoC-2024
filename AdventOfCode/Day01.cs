using MiscUtil.IO;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<int> firstList = new();
        List<int> secondList = new();

        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var parts = line.Split(" ").Where(p => p.Length != 0).ToArray();

            firstList.Add(int.Parse(parts[0]));
            secondList.Add(int.Parse(parts[1]));
        }
        
        firstList.Sort();
        secondList.Sort();

        var total = 0;
        foreach (var (first, second) in firstList.Zip(secondList))
        {
            total += Math.Abs(first - second);
        }
        
        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<int> firstList = new();
        List<int> secondList = new();

        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var parts = line.Split(" ").Where(p => p.Length != 0).ToArray();

            firstList.Add(int.Parse(parts[0]));
            secondList.Add(int.Parse(parts[1]));
        }

        var total = 0;
        foreach (var number in firstList)
        {
            var count = secondList.Count(n => n == number);
            total += number * count;
        }

        return new ValueTask<string>($"{total}");
    }
}
