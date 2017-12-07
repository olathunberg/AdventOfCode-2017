// var indata = File.ReadAllLines("Day07-TestInput.txt");
var indata = File.ReadAllLines("Day07-Input.txt");
        
var programs = indata
    .Select(x => x.Split(new string[]{" -> "}, StringSplitOptions.None))
    .Select(x => new ProgramDefinition
    {
        Name = x.First().Split(' ').First(),
        Weight =  Int32.Parse(x.First().Split(' ').Last().TrimStart('(').TrimEnd(')')),
        Children = x.Length > 1 ? x.Last() : null,
        SumWeight = x.Length > 1 ? 0 : Int32.Parse(x.First().Split(' ').Last().TrimStart('(').TrimEnd(')'))
    })
    .ToList();

    programs
        .Where(x => x.Children != null).ToList()
        .ForEach(program => 
            {
                foreach (var child in program.Children.Split(new string[]{", "}, StringSplitOptions.None))
                    programs.First(x => x.Name == child).Parent = program;
            });

    var root = programs
        .Where(x => x.Parent == null)
        .First();

// Part 1
System.Console.WriteLine(root.Name);  // dtacyn

// Part 2
CalculateSumWeight(root, programs);

var unbalancedNodes = programs
    .GroupBy(x => x.Parent)
    .Where(x => x.Select(z => z.SumWeight).Distinct().Count() > 1)
    .OrderBy(x => x.Select(z => z.SumWeight).Sum())
    .First()
    .ToList();

var balance = unbalancedNodes
    .GroupBy(z => z.SumWeight)
    .Where(x => x.Count() == 1)
    .First()
    .Select(x => x.Weight)
    .First();
var diff = (unbalancedNodes.Max(x => x.SumWeight) - unbalancedNodes.Min(x => x.SumWeight));

System.Console.WriteLine(balance - diff); // 521

private int CalculateSumWeight(ProgramDefinition program, List<ProgramDefinition> programs)
{
    var children = programs.Where(x => x.Parent == program).ToList();
    if (children.Count() == 0)
        return program.SumWeight;

    program.SumWeight = program.Weight + 
        programs.Where(x => x.Parent == program)
            .Sum(x => CalculateSumWeight(x, programs));

    return program.SumWeight;
}

private class ProgramDefinition
{
    public string Name { get; set; }
    public int Weight { get; set; }    
    public int SumWeight { get; set; }
    public string Children {get; set;}  
    public ProgramDefinition Parent {get; set;}   
}