System.Console.WriteLine(Generate(16807, 65, 1).Take(5).Last() == 1352636452);
System.Console.WriteLine(Generate(48271, 8921, 1).Take(5).Last() == 285222916);
System.Console.WriteLine(Generate(16807, 65, 4).Take(5).Last() == 740335192);
System.Console.WriteLine(Generate(48271, 8921, 8).Take(5).Last() == 412269392);

// Part 1
System.Console.WriteLine(CountMatches(16807, 65, 48271, 8921, 1, 1, 5) == 1);
System.Console.WriteLine(CountMatches(16807, 65, 48271, 8921, 1, 1, 40_000_000) == 588);
System.Console.WriteLine(CountMatches(16807, 116, 48271, 299, 1, 1, 40_000_000)); // 569

// Part 2
System.Console.WriteLine(CountMatches(16807, 65, 48271, 8921, 4, 8, 1055) == 0);
System.Console.WriteLine(CountMatches(16807, 65, 48271, 8921, 4, 8, 1056) == 1);
System.Console.WriteLine(CountMatches(16807, 65, 48271, 8921, 4, 8, 5_000_000) == 309);
System.Console.WriteLine(CountMatches(16807, 116, 48271, 299, 4, 8, 5_000_000)); // 298

private int CountMatches(uint factorA, uint startA, uint factorB, uint startB, uint dividorA, uint dividorB, uint count)
{
    var generatorA = Generate(factorA, startA, dividorA).GetEnumerator();
    var generatorB = Generate(factorB, startB, dividorB).GetEnumerator();
    var matches = 0;

    for (ulong i = 0; i < count; i++)
    {
        generatorA.MoveNext();
        generatorB.MoveNext();

        if (Match(generatorA.Current, generatorB.Current))
            matches++;
    }

    return matches;
}

private bool Match(uint valueA, uint valueB)
{
    return (valueA & (uint)65535) == (valueB & (uint)65535);
}

private IEnumerable<uint> Generate(uint factor, uint startValue, uint dividor = 1)
{
    ulong value = (ulong)startValue;
    while (true)
    {
        value = (value * factor) % (ulong)2147483647;
        while (value % dividor != 0)
            value = (value * factor) % (ulong)2147483647;
            
        yield return (uint)value;
    }
}