System.Console.WriteLine(string.Concat(CleanGarbage("<{oi!a,<{i<a>"))); // empty
System.Console.WriteLine(string.Concat(CleanGarbage("{{<!>},{<!>},{<!>},{<a>}}"))); // {{}}

System.Console.WriteLine(Part1("{}")); // 1
System.Console.WriteLine(Part1("{{{}}}")); // 6
System.Console.WriteLine(Part1("{{},{}}")); // 5
System.Console.WriteLine(Part1("{{{},{},{{}}}}")); // 16
System.Console.WriteLine(Part1("{<{},{},{{}}>}")); // 1
System.Console.WriteLine(Part1("{{<ab>},{<ab>},{<ab>},{<ab>}}")); // 9
System.Console.WriteLine(Part1("{{<!!>},{<!!>},{<!!>},{<!!>}}")); // 9
System.Console.WriteLine(Part1("{{<a!>},{<a!>},{<a!>},{<ab>}}")); // 3

System.Console.WriteLine(Part1(File.ReadAllText("Day09-Input.txt"))); // 3

// Part 2
System.Console.WriteLine(CalcGarbage("<!!!>>")); // 0
System.Console.WriteLine(CalcGarbage("<random characters>")); // 17
System.Console.WriteLine(CalcGarbage(File.ReadAllText("Day09-Input.txt"))); // 17

private int Part1(string indata)
{
    var cleaned = string
        .Concat(CleanGarbage(indata));
    var grouped = GetGroups(cleaned);
    
    return grouped
        .Select(x => CalcGroup(x))
        .Sum();
}

private IEnumerable<string> GetGroups(string indata)
{
    var count = 0;
    var starti = 0;
    for (var i = 0; i < indata.Length; i++)
    {
        switch (indata[i])
        {
            case '{':
                count++;
                break;
            case '}':
                count--;
                break;  
        }

        if (count == 0)
        {
            yield return indata.Substring(starti, i - starti + 1);
            starti = i;
        }
    }
}

private int CalcGroup(string group)
{
    var count = 0;
    var sum = 0;

    for (var i = 0; i < group.Length; i++)
    {
        switch (group[i])
        {
            case '{':
                count++;
                break;
            case '}':
                sum += count;
                count--;
                break;
        }

    }

    return sum;
}

private IEnumerable<char> CleanGarbage(string indata)
{
    var isInGarbage = false;

    for (var i = 0; i < indata.Length; i++)
    {
        switch (indata[i])
        {
            case '!':
                i += 1;
                break;
            case '<' when !isInGarbage:
                isInGarbage = true;
                break;
            case '>' when isInGarbage:
                isInGarbage = false;
                i++;
                break;                
        }
        if (!isInGarbage && i < indata.Length)
            yield return indata[i];
    }
}

private int CalcGarbage(string indata)
{
    var isInGarbage = false;
    var sum = 0;

    for (var i = 0; i < indata.Length; i++)
    {
        switch (indata[i])
        {
            case '!':
                i += 1;
                continue;
            case '<' when !isInGarbage:
                isInGarbage = true;
                continue;
            case '>' when isInGarbage:
                isInGarbage = false;
                continue;                
        }
        if (isInGarbage)
            sum++;
    }

    return sum;
}