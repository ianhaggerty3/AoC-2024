using MiscUtil.IO;

namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<long> stones = new();
        
        foreach (var stone in _input.Split(' '))
        {
            stones.Add(long.Parse(stone));
        }

        foreach (var i in Enumerable.Range(0, 25))
        {
            stones = SimulateBlink();
        }
        
        return new ValueTask<string>($"{stones.Count}");

        List<long> SimulateBlink()
        {
            List<long> newStones = new();
            foreach (var stone in stones)
            {
                var stoneString = $"{stone}";
                if (stone == 0)
                    newStones.Add(1);
                else if (stoneString.Length % 2 == 0)
                {
                    newStones.Add(long.Parse(stoneString.Substring(0, stoneString.Length / 2)));
                    newStones.Add(long.Parse(stoneString.Substring(stoneString.Length / 2, stoneString.Length / 2)));
                }
                else
                {
                    newStones.Add(stone * 2024);
                }
            }
            
            return newStones;
        }
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<long, long> stones = new();
        
        foreach (var stone in _input.Split(' '))
        {
            stones[long.Parse(stone)] = 1;
        }

        foreach (var i in Enumerable.Range(0, 75))
        {
            stones = SimulateBlink();
        }
        
        return new ValueTask<string>($"{stones.Values.Sum()}");

        Dictionary<long, long> SimulateBlink()
        {
            Dictionary<long, long> newStones = new();
            foreach (var (stone, count) in stones)
            {
                var stoneString = $"{stone}";
                if (stone == 0)
                    newStones[1] = newStones.GetValueOrDefault(1) + count;
                else if (stoneString.Length % 2 == 0)
                {
                    var firstPart = long.Parse(stoneString.Substring(0, stoneString.Length / 2));
                    newStones[firstPart] = newStones.GetValueOrDefault(firstPart) + count;
                    var secondPart = long.Parse(stoneString.Substring(stoneString.Length / 2, stoneString.Length / 2));
                    newStones[secondPart] = newStones.GetValueOrDefault(secondPart) + count;
                }
                else
                {
                    newStones[stone * 2024] = newStones.GetValueOrDefault(stone * 2024) + count;
                }
            }
            
            return newStones;
        }
    }
}
