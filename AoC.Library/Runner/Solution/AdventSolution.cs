namespace AoC.Library.Runner;

public abstract class AdventSolution
{
    public AdventInput Input { get; set; } = null!;
    
    public abstract object SolvePartOne();

    public abstract object SolvePartSecond();

    public void WriteLine(object s)
    {
        if (!Input.Print) return;

        Console.WriteLine(s);
    }

    public void Write(object s)
    {
        if (!Input.Print) return;

        Console.Write(s);
    }
}