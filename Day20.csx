var testInput = new string[]
{
    "p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>",
    "p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>"
};

// Part 1
System.Console.WriteLine(GetClosestIndex(testInput) == 1);
System.Console.WriteLine(GetClosestIndex(File.ReadAllLines("Day20-Input.txt"))); // 119

// Part 2
System.Console.WriteLine(Swarm(File.ReadAllLines("Day20-Input.txt"))); // 471

private int GetClosestIndex(string[] indata)
{
    var particles = new List<Particle>();
    foreach (var particle in indata.Select((x, i) => (x, i)))
        particles.Add(new Particle(particle.Item1, particle.Item2));

    for (int i = 0; i < 1000; i++)
    {
        foreach (var particle in particles)
            particle.Update();
    }

    return particles.OrderBy(x => x.Distance).First().Index;
}

private int Swarm(string[] indata)
{
    var particles = new List<Particle>();
    foreach (var particle in indata.Select((x, i) => (x, i)))
        particles.Add(new Particle(particle.Item1, particle.Item2));

    var iterationsSinceCollide = 0;
    while (iterationsSinceCollide < 100)
    {
        var removed = particles.RemoveAll(x => particles.Any(z => z.Collides(x)));
        if (removed != 0)
            iterationsSinceCollide = 0;
        else
            iterationsSinceCollide++;            

        foreach (var particle in particles)
            particle.Update();
    }

    return particles.Count();
}

private class Particle
{
    public Particle(string indata, int index)
    {
        Index = index;
        Position = new Vector(GetPart(indata, 1));
        Velocity = new Vector(GetPart(indata, 2));
        Acceleration = new Vector(GetPart(indata, 3));
    }

    public int Index {get; private set; }
    public Vector Position { get; set; }
    public Vector Velocity { get; set; }
    public Vector Acceleration { get; set; }

    public int Distance => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

    public void Update()
    {
        Velocity.Add(Acceleration);
        Position.Add(Velocity);
    }

    public bool Collides(Particle other)
    {
        return Position.Equal(other.Position) && Index != other.Index;
    }

    private string GetPart(string indata, int index)
    {
        var startIndex = indata.Select((x, i) => (x, i)).Where(x => x.Item1 == '<').Skip(index-1).First();
        var endIndex = indata.Select((x, i) => (x, i)).Where(x => x.Item1 == '>').Skip(index-1).First();

        return indata.Substring(startIndex.Item2+1, endIndex.Item2-startIndex.Item2-1);
    }
}

private class Vector
{
    public Vector(string indata)
    {
        var split = indata.Split(',');
        X = Int32.Parse(split[0]);    
        Y = Int32.Parse(split[1]);    
        Z = Int32.Parse(split[2]);    
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public void Add(Vector other)
    {
        this.X += other.X;
        this.Y += other.Y;
        this.Z += other.Z;
    }

    public bool Equal(Vector other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }
}