var indata = File.ReadAllLines("Day05-Input.txt")
    .Select(x => Int32.Parse(x))
    .ToArray();

Console.WriteLine(TwistyMaze(new int[] {0, 3, 0, 1, -3}, part1Changer));  // 5
Console.WriteLine(TwistyMaze(indata, part1Changer));              // 394829

Console.WriteLine(TwistyMaze(new int[] {0, 3, 0, 1, -3}, part2Changer));   // 10
Console.WriteLine(TwistyMaze(indata, part2Changer)); // 31150702

private int part1Changer(int x) => 1;
private int part2Changer(int x) => x >= 3 ? -1 : 1;

private int TwistyMaze(int[] indata, Func<int, int> changer)
{
    var data = (int[])indata.Clone();
    var goal = indata.Count();
    var current = 0;
    var step = 0;

    while(current < goal)
    {
        step++;
        var jump = data[current];
        data[current] += changer(jump);
        current += jump;
    }
    return step;
}