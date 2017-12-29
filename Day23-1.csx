System.Console.WriteLine(Conflagration (File.ReadAllLines("Day23-Input.txt"))); // 6724

private long Conflagration (string[] instructions)
{
    var registers = Enumerable.Range('a', 'h')
        .ToDictionary(x => (char)x, x => 0L);

    var position = 0L;
    var multiplications = 0L;

    while (position >= 0 && position < instructions.Length)
    {
        var statment = instructions[position].Split(' ');
        switch (statment[0])
        {
            case "set": 
                SetValue(statment[1][0], GetValue(statment[2], registers), registers);
                position++;
                break;
            case "sub": 
                SetValue(statment[1][0], GetValue(statment[1], registers) - GetValue(statment[2], registers), registers);
                position++;
                break;
            case "mul": 
                SetValue(statment[1][0], GetValue(statment[1], registers) * GetValue(statment[2], registers), registers);
                position++;
                multiplications++;
                break;
            case "jnz": 
                if (GetValue(statment[1], registers) != 0)
                    position += GetValue(statment[2], registers);            
                else
                    position++;
                break;
        }
    }

    return multiplications;
}

private long GetValue(string str, Dictionary<char, long> registers)
{
    if (Int32.TryParse(str, out var value))
        return value;

    return registers[str[0]];
}

private void SetValue(char register, long value, Dictionary<char, long> registers)
{
    registers[register] = value;
}