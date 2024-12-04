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
        
        
        return new ValueTask<string>($"{0}");
    }
}
