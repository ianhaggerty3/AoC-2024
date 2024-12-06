using MiscUtil.Collections.Extensions;
using MiscUtil.IO;

namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string _input;

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        bool emptyFound = false;
        Dictionary<int, List<int>> rules = new();
        List<List<int>> updates = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            if (line.Length == 0)
            {
                emptyFound = true;
                continue;
            }

            if (!emptyFound)
            {
                var parts = line.Split('|');
                var first = int.Parse(parts[0]);
                var second = int.Parse(parts[1]);

                rules[first] = rules.GetValueOrDefault(first, []).Append(second).ToList();
            }
            else
            {
                updates.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }

        var total = 0;
        foreach (var update in updates)
        {
            if (IsInOrder(update))
            {
                var addition = update[update.Count / 2];
                total += addition;
            }
        }
        
        return new ValueTask<string>($"{total}");


        bool IsInOrder(List<int> update)
        {
            for (var i = update.Count - 1; i >= 0; i--)
            {
                var current = update[i];
                for (var j = 0; j < i; j++)
                {
                    if (rules.GetValueOrDefault(current, []).Contains(update[j]))
                        return false;
                }
                
            }
            
            return true;
        }
    }

    public override ValueTask<string> Solve_2()
    {
        bool emptyFound = false;
        Dictionary<int, List<int>> rules = new();
        List<List<int>> updates = new();
        foreach (var line in new LineReader(() => new StringReader(_input)))
        {
            if (line.Length == 0)
            {
                emptyFound = true;
                continue;
            }

            if (!emptyFound)
            {
                var parts = line.Split('|');
                var first = int.Parse(parts[0]);
                var second = int.Parse(parts[1]);

                rules[first] = rules.GetValueOrDefault(first, []).Append(second).ToList();
            }
            else
            {
                updates.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }

        var total = 0;
        foreach (var update in updates)
        {
            if (!IsInOrder(update))
            {
                update.Sort((a, b) =>
                {
                    if (rules.GetValueOrDefault(a, []).Contains(b))
                        return -1;

                    if (rules.GetValueOrDefault(b, []).Contains(a))
                        return 1;

                    return 0;
                });
                var addition = update[update.Count / 2];
                total += addition;
            }
        }
        
        return new ValueTask<string>($"{total}");
        
        bool IsInOrder(List<int> update)
        {
            for (var i = update.Count - 1; i >= 0; i--)
            {
                var current = update[i];
                for (var j = 0; j < i; j++)
                {
                    if (rules.GetValueOrDefault(current, []).Contains(update[j]))
                        return false;
                }
                
            }
            
            return true;
        }
    }
}
