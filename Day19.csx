var testInput = new string[]
{
"     |          ",
"     |  +--+    ",
"     A  |  C    ",
" F---|----E|--+ ",
"     |  |  |  D ",
"     +B-+  +--+ ",
};

System.Console.WriteLine(GetPath(testInput).characters == "ABCDEF");
System.Console.WriteLine(GetPath(testInput).steps == 38);
System.Console.WriteLine(GetPath(File.ReadAllLines("Day19-Input.txt"))); // 16312, EOCZQMURF

private (int steps, string characters) GetPath(string[] indata)
{
    (int x, int y) dir = (0, 1);
    (int x, int y) pos = (indata[0].IndexOf('|'), 0);

    var characters = new List<char>();
    var steps = 0;

    while (pos.y >= 0 && pos.y < indata.Length && pos.x >= 0 && pos.x < indata[0].Length)
    {
        if (indata[pos.y][pos.x] == ' ')
            break;
        
        if (Enumerable.Range((int)'A', (int)'Z'-(int)'A'+1).Contains(indata[pos.y][pos.x]))
            characters.Add(indata[pos.y][pos.x]);

        if (indata[pos.y][pos.x] == '+')
        {
            if (dir.y != 0)
            {
                dir.y = 0;
                if (indata[pos.y][pos.x-1] != ' ')
                    dir.x = -1;
                else if (indata[pos.y][pos.x+1] != ' ')
                    dir.x = 1;
            }
            else
            {
                dir.x = 0;
                if (indata[pos.y-1][pos.x] != ' ')
                    dir.y = -1;
                else if (indata[pos.y+1][pos.x] != ' ')
                    dir.y = 1;
            }
        }
        pos.x += dir.x;
        pos.y += dir.y;
        steps++;
    }

    return (steps, string.Join(string.Empty, characters));
}
