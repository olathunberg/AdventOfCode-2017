//#! "netcoreapp2.0"

private long CorruptionChecksum(string[] input, Func<IEnumerable<int>, int> calculator)
{   
    return input
        .Select(x => x.Split('\t')
                      .Select(z => Convert.ToInt32(z)))
        .Select(x => calculator(x))
        .Sum();
}

private int Calculator1(IEnumerable<int> indata)
{
    return indata.Max() - indata.Min();
}

private int Calculator2(IEnumerable<int> indata)
{
    return indata
        .Select((x, i) => new
        {
            one = indata.Skip(i+1).FirstOrDefault(z => z % x == 0 || x % z == 0),
            two = x
        })
        .Where(x => x.one != 0)
        .Select(x => x.one % x.two == 0 ? x.one / x.two: x.two / x.one)
        .First();
}

// Part 1
var testInput1 = new string[]
    {
        "5	1	9	5",
        "7	5	3",
        "2	4	6	8"
    };
Console.WriteLine(CorruptionChecksum(testInput1, Calculator1)); // 18
Console.WriteLine(CorruptionChecksum(File.ReadAllLines("Day02-Input.txt"), Calculator1)); // 54426

// Part 2
var testInput2 = new string[]
    {
        "5	9	2	8",
        "9	4	7	3",
        "3	8	6	5"
    };

Console.WriteLine(CorruptionChecksum(testInput2, Calculator2)); // 9
Console.WriteLine(CorruptionChecksum(File.ReadAllLines("Day02-Input.txt"), Calculator2)); // 333
