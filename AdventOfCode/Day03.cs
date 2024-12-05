using System.Text.RegularExpressions;
using MiscUtil.IO;

namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var matches = Regex.Matches(_input, @"mul\((\d{1,3}),(\d{1,3})\)");
        
        var total = 0;

        foreach (Match match in matches)
        {
            var first = int.Parse(match.Groups[1].Value);
            var second = int.Parse(match.Groups[2].Value);

            total += first * second;
        }
        
        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        var multiplyMatches = Regex.Matches(_input, @"mul\((\d{1,3}),(\d{1,3})\)");
        var doMatches = Regex.Matches(_input, @"do\(\)");
        var dontMatches = Regex.Matches(_input, @"don't\(\)");
        List<int> doIndices = [0, ..doMatches.Select(m => m.Index)];
        List<int?> dontIndices = dontMatches.Select(m => (int?) m.Index).ToList();
        
        var total = 0;

        foreach (Match match in multiplyMatches)
        {
            if (!IsEnabled(match.Index))
                continue;
            
            var first = int.Parse(match.Groups[1].Value);
            var second = int.Parse(match.Groups[2].Value);

            total += first * second;
        }

        bool IsEnabled(int index)
        {
            var doIndex = doIndices.Last(i => i <= index);
            int? dontIndex = dontIndices.LastOrDefault(i => i <= index) ?? -1;

            return doIndex > dontIndex;
        }
        
        return new ValueTask<string>($"{total}");
    }
}
