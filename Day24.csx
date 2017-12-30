var testInput = new string[]
{
    "0/2",
    "2/2",
    "2/3",
    "3/4",
    "3/5",
    "0/1",
    "10/1",
    "9/10"
};

// Part 1
System.Console.WriteLine(Part1(testInput) == 31);
System.Console.WriteLine(Part1(File.ReadAllLines("Day24-Input.txt"))); // 1906

System.Console.WriteLine(Part2(testInput) == 19);
System.Console.WriteLine(Part2(File.ReadAllLines("Day24-Input.txt"))); // 1824

private long Part1(string[] indata)
{
    return GetComponents(indata, new List<string>(), "0")
        .Select(x => x.CalcStrength())
        .Max();
}

private long Part2(string[] indata)
{
    return GetComponents(indata, new List<string>(), "0")
        .OrderByDescending(x => x.Length)
        .ThenByDescending(x => x.CalcStrength())
        .First()
        .CalcStrength();
}

private IEnumerable<string[]> GetComponents(string[] indata, List<string> trace, string portType)
{
    foreach (var item in indata.Where(x => x.Split('/').Any(z => z == portType)))
    {
        var newTrace = new List<string>(trace);
        newTrace.Add(item);
        foreach (var part in GetComponents(indata.Where(x => x != item).ToArray(), newTrace, item.Split('/').Where(x => x != portType).FirstOrDefault() ?? portType))
            yield return part;
    }
    yield return trace.ToArray();
}

private static long CalcStrength(this string[] indata) => indata.Select(x => x.Split('/')
                                                                              .Select(z => Int32.Parse(z))
                                                                              .Sum())
                                                                .Sum();
