var startValue = ".#./..#/###";

var testRules = ParseRules(new string[]
{
    "../.# => ##./#../...",
    ".#./..#/### => #..#/..../..../#..#"
});
var rules = ParseRules(File.ReadAllLines("Day21-Input.txt"));

System.Console.WriteLine(Iterate(startValue, testRules, 2).Count(x => x == '#') == 12);
System.Console.WriteLine(Iterate(startValue, rules, 5).Count(x => x == '#')); // 171
System.Console.WriteLine(Iterate(startValue, rules, 18).Count(x => x == '#')); // 2498142

private string Iterate(string startValue, List<(string rule, string enhancement)> rules, int iterations)
{
    var value = startValue;
    for (int i = 0; i < iterations; i++)
    {
        var divided = Divide(value);
        for (int j = 0; j < divided.Length; j++)
            divided[j] = ApplyRules(divided[j], rules);
        value = Join(divided);
    }

    return value;
}

private List<(string rule, string enhancement)> ParseRules(string[] rules) => rules.Select(x => x.Split(new string[]{" => "}, StringSplitOptions.None))
                                                                                   .Select(z => (z[0], z[1]))
                                                                                   .ToList();

private string ApplyRules(string indata, List<(string rule, string enhancement)> rules)
{
    var rotations = Rotations(indata).ToList();
    return rules.First(x => rotations.Contains(x.rule)).enhancement;
}

private string[] Divide(string indata)
{
    var parts = indata.Split('/');
    var divideBy = parts.Length % 2 == 0 ? 2 : 3;
    var divided = new List<string>();

    for (int i = 0; i < parts.Length / divideBy; i++)
        for (int j = 0; j < parts.Length / divideBy; j++)
            divided.Add(string.Join('/', parts.Skip(i*divideBy)
                                              .Take(divideBy)
                                              .Select(x => x.Substring(j*divideBy, divideBy))));

    return divided.ToArray();
}

private string Join(string[] indata)
{
    if(indata.Length == 1)
        return indata[0];

    var size = (int)Math.Sqrt(indata.Length);
    var parts = indata.Select(x => x.Split('/').ToArray()).ToArray();
    var joined = new List<string>();

    for (int i = 0; i < size ; i++)
        for (int j = 0; j < parts[0].Length ; j++)
            joined.Add(string.Join(string.Empty, parts.Skip(i * size).Take(size).Select(x => x.Skip(j).First())));    

    return string.Join('/', joined);
}

private IEnumerable<string> Rotations(string indata)
{
    yield return indata;
    
    // Flip vertically
    var parts = indata.Split('/');
    Array.Reverse(parts);
    yield return string.Join('/', parts);

    // Rotate flipped
    parts = Rotate90(parts);
    yield return string.Join('/', parts);    
    parts = Rotate90(parts);
    yield return string.Join('/', parts);    
    parts = Rotate90(parts);
    yield return string.Join('/', parts);    
    
    // Flip horizontally
    parts = indata.Split('/');
    yield return string.Join('/', parts.Select(x => {
                                                        var arr = x.ToCharArray(); 
                                                        Array.Reverse(arr); 
                                                        return string.Join(string.Empty, arr);
                                                    }));
    // Rotate original
    parts = indata.Split('/');
    parts = Rotate90(parts);
    yield return string.Join('/', parts);    
    parts = Rotate90(parts);
    yield return string.Join('/', parts);    
    parts = Rotate90(parts);
    yield return string.Join('/', parts);    
}

private string[] Rotate90(string[] indata)
{
    var size = indata.Length;
    var dest = indata.Select(x => x.ToCharArray()).ToArray();

    for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++)
            dest[x][y] = indata[size - y - 1][x];
    return dest.Select(x => string.Join(string.Empty, x)).ToArray();
}