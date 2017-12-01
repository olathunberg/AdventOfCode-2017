#! "netcoreapp2.0"

private long ReverseCaptcha(string input, bool partTwo = false)
{
    var length = input.Count();
    var step = partTwo ? length / 2 : 1;

    return input
        .Where((x, i) =>  x == input.ElementAt(i + step >= length ? (i + step) - length : i + step))
        .Sum(x => Convert.ToInt32(x.ToString()));
}

// Part 1
Console.WriteLine(ReverseCaptcha("1122")); // 3
Console.WriteLine(ReverseCaptcha("1111")); // 4
Console.WriteLine(ReverseCaptcha("1234")); // 0
Console.WriteLine(ReverseCaptcha("91212129")); // 9
    
Console.WriteLine(ReverseCaptcha(File.ReadAllText("Day01-Input.txt"))); // 1144

// Part 2
Console.WriteLine(ReverseCaptcha("1212", true)); // 6
Console.WriteLine(ReverseCaptcha("1221", true)); // 0
Console.WriteLine(ReverseCaptcha("123425", true)); // 4
Console.WriteLine(ReverseCaptcha("123123", true)); // 12
Console.WriteLine(ReverseCaptcha("12131415", true)); // 4

Console.WriteLine(ReverseCaptcha(File.ReadAllText("Day01-Input.txt"), true)); // 