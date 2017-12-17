var testIndata = new string[]
{
    "0: 3",
    "1: 2",
    "4: 4",
    "6: 4"
};

// Part 1
System.Console.WriteLine(ParseIndata(testIndata).CalcSeverity() == 24);
System.Console.WriteLine(ParseIndata(File.ReadAllLines("Day13-Input.txt")).CalcSeverity()); // 2264

// Part 2
System.Console.WriteLine(CalcDelay(testIndata) == 10);
System.Console.WriteLine(CalcDelay(File.ReadAllLines("Day13-Input.txt"))); // 3875838

private static int CalcDelay(string[] indata)
{
    var fwState = ParseIndata(indata);
    var delayedState = fwState.Select(x => new State{depth = x.depth, range = x.range }).ToList();
    var delay = 0;
    while (fwState.IsCaught(delay))
    {
        delayedState.UpdateScanpos();
        for (int i = 0; i < fwState.Count(); i++)
        {
            fwState[i].scanDir = delayedState[i].scanDir;
            fwState[i].scanPos = delayedState[i].scanPos;
        }

        delay++;
    }
    return delay;
}

private static bool IsCaught(this List<State> fwState, int delay)
{
    for (int i = 0; i <= fwState.Select(x => x.depth).Max(); i++)
    {
        if (fwState.Any(x => x.depth == i && x.scanPos == 0))
            return true;

        fwState.UpdateScanpos();
    }
    return false;
}

private static int CalcSeverity(this List<State> fwState)
{
    var severity = 0;
    for (int i = 0; i <= fwState.Select(x => x.depth).Max(); i++)
    {
        var curDepth = fwState.FirstOrDefault(x => x.depth == i && x.scanPos == 0);
        if (curDepth != null) 
            severity += curDepth.depth * curDepth.range;

        fwState.UpdateScanpos();
    }

    return severity;
}

private static List<State> ParseIndata(string[] indata)
{
    return indata
        .Select(x => x.Split(new string[]{ ": " }, StringSplitOptions.None))
        .Select(x => new State { depth = Int32.Parse(x[0]), range = Int32.Parse(x[1]) })
        .ToList();
}

private static void UpdateScanpos(this List<State> fwState)
{
    foreach (var x in fwState)
    {
        if (x.scanPos == x.range-1)
            x.scanDir = -1;
        if (x.scanPos == 0)
            x.scanDir = 1;

        x.scanPos += x.scanDir;
    };
}

private class State
{
    public int depth { get; set; }
    public int range { get; set; }
    public int scanPos { get; set; }
    public int scanDir { get; set; }
}