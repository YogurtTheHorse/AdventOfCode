using AoC.Library.Utils;

namespace AoC.Library.Runner;

public record AdventInput(string Raw)
{
    private Lazy<string[]> _lines = new(() => Raw.SmartSplit("\n"));

    public string[] Lines => _lines.Value;

    public int Width => Lines[0].Length;

    public int Height => Lines.Length;

    public static implicit operator AdventInput(string s) => new(s);
}