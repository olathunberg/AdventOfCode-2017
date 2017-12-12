System.Console.WriteLine(StepsHome("ne,ne,ne") == 3);
System.Console.WriteLine(StepsHome("ne,ne,sw,sw") == 0);
System.Console.WriteLine(StepsHome("ne,ne,s,s") == 2);
System.Console.WriteLine(StepsHome("se,sw,se,sw,sw") == 3);

System.Console.WriteLine(CalcStepsHome((1,3)));
System.Console.WriteLine(CalcStepsHome((0,3)));
System.Console.WriteLine(CalcStepsHome((2,4)));

// Part 1
System.Console.WriteLine(StepsHome(File.ReadAllText("Day11-Input.txt"))); // 682

// Part 2
System.Console.WriteLine(MaxStepsHome(File.ReadAllText("Day11-Input.txt"))); // 1406

private int StepsHome(string input)
{
    (int x, int y) position = (0, 0);
    foreach (var step in input.Split(','))
        TakeStep(step, ref position);
    
    return CalcStepsHome(position);
}

private int MaxStepsHome(string input)
{
    (int x, int y) position = (0, 0);
    int maxStepsHome = 0;
    foreach (var step in input.Split(','))
    {
        TakeStep(step, ref position);

        var stepsHome = CalcStepsHome(position);
        if (stepsHome > maxStepsHome)
            maxStepsHome = stepsHome;
    }

    return maxStepsHome;
}

//    \0,-1/
// -1,0+--+ -1,1
//    /    \
//  -+  0,0 +-
//    \    /
// 1,-1+--+ 1,0
//    /0,1 \
private void TakeStep(string step, ref (int x, int y) position)
{
    switch (step)
    {
        case "n":
            position.y += 1;
            break;
        case "ne":
            position.y += 1;
            position.x += 1;
            break;
        case "se":
            position.x += 1;
            break;
        case "s":
            position.y -= 1;
            break;
        case "sw":
            position.y -= 1;
            position.x -= 1;
            break;
        case "nw":
            position.x -= 1;
            break;
    }
}

private int CalcStepsHome((int x, int y) position)
{
    // Manhattan distance in hexgrid
    if (Math.Sign(position.x) != Math.Sign(position.y))
        return Math.Abs(position.x + position.y);
    else
        return Math.Max(Math.Abs(position.x), Math.Abs(position.y));
}