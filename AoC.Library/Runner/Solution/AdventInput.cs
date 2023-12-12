using AoC.Library.Utils;

namespace AoC.Library.Runner;

public record AdventInput(string Raw, bool Print)
{
    private Lazy<string[]> _lines = new(() => Raw.SmartSplit("\n"));
    private Lazy<string[]> _fullLines = new(() => Raw.Split("\n"));

    public string[] Lines => _lines.Value;

    public string[] FullLines => _fullLines.Value;

    public int Width => Lines[0].Length;

    public int Height => Lines.Length;

    public static implicit operator AdventInput(string s) => new(s, false);
}