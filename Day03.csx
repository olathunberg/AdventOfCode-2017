//#! "netcoreapp2.0"

private IEnumerable<(int value, int x, int y)> SpiralMemory(bool calc)
{
    (int x, int y) curr = (0, 0);
    (int x, int y) min = (0, 0);
    (int x, int y) max = (0, 0);
    (int x, int y) dir = (1, 0);
    var values = new List<(int value, int x, int y)>();

    while(true)
    {
        if(values.Count() == 0)
            values.Add((1, 0, 0));
           
        yield return values.Last();
          
        curr.x += dir.x;
        curr.y += dir.y;

        var value = calc
            ? values.Where(x => x.x >= curr.x-1 && x.x <= curr.x+1 && x.y >= curr.y-1 && x.y <= curr.y+1).Sum(x => x.value)
            : values.Last().value + 1;

        values.Add((value, curr.x, curr.y));

        if(dir.x == 1 && curr.x > max.x)
        {
            max.x = curr.x;
            dir = (0, -1);
        }
        else if(dir.x == -1 && curr.x < min.x)
        {
            min.x = curr.x;
            dir = (0, 1);
        }
        else if(dir.y == 1 && curr.y > max.y)
        {
            max.y = curr.y;
            dir = (1, 0);
        }
        else if(dir.y == -1 && curr.y < min.y)
        {
            min.y = curr.y;
            dir = (-1, 0);
        }
    }
 }

 private long Part1(int pos)
 {
    var grid = SpiralMemory(false).Skip(pos - 1).Take(1);         
    return Math.Abs(grid.Last().Item2) + Math.Abs(grid.Last().Item3);
 }

private long Part2(int pos)
{
    return SpiralMemory(true).Where(x => x.value > pos).First().value;  
}

// Part 1
Console.WriteLine(Part1(1)); // 0
Console.WriteLine(Part1(12)); // 3
Console.WriteLine(Part1(23)); // 2
Console.WriteLine(Part1(1024)); // 31

Console.WriteLine(Part1(277678)); // 475

// Part 2
Console.WriteLine(Part2(361)); // 362
Console.WriteLine(Part2(121)); // 122

Console.WriteLine(Part2(277678)); // 279138
