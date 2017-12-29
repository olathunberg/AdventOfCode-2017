var testInput = new string[]
{
    "..#",
    "#..",
    "..."
};

// X ->
// Y
// |
// V

System.Console.WriteLine(Sporifica(testInput, 70, true) == 41);
System.Console.WriteLine(Sporifica(testInput, 10_000, true) == 5587);
System.Console.WriteLine(Sporifica(File.ReadAllLines("Day22-Input.txt"), 10_000, true)); // 5552

System.Console.WriteLine(Sporifica(testInput, 100, false) == 26);
System.Console.WriteLine(Sporifica(testInput, 10_000_000, false) == 2_511_944);
System.Console.WriteLine(Sporifica(File.ReadAllLines("Day22-Input.txt"), 10_000_000, false)); // 

private uint Sporifica(string[] indata, int bursts, bool part1)
{
    var grid = CreateGrid(indata);
    var carrier = new Carrier
    {
        X = grid.Select(x => x.Value.X).Max() / 2,
        Y = grid.Select(x => x.Value.Y).Max() / 2,
        FacingY = -1
    };

    for (int i = 0; i < bursts; i++)
        Burst(grid, carrier, part1);

    return carrier.Infections;
}

private void Burst(Dictionary<(int, int), Node> grid, Carrier carrier, bool part1)
{
    if (!grid.ContainsKey((carrier.X, carrier.Y)))
        grid.Add((carrier.X, carrier.Y), new Node { X = carrier.X, Y = carrier.Y });

    var currentNode = grid[(carrier.X, carrier.Y)];
    if (part1)
    {
        carrier.Turn(currentNode.IsInfected);
        carrier.Infect1(currentNode);
    }   
    else
    {
        carrier.Turn(currentNode.State);
        carrier.Infect2(currentNode);
    }   

    carrier.Move();
}

private Dictionary<(int, int), Node> CreateGrid(string[] indata)
{
    return indata.Select((x, i) => x.Select((y, j) => new Node { X = j, Y = i, IsInfected = y == '#', State = y == '#' ? NodeState.Infected : NodeState.Clean }))
        .SelectMany(x => x)
        .ToDictionary(x => (x.X, x.Y), x => x);
}

private class Carrier
{
    public int X { get; set; }
    public int Y { get; set; }
    public int FacingX { get; set; }
    public int FacingY { get; set; }
    public uint Infections { get; private set; }

    public void Infect1(Node currentNode)
    {
        if (!currentNode.IsInfected)
            Infections++;
        currentNode.IsInfected = !currentNode.IsInfected;
    }

    public void Infect2(Node currentNode)
    {
        switch (currentNode.State)
        {
            case NodeState.Clean:
                currentNode.State = NodeState.Weakened;
                break;
            case NodeState.Weakened:
                currentNode.State = NodeState.Infected;
                Infections++;
                break;
            case NodeState.Infected:
                currentNode.State = NodeState.Flagged;
                break;
            case NodeState.Flagged:
                currentNode.State = NodeState.Clean;
                break;
        }
    }

    public void Turn(bool isInfected) 
    {
        var dir = isInfected ? 1 : -1;

        if (FacingX != 0)
        {
            FacingY = dir * FacingX;    
            FacingX = 0;
        }
        else
        {
            FacingX = -1 * dir * FacingY;    
            FacingY = 0;
        }
    }

    public void Turn(NodeState state)
    {
        if (state == NodeState.Clean || state == NodeState.Infected)
            Turn(state == NodeState.Infected);    

        if (state == NodeState.Flagged)
        {
            FacingX = -FacingX;
            FacingY = -FacingY;
        }
    }

    public void Move()
    {
        X += FacingX;
        Y += FacingY;
    }
}

private class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsInfected { get; set; }
    public NodeState State { get; set; }
}

public enum NodeState
{
    Clean,
    Weakened,
    Infected,
    Flagged
}