var allocs = Reallocations("0,2,7,0".Split(',').Select( x => Int32.Parse(x)).ToArray()); 
Console.WriteLine(allocs); // 5, 4
allocs = Reallocations("10	3	15	10	5	15	5	15	9	2	5	8	5	2	3	6".Split('\t').Select(x=>Int32.Parse(x)).ToArray());
Console.WriteLine(allocs); // 14029, 2765

private (int, int) Reallocations(int[] memoryBank)
{
    var memBank = (int[])memoryBank.Clone();
    var history = new List<int[]>();
    var counter = 0;
    
    while(true)
    {
        history.Add(memBank);
        memBank = Redistribute(memBank);
        counter++;
     
        if (history.Any(x => Enumerable.SequenceEqual(x, memBank)))
           return (counter, counter - history.IndexOf(history.First(x => Enumerable.SequenceEqual(x, memBank))));
    }
}

private int[] Redistribute(int[] memoryBank)
{
    var memBank = (int[])memoryBank.Clone();
    var toDistribute = memBank.Max();
    var index = Array.IndexOf(memBank, toDistribute);
    memBank[index] = 0;
    while(toDistribute > 0)
    {
        index++;
        if (index >= memBank.Length)
            index = 0;
        
        memBank[index]++;
        toDistribute--;
    }
    return memBank;
}
