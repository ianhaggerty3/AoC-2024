using MiscUtil.IO;

namespace AdventOfCode;

public class Day17 : BaseDay
{
    private readonly string _input;

    public Day17()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<string> lines = new();
        List<byte> instructions = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        var initialA = int.Parse(lines[0].Split(":")[1]);
        var initialB = int.Parse(lines[1].Split(":")[1]);
        var initialC = int.Parse(lines[2].Split(":")[1]);
        var initialState = new State(0, initialA, initialB, initialC);

        instructions = lines[4].Split(":")[1].Split(",").Select(byte.Parse).ToList();

        var current = initialState;
        List<byte> output = [];
        while (current.IP < instructions.Count)
            current = SimulateInstruction(current);


        State SimulateInstruction(State s)
        {
            var instruction = instructions[s.IP];
            var operand = instructions[s.IP + 1];

            return instruction switch
            {
                0 => s with { IP = s.IP + 2, A = (int)Math.Truncate(s.A / Math.Pow(2, GetComboOperand(operand, s))) },
                1 => s with { IP = s.IP + 2, B = GetLiteralOperand(operand) ^ s.B },
                2 => s with { IP = s.IP + 2, B = GetComboOperand(operand, s) % 8 },
                3 => s with { IP = s.A == 0 ? s.IP + 2 : (int)GetLiteralOperand(operand) },
                4 => s with { IP = s.IP + 2, B = s.B ^ s.C },
                5 => SimulateOutput(s),
                6 => s with { IP = s.IP + 2, B = (int)Math.Truncate(s.A / Math.Pow(2, GetComboOperand(operand, s))) },
                7 => s with { IP = s.IP + 2, C = (int)Math.Truncate(s.A / Math.Pow(2, GetComboOperand(operand, s))) }
            };
        }

        State SimulateOutput(State s)
        {
            var operand = instructions[s.IP + 1];

            output.Add((byte)(GetComboOperand(operand, s) % 8));
            return s with { IP = s.IP + 2 };
        }

        int GetLiteralOperand(byte operand)
        {
            return operand;
        }

        long GetComboOperand(byte operand, State state)
        {
            if (operand < 4) return operand;

            if (operand == 4) return state.A;
            if (operand == 5) return state.B;
            if (operand == 6) return state.C;

            throw new InvalidDataException();
        }

        var outputString = "";
        foreach (var num in output) outputString += $"{num},";


        return new ValueTask<string>(outputString);
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> lines = new();
        List<byte> instructions = new();
        foreach (var line in new LineReader(() => new StringReader(_input))) lines.Add(line);

        var initialB = int.Parse(lines[1].Split(":")[1]);
        var initialC = int.Parse(lines[2].Split(":")[1]);

        instructions = lines[4].Split(":")[1].Split(",").Select(byte.Parse).ToList();

        long mirrorValue = 0;

        long startingA = (long) Math.Pow(2, 42);
        
        for (var i = startingA; i < long.MaxValue; i++)
        {
            var initialState = new State(0, i, initialB, initialC);

            try
            {
                var result = SimulateProgram(initialState);
                if (result.SequenceEqual(instructions))
                {
                    mirrorValue = i;
                    break;
                }
            }
            catch
            {
                
            }
        }

        List<byte> SimulateProgram(State initialState)
        {
            var current = initialState;
            List<byte> output = [];
            while (current.IP < instructions.Count)
                current = SimulateInstruction(current, output);

            return output;
        }

        State SimulateInstruction(State s, List<byte> output)
        {
            var instruction = instructions[s.IP];
            var operand = instructions[s.IP + 1];

            return instruction switch
            {
                0 => s with { IP = s.IP + 2, A = (int)Math.Truncate(s.A / Math.Pow(2, GetComboOperand(operand, s))) },
                1 => s with { IP = s.IP + 2, B = GetLiteralOperand(operand) ^ s.B },
                2 => s with { IP = s.IP + 2, B = GetComboOperand(operand, s) % 8 },
                3 => s with { IP = s.A == 0 ? s.IP + 2 : (int)GetLiteralOperand(operand) },
                4 => s with { IP = s.IP + 2, B = s.B ^ s.C },
                5 => SimulateOutput(s, output),
                6 => s with { IP = s.IP + 2, B = (int)Math.Truncate(s.A / Math.Pow(2, GetComboOperand(operand, s))) },
                7 => s with { IP = s.IP + 2, C = (int)Math.Truncate(s.A / Math.Pow(2, GetComboOperand(operand, s))) }
            };
        }

        State SimulateOutput(State s, List<byte> output)
        {
            var operand = instructions[s.IP + 1];

            output.Add((byte)(GetComboOperand(operand, s) % 8));

            if (output[^1] != instructions[output.Count - 1])
                throw new InvalidDataException();

            return s with { IP = s.IP + 2 };
        }

        int GetLiteralOperand(byte operand)
        {
            return operand;
        }

        long GetComboOperand(byte operand, State state)
        {
            if (operand < 4) return operand;

            if (operand == 4) return state.A;
            if (operand == 5) return state.B;
            if (operand == 6) return state.C;

            throw new InvalidDataException();
        }

        return new ValueTask<string>($"{mirrorValue}");
    }

    private record State(int IP, long A, long B, long C);
}