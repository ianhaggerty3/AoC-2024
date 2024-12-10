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
        Dictionary<int, List<(long, long)>> files = new();
        List<(long, long)> freeSpaces = new();

        var address = 0;
        var i = 0;
        foreach (var token in _input)
        {
            var length = int.Parse([token]);
            if (i % 2 == 0)
                files.Add(i / 2, [(address, address + length)]);
            else
                freeSpaces.Add((address, address + length));
            address += length;
            i++;
        }

        var fileIndex = files.Keys.Max();
        var currentFile = files[fileIndex][0];
        while (freeSpaces.Count != 0)
        {
            var currentFreeSpace = freeSpaces[0];
            freeSpaces.RemoveAt(0);

            while (GetLength(currentFreeSpace) != 0 && currentFreeSpace.Item1 < currentFile.Item1)
            {
                var freeSpaceLength = GetLength(currentFreeSpace);
                var fileLength = GetLength(currentFile);

                if (fileLength <= freeSpaceLength)
                {
                    files[fileIndex].Add((currentFreeSpace.Item1, currentFreeSpace.Item1 + fileLength));
                    currentFreeSpace = (currentFreeSpace.Item1 + fileLength, currentFreeSpace.Item2);
                    currentFile = (0, 0);
                }
                else
                {
                    files[fileIndex].Add((currentFreeSpace.Item1, currentFreeSpace.Item2));
                    currentFreeSpace = (0, 0);
                    currentFile = (currentFile.Item1, currentFile.Item2 - freeSpaceLength);
                }

                files[fileIndex][0] = currentFile;
                if (GetLength(currentFile) == 0)
                {
                    fileIndex--;
                    currentFile = files[fileIndex][0];
                }
            }
        }

        long GetLength((long, long) block)
        {
            return block.Item2 - block.Item1;
        }

        long total = 0;
        foreach (var (key, value) in files)
        foreach (var block in value.Where(v => GetLength(v) != 0))
        {
            var toM = block.Item2 * (block.Item2 - 1) / 2;
            var toN = block.Item1 * (block.Item1 - 1) / 2;
            var addition = key * (toM - toN);
            total += addition;
        }

        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, (long, long)> files = new();
        List<(long, long)?> freeSpaces = new();

        var address = 0;
        var i = 0;
        foreach (var token in _input)
        {
            var length = int.Parse([token]);
            if (i % 2 == 0)
                files.Add(i / 2, (address, address + length));
            else
                freeSpaces.Add((address, address + length));
            address += length;
            i++;
        }

        foreach (var (fileIndex, file) in files.OrderBy(pair => pair.Key).Reverse())
        {
            var freeSpace =
                freeSpaces.FirstOrDefault(s => s.Value.Item1 < file.Item1 && GetLength(s.Value!) >= GetLength(file));
            if (freeSpace is not null)
            {
                var index = freeSpaces.IndexOf(freeSpace);
                freeSpaces[index] = ((freeSpace.Value.Item1 + GetLength(file), freeSpace.Value.Item2));
                files[fileIndex] = (freeSpace.Value.Item1, freeSpace.Value.Item1 + GetLength(file));
            }
        }
        
        long GetLength((long, long) block)
        {
            return block.Item2 - block.Item1;
        }

        long total = 0;
        foreach (var (key, value) in files)
        {
            var toM = value.Item2 * (value.Item2 - 1) / 2;
            var toN = value.Item1 * (value.Item1 - 1) / 2;
            var addition = key * (toM - toN);
            total += addition;
        }

        return new ValueTask<string>($"{total}");
    }
}