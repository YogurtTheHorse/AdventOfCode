using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 13, AdventParts.All)]
public class Day13 : AdventSolution
{
    public override object SolvePartOne() => Input
        .Raw
        .SmartSplit("\n\n")
        .Select(m => SolveMap(m, 0))
        .Sum();

    public override object SolvePartSecond()  => Input
        .Raw
        .SmartSplit("\n\n")
        .Select(m => SolveMap(m, 1))
        .Sum();

    private long SolveMap(string map, int requiredBad)
    {
        var lines = map.SmartSplit("\n");
        var w = lines[0].Length;
        var h = lines.Length;

        var original = new char[w, h];
        var rotated = new char[h, w];

        for (int x = 0; x < w; x++)
        for (int y = 0; y < h; y++)
        {
            original[x, y] = lines[y][x];
            rotated[h - (y + 1), x] = lines[y][x];
        }

        return 100 * SolveHorizontal(original, requiredBad) + SolveHorizontal(rotated, requiredBad);
    }

    private int SolveHorizontal(char[,] map, int requiredBad)
    {
        var res = 0;
        int w = map.GetLength(0);
        int h = map.GetLength(1);

        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++) Write(map[x, y]);
            WriteLine("");
        }

        for (var i = 1; i < h; i++)
        {
            var badAmount = 0;

            for (var j = 0; i - j > 0 && i + j < h; j++)
            {
                for (var x = 0; x < w; x++)
                {
                    if (map[x, i - j - 1] != map[x, i + j])
                    {
                        badAmount++;
                        
                        if (badAmount > requiredBad) break;
                    }
                }

                if (badAmount > requiredBad) break;
            }

            if (badAmount == requiredBad)
            {
                WriteLine(i);
                res += i;
            }
        }
        WriteLine("");

        return res;
    }
}