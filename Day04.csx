var indata = File.ReadAllLines("Day04-Input.txt");

Console.WriteLine(new PassPhrase("aa bb cc dd ee", Part1Equality).IsValid);  // true
Console.WriteLine(new PassPhrase("aa bb cc dd aa", Part1Equality).IsValid);  // false
Console.WriteLine(new PassPhrase("aa bb cc dd aaa", Part1Equality).IsValid); // true
Console.WriteLine(indata.Select(z => new PassPhrase(z, Part1Equality)).Count(x => x.IsValid)); // 325

Console.WriteLine(new PassPhrase("aa bb cc dd ee", Part2Equality).IsValid);             // true
Console.WriteLine(new PassPhrase("abcde xyz ecdab", Part2Equality).IsValid);            // false
Console.WriteLine(new PassPhrase("a ab abc abd abf abj", Part2Equality).IsValid);       // true
Console.WriteLine(new PassPhrase("iiii oiii ooii oooi oooo", Part2Equality).IsValid);   // true
Console.WriteLine(new PassPhrase("oiii ioii iioi iiio", Part2Equality).IsValid);        // false
Console.WriteLine(indata.Select(z => new PassPhrase(z, Part2Equality)).Count(x => x.IsValid)); // 119

private string Part1Equality(string obj)
{
    return obj;
}

private string Part2Equality(string obj)
{
   return string.Join("", obj.ToCharArray().OrderBy(x => x));
}

private class PassPhrase
{
    private readonly PassWord[] passWords;
    public PassPhrase(string words, Func<string, string> equals)
    {
        this.passWords = words.Split(' ').Select(z => new PassWord(z, equals)).ToArray();
    }
    public bool IsValid => passWords.Distinct().Count() == passWords.Count();
}

private class PassWord
{
    private readonly string equalRepresentation;
    public PassWord(string word, Func<string, string> equalRepresentation)
    {
        this.equalRepresentation = equalRepresentation(word);
    }
    public override bool Equals(object obj)
    {
        if(obj == null)
            return false;
        
        return this.equalRepresentation.Equals(((PassWord)obj).equalRepresentation);
    }
    public override int GetHashCode()
    {
        return equalRepresentation.GetHashCode();
    }
}