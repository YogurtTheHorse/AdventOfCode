namespace AoC.Library.Runner;

[Flags]
public enum RunType
{
    Example = 1,
    Full = 2,
    Custom = 4,

    All = Example | Full | Custom
}