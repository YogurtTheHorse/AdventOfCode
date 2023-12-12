using AoC.Library.Utils;

namespace AoC.Library.Runner;

public record AdventInput(string Raw)
{
    private Lazy<string[]> _lines = new(() => Raw.SmartSplit("\n"));

    public string[] Lines => _lines.Value;

    public static implicit operator AdventInput(string s) => new(s);
}