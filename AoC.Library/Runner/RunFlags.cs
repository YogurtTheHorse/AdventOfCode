namespace AoC.Library.Runner;

[Flags]
public enum RunFlags
{
    None = 0,
    
    PrintInput = 1 << 0,
    PrintOutput = 1 << 1
}
