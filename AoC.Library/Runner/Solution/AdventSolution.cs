namespace AoC.Library.Runner;

public abstract class AdventSolution
{
    public AdventInput Input { get; set; }
    
    public abstract object SolvePartOne();

    public abstract object SolvePartSecond();
}