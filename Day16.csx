var indata = File.ReadAllText("Day16-Input.txt");

var testStands = "abcde".ToArray();
System.Console.WriteLine(string.Join(string.Empty, Dance(testStands, "s1,x3/4,pe/b")) == "baedc");

// Part 1
System.Console.WriteLine(Dance("abcdefghijklmnop".ToArray(), indata)); // hmefajngplkidocb

// Part 2
System.Console.WriteLine(BillionDance("abcdefghijklmnop", indata)); // fbidepghmjklcnoa

private char[] BillionDance(string baseStands, string moves)
{
    var countUntilSame = 0;
    var stands = baseStands.ToArray();
    while (true)
    {
        stands = Dance(stands, moves);
        countUntilSame++;
        if (string.Join(string.Empty, stands) == baseStands)
            break;
    }

    for (int i = 0; i < (1_000_000_000 % countUntilSame); i++)
        stands = Dance(stands, moves);

    return stands;
}

private char[] Dance(char[] stands, string moves)
{
    foreach (var move in moves.Split(','))
    {
        switch (move[0])
        {
            case 's':
                var position = byte.Parse(string.Join(string.Empty, move.Skip(1)));
                stands = stands.Spin(position);
                break;
    
            case 'x':
                var split = string.Join(string.Empty, move.Skip(1)).Split('/');            
                var indexA = byte.Parse(split[0]);
                var indexB = byte.Parse(split[1]);
                stands = stands.Exchange(indexA, indexB);
                break;
        
            case 'p':
                stands = stands.Partner(move[1], move[3]);
                break;
        }
    } 

    return stands;
}

private static char[] Spin(this char[] stands, int index)
{
    return stands.Skip(stands.Length-index)
        .Union(stands.Take(stands.Length-index))
        .ToArray();
}

private static char[] Exchange(this char[] stands, int a, int b)
{
    var strA = stands[a];
    stands[a] = stands[b];
    stands[b] = strA;
    return stands;
}

private static char[] Partner(this char[] stands, char a, char b)
{
    int IndexOf(char idx)
    {
        for (int i = 0; i < stands.Length; i++)
        {
            if(stands[i] == idx)
                return i;
        }

        return -1;
    }
    var idxA = IndexOf(a);
    var idxB = IndexOf(b);
    stands[idxA] = b;
    stands[idxB] = a;

    return stands;
}