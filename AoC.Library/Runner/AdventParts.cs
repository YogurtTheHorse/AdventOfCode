namespace AoC.Library.Runner;

[Flags]
public enum AdventParts
{
    None = 0,
    PartOne = 1,
    PartTwo = 2,
    
    All = PartOne | PartTwo
}