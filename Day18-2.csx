var testInstructions = new string[]
{
    "snd 1",
    "snd 2",
    "snd p",
    "rcv a",
    "rcv b",
    "rcv c",
    "rcv d"
};

System.Console.WriteLine(Part2(testInstructions) == 3);

var instructions = File.ReadAllLines("Day18-Input.txt");
System.Console.WriteLine(Part2(instructions)); // 6858

private int Part2(string[] instructions)
{
    var program0 = new Program(instructions, 0);
    var program1 = new Program(instructions, 1);

    program0.Sending = program1.Receiving = new Queue<long>();
    program0.Receiving = program1.Sending = new Queue<long>();

    do
    {
        program0.RunUntilReceiving();
        program1.RunUntilReceiving();
    }
    while (!(program0.IsFinished && program1.IsFinished) && !(program0.IsWaiting && program1.IsWaiting && program0.Receiving.Count == 0 && program1.Receiving.Count == 0));
    return program1.NumberOfSends;
}

private class Program
{
    private readonly string[] instructions;
    private readonly int programNum = 0;
    private Dictionary<char, long> registers;
    private long position = 0;

    public Program(string[] instructions, int programNum)
    {
        this.instructions = instructions;
        this.programNum = programNum;
        this.registers = new Dictionary<char, long>();
        this.registers.Add('p', programNum);
    }

    public int NumberOfSends { get; private set; }
    public bool IsFinished { get; private set; }
    public bool IsWaiting { get; private set; }
    public Queue<long> Sending { get; set; }
    public Queue<long> Receiving { get; set; }

    public void RunUntilReceiving()
    {
        IsWaiting = false;
        while (!IsWaiting && position < instructions.Length && position >= 0)
        {
            var instruction = instructions[position].Split(' ');
            var target = instruction[1][0];
            switch (instruction[0])
            {
                case "snd":
                    Sending.Enqueue(GetValue(instruction[1], registers));
                    NumberOfSends++;
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
                    if (Receiving.TryDequeue(out var value))
                    {
                        SetValue(target, value, registers);
                        position++;
                    }
                    else
                        IsWaiting = true;
                    break;
                case "jgz":
                    if (GetValue(instruction[1], registers) > 0)
                        position += GetValue(instruction[2], registers);
                    else
                        position++;
                    break;
            }
        }
        
        IsFinished = !IsWaiting;
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
}