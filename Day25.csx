using System.Text.RegularExpressions;

System.Console.WriteLine(TuringChecksum(File.ReadAllLines("Day25-TestInput.txt")) == 3);
System.Console.WriteLine(TuringChecksum(File.ReadAllLines("Day25-Input.txt"))); // 3099

private long TuringChecksum(string[] indata)
{
    var bluePrint = ParseInput(indata);
    var cursor = 0L;
    var tape = new Dictionary<long, int>();
    var currentstate = bluePrint.states[bluePrint.start];

    for (int i = 0; i < bluePrint.steps; i++)
    {
        if (!tape.ContainsKey(cursor))    
            tape.Add(cursor, 0);

        var rule = currentstate.Rules.First(x => x.TriggerValue == tape[cursor]);
        tape[cursor] = rule.WriteValue;
        cursor += (int)rule.Move;
        currentstate = bluePrint.states[rule.NextState];
    }

    return tape.Select(x => x.Value).Sum();
}

private Rule[] ParseRules(System.Collections.IEnumerator enumerator)
{
    var result = new Rule[2];
    for (int i = 0; i < result.Length; i++)
    {
        result[i] = new Rule
        {
            TriggerValue = Next<int>(enumerator, @"If the current value is (\d*):"),
            WriteValue = Next<int>(enumerator, @"Write the value (\d*)."),
            Move = Next<Move>(enumerator, @"Move one slot to the (\w*)."),
            NextState = Next<string>(enumerator, @"Continue with state (\w*)."),
        };
    }
    return result;
}
    
private (string start, int steps, Dictionary<string, State> states) ParseInput(string[] indata)
{
    var enumerator = indata.GetEnumerator();

    var start = Next<string>(enumerator, @"Begin in state (\w*).");
    var steps = Next<int>(enumerator, @"Perform a diagnostic checksum after (\d*) steps.");

    var states = new Dictionary<string, State>();
    while (enumerator.MoveNext())
    {
        var state = new State
        {
            Name = Next<string>(enumerator, @"In state (\w*):"),
            Rules = ParseRules(enumerator)
        };
        
        states.Add(state.Name, state);
    }

    return (start, steps, states);
}

private T Next<T>(System.Collections.IEnumerator enumerator, string pattern)
{
    enumerator.MoveNext();
    return RegX<T>(enumerator.Current.ToString(), pattern);
}

private T RegX<T>(string text, string pattern)
{
    var m = Regex.Match(text, pattern);
    if (m.Success)
    {
        if (typeof(T).IsEnum)
            return (T)Enum.Parse(typeof(T), m.Groups.Last().Value);
        else
            return (T)Convert.ChangeType(m.Groups.Last().Value, typeof(T));
    }
    return default(T);
}

private class State
{
    public string Name { get; set; }
    public Rule[] Rules { get; set; }
}

private class Rule
{
    public int TriggerValue { get; set; }
    public int WriteValue { get; set; }
    public Move Move { get; set; }
    public string NextState { get; set; }
}

private enum Move
{
    right = 1,
    left = -1
}