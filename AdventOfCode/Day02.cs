using MiscUtil.IO;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<List<int>> reports = new();

        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var parts = line.Split(" ").Where(p => p.Length != 0)
                .Select(int.Parse).ToArray();

            reports.Add(parts.ToList());
        }

        var safeCount = 0;
        foreach (var report in reports)
        {
            var increasing = report[1] > report[0];
            var previousValue = report[0];
            var monotonic = true;
            
            foreach (var value in report.Skip(1))
            {
                var diff = value - previousValue;
                if (increasing && diff <= 0 || !increasing && diff >= 0 || Math.Abs(diff) > 3)
                    monotonic = false;

                previousValue = value;
            }

            if (monotonic)
                safeCount += 1;
        }
        
        return new ValueTask<string>($"{safeCount}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<List<int>> reports = new();

        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            var parts = line.Split(" ").Where(p => p.Length != 0)
                .Select(int.Parse).ToArray();

            reports.Add(parts.ToList());
        }

        var safeCount = 0;
        foreach (var report in reports)
        {
            
            if (SequenceSafe(report) || SequenceSafe(report.AsEnumerable().Reverse().ToList()))
                safeCount += 1;
        }
        
        return new ValueTask<string>($"{safeCount}");

        bool SequenceSafe(List<int> report)
        {
            var increasing = report[1] > report[0];
            var previousLevel = report[0];
            var numUnsafe = 0;
            
            foreach (var level in report.Skip(1))
            {
                var diff = level - previousLevel;
                if (increasing && diff <= 0 || !increasing && diff >= 0 || Math.Abs(diff) > 3)
                {
                    numUnsafe += 1;
                    continue;
                }

                previousLevel = level;
            }

            return numUnsafe <= 1;
        }
    }
}
