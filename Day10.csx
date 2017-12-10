var indataTest = new int[] { 3,4,1,5 };
System.Console.WriteLine(KnotHash1(indataTest, 5) == 12);

// Part 1
var indata = new int[] {189,1,111,246,254,2,0,120,215,93,255,50,84,15,94,62};
System.Console.WriteLine(KnotHash1(indata, 256)); // 38415

System.Console.WriteLine(DenseHash(SparseHash("")) == "a2582a3a0e66e6e86e3812dcb672a272");
System.Console.WriteLine(DenseHash(SparseHash("AoC 2017")) == "33efeb34ea91902bb2f59c9920caa6cd");
System.Console.WriteLine(DenseHash(SparseHash("1,2,3")) == "3efbe78a8d82f29979031a4aa0b16a9d");
System.Console.WriteLine(DenseHash(SparseHash("1,2,4")) == "63960835bcdc130f0b66d7ff4f6a5a8e");

// Part 2
System.Console.WriteLine(DenseHash(SparseHash("189,1,111,246,254,2,0,120,215,93,255,50,84,15,94,62")));

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