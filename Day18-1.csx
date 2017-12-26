var testInstructions = new string[]
{
    "set a 1",
    "add a 2",
    "mul a a",
    "mod a 5",
    "snd a",
    "set a 0",
    "rcv a",
    "jgz a -1",
    "set a 1",
    "jgz a -2"
};

System.Console.WriteLine(Part1(testInstructions) == 4);
System.Console.WriteLine(Part1(File.ReadAllLines("Day18-Input.txt")) == 4601);

private long Part1(string[] instructions)
{
    var registers = new Dictionary<char, long>();
    var position = 0L;
    var lastPlayedFreq = 0L;

    while (position < instructions.Length && position >= 0)
    {
        var instruction = instructions[position].Split(' ');
        var target = instruction[1][0];
        switch (instruction[0])
        {
            case "snd":
                lastPlayedFreq = GetValue(instruction[1], registers);
                position++;
                break;
            case "set": 
                SetValue(target, GetValue(instruction[2], registers), registers);
                position++;
                break;
            case "add": 
                SetValue(target, GetValue(instruction[1], registers) + GetValue(instruction[2], registers), registers);
                position++;
                break;
            case "mul": 
                SetValue(target, GetValue(instruction[1], registers) * GetValue(instruction[2], registers), registers);
                position++;
                break;
            case "mod": 
                SetValue(target, GetValue(instruction[1], registers) % GetValue(instruction[2], registers), registers);
                position++;
                break;
            case "rcv": 
                var value = GetValue(instruction[1], registers);
                if (value > 0)
                    return lastPlayedFreq;      
                position++;
                break;
            case "jgz": 
                if (GetValue(instruction[1], registers) > 0)
                    position += GetValue(instruction[2], registers);            
                else
                    position++;
                break;
        }
    }

    return 0;
}

private long GetValue(string str, Dictionary<char, long> registers)
{
    if (Int32.TryParse(str, out var value))
        return value;

    if (!registers.ContainsKey(str[0]))
        registers.Add(str[0], 0);

    return registers[str[0]];
}

private void SetValue(char register, long value, Dictionary<char, long> registers)
{
    if (registers.ContainsKey(register))
        registers[register] = value;
    else
        registers.Add(register, value);
}