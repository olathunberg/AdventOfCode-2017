// var indata = new string[]
// {
//     "b inc 5 if a > 1",
//     "a inc 1 if b < 5",
//     "c dec -10 if a >= 1",
//     "c inc -20 if c == 10"
// };

var indata = File.ReadAllLines("Day08-Input.txt");

var registers = new Dictionary<string, (int value, int max)>();
foreach (var instr in indata.Select(x => new JumpInstruction(x)))
{
    if (!registers.ContainsKey(instr.Register))
        registers.Add(instr.Register, (0,0));

    if (Eval(instr, registers))
    {
        var val = registers[instr.Register];
        switch (instr.Instruction)
        {
            case "inc":
                val.value = (val.value + instr.Value);
                break;
            case "dec":
                val.value = (val.value - instr.Value);
                break;
        }
        if (val.max < val.value)
            val.max = val.value;
        
        registers[instr.Register] = val;
    }
}

// Part 1
System.Console.WriteLine(registers.OrderByDescending( x => x.Value.value).First());

// Part 2
System.Console.WriteLine(registers.OrderByDescending( x => x.Value.max).First());

private bool Eval(JumpInstruction instr, Dictionary<string, (int, int)> registers)
{
    var regValue = 0;
    if (registers.ContainsKey(instr.ConditionRegister))
        regValue = registers[instr.ConditionRegister].Item1;
 
    switch (instr.ConditionOperator)
    {
        case ">":
            return regValue > instr.ConditionValue;
        case "<":
            return regValue < instr.ConditionValue;
        case ">=":
            return regValue >= instr.ConditionValue;
        case "<=":
            return regValue <= instr.ConditionValue;
        case "==":
            return regValue == instr.ConditionValue;
        case "!=":
            return regValue != instr.ConditionValue;
    }

    return false;
}

private class JumpInstruction
{
    public string Register { get; set; }
    public string Instruction { get; set; }
    public int Value { get; set; }
    public string ConditionRegister { get; set; }
    public string ConditionOperator { get; set; }
    public int ConditionValue { get; set; }

    public JumpInstruction(string line)
    {
        var values = line.Split(' ' );

        Register = values[0];
        Instruction = values[1];
        Value = Int32.Parse(values[2]);
        ConditionRegister = values[4];
        ConditionOperator = values[5];
        ConditionValue = Int32.Parse(values[6]);
    }
}