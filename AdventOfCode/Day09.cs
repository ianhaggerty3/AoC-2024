using MiscUtil.IO;

namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            lines.Add(line);
        }
        
        return new ValueTask<string>($"{0}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>($"{0}");
    }
}
