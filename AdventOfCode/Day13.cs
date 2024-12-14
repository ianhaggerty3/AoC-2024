using System.Text.RegularExpressions;
using MiscUtil.IO;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly string _input;

    public Day13()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<Machine> machines = new();
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        for (var i = 0; i <= lines.Count / 4; i++)
        {
            var a = Regex.Matches(lines[4 * i], @".* X\+(\d+), Y\+(\d+)").Single();
            var b = Regex.Matches(lines[4 * i + 1], @".* X\+(\d+), Y\+(\d+)").Single();
            var prize = Regex.Matches(lines[4 * i + 2], @".* X\=(\d+), Y\=(\d+)").Single();

            machines.Add(new Machine((long.Parse(a.Groups[1].Value), long.Parse(a.Groups[2].Value)),
                (long.Parse(b.Groups[1].Value), long.Parse(b.Groups[2].Value)),
                (long.Parse(prize.Groups[1].Value), long.Parse(prize.Groups[2].Value))));
        }

        var j = -1;
        var total = 0;
        foreach (var machine in machines)
        {
            j++;

            decimal A = machine.A.A;
            decimal B = machine.B.B;
            decimal C = machine.Prize.C;
            decimal D = machine.A.D;
            decimal E = machine.B.E;
            decimal F = machine.Prize.F;

            var denominator = E - B * D / A;
            var numerator = F - D * C / A;
            if (denominator == 0) continue;

            var bCount = numerator / denominator;
            if (Math.Abs(bCount - Math.Round(bCount)) > 0.001m) continue;

            var aCount = (C - B * bCount) / A;
            if (Math.Abs(aCount - Math.Round(aCount)) > 0.001m)
            {
                Console.WriteLine($"Machine {j} not possible A = {aCount} B = {bCount}");
                continue;
            }

            if (Math.Abs(A / B - Math.Round(A / B)) < 0.001m && Math.Abs(D / E - Math.Round(D / E)) < 0.001m)
                Console.WriteLine($"Red alert for machine {j}");

            if (aCount <= 100 && bCount <= 100) total += 3 * (int)Math.Round(aCount) + 1 * (int)Math.Round(bCount);
        }

        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<Machine> machines = new();
        List<string> lines = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        for (var i = 0; i <= lines.Count / 4; i++)
        {
            var a = Regex.Matches(lines[4 * i], @".* X\+(\d+), Y\+(\d+)").Single();
            var b = Regex.Matches(lines[4 * i + 1], @".* X\+(\d+), Y\+(\d+)").Single();
            var prize = Regex.Matches(lines[4 * i + 2], @".* X\=(\d+), Y\=(\d+)").Single();

            machines.Add(new Machine((long.Parse(a.Groups[1].Value), long.Parse(a.Groups[2].Value)),
                (long.Parse(b.Groups[1].Value), long.Parse(b.Groups[2].Value)),
                // (long.Parse(prize.Groups[1].Value), long.Parse(prize.Groups[2].Value))));
                (long.Parse($"{prize.Groups[1].Value}") + 10000000000000,
                    long.Parse($"{prize.Groups[2].Value}") + 10000000000000)));
        }

        var j = -1;
        long total = 0;
        foreach (var machine in machines)
        {
            j++;

            var A = machine.A.A;
            var B = machine.B.B;
            var C = machine.Prize.C;
            var D = machine.A.D;
            var E = machine.B.E;
            var F = machine.Prize.F;

            var denominator = A * E - B * D;
            var numerator = A * F - D * C;
            if (denominator == 0)
                // Console.WriteLine($"Stopping on first check for machine {j}");
                continue;

            if (numerator % denominator != 0)
                // Console.WriteLine($"Stopping on middle check for machine {j}");
                continue;

            var bCount = numerator / denominator;
            if ((C - B * bCount) % A != 0)
            {
                Console.WriteLine($"Stopping on second check for machine {j}");
                continue;
            }

            var aCount = (C - B * bCount) / A;

            if (A % B == 0 && D % E == 0)
                Console.WriteLine($"Red alert for machine {j}");

            total += 3 * aCount + 1 * bCount;
        }

        return new ValueTask<string>($"{total}");
    }

    internal record Machine((long A, long D) A, (long B, long E) B, (long C, long F) Prize);
}