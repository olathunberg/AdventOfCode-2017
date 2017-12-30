System.Console.WriteLine(Optimized());

private long Translated()
{
    long f, h = 0L;

    for (var b = 108400; b != 108400+17000; b += 17)
    {
        f = 1;
        for (var d = 2; d != b; d++)
            for (var e = 2; e != b; e++)
            {
                // If b can be formed from d and e
                // It's not a prime!
                if (d*e == b)
                    f = 0;
            }
        if (f == 0)
            h++;
    }
    return h;
}

private long Optimized()
{
    bool isPrime(int num)
    {
        var s = Math.Sqrt(num);
        for (var i = 2; i <= s; i++)
            if (num % i == 0) return false; 
        return num != 1;
    }

    var h = 0L;
    for (var b = 108400; b <= 108400+17000; b += 17) 
    { 
        if (!isPrime(b)) 
            h++;
    }
    return h;
}
