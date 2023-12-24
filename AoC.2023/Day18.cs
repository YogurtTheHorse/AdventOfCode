using System.Globalization;
using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

using Point = PointBase<int>;

[DateInfo(2023, 18, AdventParts.All)]
public class Day18 : AdventSolution
{
    public override object SolvePartOne()
    {
        var commands = Input
            .Lines
            .Select(l =>
            {
                var (dir, path, _) = l.SmartSplit().Unpack3();

                return (
                    dir switch {
                        "U" => Direction.Up,
                        "R" => Direction.Right,
                        "D" => Direction.Down,
                        _ => Direction.Left
                    },
                    path.ToInt()
                );
            })
            .ToArray();

        return SolveMap(commands);
    }

    private object SolveMap((Direction, int)[] commands)
    {
        var pos = new Point(0, 0);

        var points = new List<Point> {
            pos
        };

        var perm = 0;

        foreach (var (dir, cnt) in commands)
        {
            pos += (Point)dir * cnt;
            perm += cnt;

            points.Add(pos);
        }

        var area = GetArea(points);


        return area + perm * 0.5 + 1L;
    }

    public override object SolvePartTwo()
    {
        var commands = Input
            .Lines
            .Select(l =>
            {
                var clr = l.SmartSplit()[^1];
                var cnt = int.Parse(clr[2..^2], NumberStyles.HexNumber);
                var dir = clr[^2] - '0';

                return (
                    dir switch {
                        3 => Direction.Up,
                        2 => Direction.Left,
                        1 => Direction.Down,
                        _ => Direction.Right,
                    },
                    cnt
                );
            })
            .ToArray();

        return SolveMap(commands);
    }

    // copied from stack overflow not to write myself gauss formula
    private double GetDeterminant(double x1, double y1, double x2, double y2) => x1 * y2 - x2 * y1;

    private double GetArea(List<Point> vertices)
    {
        var area = GetDeterminant(vertices[^1].X, vertices[^1].Y, vertices[0].X, vertices[0].Y);

        for (var i = 1; i < vertices.Count; i++)
        {
            area += GetDeterminant(vertices[i - 1].X, vertices[i - 1].Y, vertices[i].X, vertices[i].Y);
        }

        return area / 2;
    }
}