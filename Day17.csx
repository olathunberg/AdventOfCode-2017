System.Console.WriteLine(Part1(SpinLock(3, 2017)) == 638);

// Part 1
System.Console.WriteLine(Part1(SpinLock(337, 2017))); // 600

// Part 2
System.Console.WriteLine(Part2(SpinLock(337, 50000000))); // 31220910

private List<uint> SpinLock(uint steps, uint inserts)
{
    var list = new LinkedList<uint>();
    var current = list.AddFirst(0);

    for (uint i = 1; i <= inserts; i++)
    {
        for (uint j = 0; j < steps; j++)
        {
            current = current.Next;
            if(current == null)
                current = list.First;
        }
        current = list.AddAfter(current, i);
    }

    return list.ToList();
}

private uint Part1(List<uint> list)
{
    return list[list.IndexOf(2017)+1];
}

private uint Part2(List<uint> list)
{
    return list[list.IndexOf(0)+1];
}