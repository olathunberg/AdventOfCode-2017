var testdata = new string[]
{
    "0 <-> 2",
    "1 <-> 1",
    "2 <-> 0, 3, 4",
    "3 <-> 2, 4",
    "4 <-> 2, 3, 6",
    "5 <-> 6",
    "6 <-> 4, 5"
};

// Part 1
System.Console.WriteLine(GetDistinctPipes(testdata).GetConnectedPipes(0).CountPrograms() == 6);
System.Console.WriteLine(GetDistinctPipes(File.ReadAllLines("Day12-Input.txt")).GetConnectedPipes(0).CountPrograms()); // 141

// Part 2
System.Console.WriteLine(CountGroups(GetDistinctPipes(testdata)) == 2); 
System.Console.WriteLine(CountGroups(GetDistinctPipes(File.ReadAllLines("Day12-Input.txt")))); // 171

private int CountGroups(List<(int, int)> originalPipes)
{
    var groupCount = 0;
    while (originalPipes.Count > 0)
    {
        groupCount++;
        foreach (var pipe in originalPipes.GetConnectedPipes(originalPipes.First().Item1).ToArray())
            originalPipes.RemoveAll(x => x.Item1 == pipe.Item1 || x.Item2 == pipe.Item1 || x.Item1 == pipe.Item2 || x.Item2 == pipe.Item2);
    }

    return groupCount;
}

private List<(int, int)> GetDistinctPipes(string[] indata)
{
    var pipes = new List<(int, int)>();
    foreach (var line in indata)
    {
        var split = line.Split(new string[]{" <-> "}, StringSplitOptions.None);
        foreach (var pipe in split[1].Split(new string[]{", "}, StringSplitOptions.None))
        {
            var from = Int32.Parse(split[0]);
            var to = Int32.Parse(pipe);
            if (!pipes.Contains((from, to)) && !pipes.Contains((to, from)))
                pipes.Add((from, to));
        }
    }
    return pipes;
}

private static int CountPrograms(this List<(int, int)> pipes)
{
    return pipes.Select(x => x.Item1).Union(pipes.Select(x => x.Item2)).Distinct().Count();
}

private static List<(int, int)> GetConnectedPipes(this List<(int, int)> pipes, int origin)
{
    var result = pipes
        .Where(x => x.Item1 == origin || x.Item2 == origin)
        .ToList();

    result.AddRange(pipes.Where(x => (x.Item1 == origin || x.Item2 == origin) && (x.Item1 != x.Item2))
                         .SelectMany(pipe => pipes.Where(z => z.Item1 != origin && z.Item2 != origin && z.Item1 != z.Item2)
                                                  .ToList()
                                                  .GetConnectedPipes(pipe.Item1 == origin ? pipe.Item2 : pipe.Item1)));

     return result;
}
