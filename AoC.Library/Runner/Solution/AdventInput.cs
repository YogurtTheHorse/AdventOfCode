using AoC.Library.Utils;

namespace AoC.Library.Runner;

public record AdventInput(string Raw, bool Print)
{
    private readonly Lazy<string[]> _lines = new(() => Raw.SmartSplit("\n"));
    private readonly Lazy<string[]> _fullLines = new(() => Raw.Split("\n"));

    public string[] Lines => _lines.Value;

    public string[] FullLines => _fullLines.Value;

    public int Width => Lines[0].Length;

    public int Height => Lines.Length;

    public char[,] SquareMap() => SquareMap(Helpers.Id);

    public T[,] SquareMap<T>(Func<char, T> map)
    {
        var arr = new T[Width, Height];

        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            arr[x, y] = map(this[x, y]);

        return arr;
    }

    public char this[int x, int y] => Lines[y][x];

    public string this[int y] => Lines[y];

    public static implicit operator AdventInput(string s) => new(s, false);
}