System.Console.WriteLine(HexToBitString("a0c2017") == "1010000011000010000000010111");

// Part 1
System.Console.WriteLine(CountUsedSpaces("flqrgnkx") == 8108);
System.Console.WriteLine(CountUsedSpaces("oundnydw")); // 8106

// Part 2 1164
System.Console.WriteLine(GroupCount("flqrgnkx") == 1242);
System.Console.WriteLine(GroupCount("oundnydw")); // 1164

private int GroupCount(string indata)
{
    var data = Enumerable.Range(0, 128)
        .Select(x => $"{indata}-{x}")
        .Select(x => DenseHash(SparseHash(x)))
        .Select(x => HexToBitString(x))
        .SelectMany((x, row) => x.Select((z, col) => new Space { State = z == '1', Row = row, Column = col }))
        .ToList();

    var maxGroup = 0;
    foreach (var space in data.Where(x => x.State && x.Group == 0))
    {
        space.Group = ++maxGroup;
        SetAllAdjacentGroup(data, space);
    }

    return maxGroup;
}

private void SetAllAdjacentGroup(List<Space> spaces, Space origin)
{
    foreach (var dir in new (int row, int col)[] {(-1,0), (1,0), (0,-1), (0,1) })
        foreach (var space in spaces.Where(x => x.Row == origin.Row + dir.row && x.Column == origin.Column + dir.col && x.State && x.Group == 0))
        {
            space.Group = origin.Group;
            SetAllAdjacentGroup(spaces, space);
        }
}

private class Space
{
    public int Row {get; set;}
    public int Column {get; set;}
    public bool State {get; set;}
    public int Group {get; set;}
}

private int CountUsedSpaces(string indata)
{
    return Enumerable.Range(0, 128)
        .Select(x => $"{indata}-{x}")
        .Select(x => DenseHash(SparseHash(x)))
        .Select(x => HexToBitString(x))
        .Select(x => x.Where(z => z == '1').Count())
        .Sum();
}

private string HexToBitString(string hexIndata)
{
    return string.Join(String.Empty,
            hexIndata.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
}

// Knot Hash from Day10
private string DenseHash(int[] sparseHash)
{
    var dense = Enumerable.Range(0, 16)
        .Select(i => sparseHash
                        .Skip(i*16)
                        .Take(16)
                        .Aggregate((x, z) => x ^ z));
    
    return string.Join("", dense.Select(x => x.ToString("x2")));
}

private int[] SparseHash(string indata)
{
    var current = 0;
    var skipSize = 0;
    var list = Enumerable.Range(0, 256).ToArray();
    var lengths = GetCodes(indata);

    for (int i = 0; i < 64; i++)
        (list, current, skipSize) = KnotHash(lengths, list, current, skipSize);
 
    return list;
}

private int[] GetCodes(string indata)
{
    var stdSuffix = new int[] { 17, 31, 73, 47, 23 };
    var result = indata
        .Select(x => (int)x)
        .ToList();
    result.AddRange(stdSuffix);
    return result.ToArray();
}

private int KnotHash1(int[] lengths, int size)
{ 
    var list = Enumerable.Range(0, size).ToArray();
    var hash = KnotHash(lengths, list, 0, 0);

    return hash.list[0] *  hash.list[1];
}

private (int[] list, int current, int skipSize) KnotHash(int[] lengths, int[] list, int startCurrent, int startSkipSize)
{
    var current = startCurrent;
    var skipSize = startSkipSize;

    foreach (var length in lengths)
    {
        ReverseSubList(list, current, length);

        current += length + skipSize;
        while (current > list.Length)
            current = current - list.Length;
        skipSize++;
    }

    return (list, current, skipSize);
}

private void ReverseSubList(int[] list, int current, int length)
{
    var revSubList = new int[length];
    
    for (int  i = current, j = 0;  j < length;  i++, j++)
    {
        var index = i > list.Length - 1 ? i - list.Length : i;
        revSubList[length-j-1] = list[index];
    }

    for (int  i = current, j = 0;  j < length;  i++, j++)
    {
        var index = i > list.Length - 1 ? i - list.Length : i;
        list[index] = revSubList[j];
    }
}