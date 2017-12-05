var indata = File.ReadAllLines("Day04-Input.txt");

Console.WriteLine(IsPassPhraseValid("aa bb cc dd ee", Part1Equality));             // true
Console.WriteLine(IsPassPhraseValid("aa bb cc dd aa", Part1Equality));             // false
Console.WriteLine(IsPassPhraseValid("aa bb cc dd aaa", Part1Equality));            // true
Console.WriteLine(indata.Where(z => IsPassPhraseValid(z, Part1Equality)).Count()); // 325

Console.WriteLine(IsPassPhraseValid("aa bb cc dd ee", Part2Equality));             // true
Console.WriteLine(IsPassPhraseValid("abcde xyz ecdab", Part2Equality));            // false
Console.WriteLine(IsPassPhraseValid("a ab abc abd abf abj", Part2Equality));       // true
Console.WriteLine(IsPassPhraseValid("iiii oiii ooii oooi oooo", Part2Equality));   // true
Console.WriteLine(IsPassPhraseValid("oiii ioii iioi iiio", Part2Equality));        // false
Console.WriteLine(indata.Where(z => IsPassPhraseValid(z, Part2Equality)).Count()); // 119

private bool IsPassPhraseValid(string passPhrase, Func<string, string> equals)
{
    var words = passPhrase.Split(' ');
    return words.Select(z => equals(z)).Distinct().Count() == words.Count();
}

private string Part1Equality(string obj)
{
    return obj;
}

private string Part2Equality(string obj)
{
   return string.Join("", obj.ToCharArray().OrderBy(x => x));
}