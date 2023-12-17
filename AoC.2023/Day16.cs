using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 16, AdventParts.PartTwo)]
public class Day16 : AdventSolution
{
    public override object SolvePartOne() => Solve(new((0, 0), Direction.Right));

    public override object SolvePartTwo()
    {
        var m = 0;

        for (int x = 0; x < Input.Width; x++)
        {
            m = Math.Max(m, Solve(new Beam((x, 0), Direction.Down)));
            m = Math.Max(m, Solve(new Beam((x, Input.Height - 1), Direction.Up)));
        }

        for (int y = 0; y < Input.Height; y++)
        {
            m = Math.Max(m, Solve(new Beam((0, y), Direction.Right)));
            m = Math.Max(m, Solve(new Beam((Input.Width - 1, y), Direction.Left)));
        }

        return m;
    }

    private int Solve(Beam startBeam)
    {
        bool infinite = false;
        var beams = new List<Beam> {
            startBeam
        };

        var dirsMap = Input.SquareMap(_ => new List<Direction>());

        while (beams.Any())
        {
            var b = beams[0];

            if (!b.Pos.InBounds(Input.Width, Input.Height) && infinite)
            {
                b = b with {
                    Pos = b.Pos.Loop(Input.Width, Input.Height)
                };
            }

            if (!b.Pos.InBounds(Input.Width, Input.Height) || dirsMap[b.Pos.X, b.Pos.Y].Contains(b.Direction))
            {
                beams.RemoveAt(0);

                continue;
            }

            dirsMap[b.Pos.X, b.Pos.Y].Add(b.Direction);
            var c = Input[b.Pos.X, b.Pos.Y];

            if (c == '.' || (c == '-' && b.Direction.IsHorizontal()) || (c == '|' && b.Direction.IsVertical()))
            {
                MoveToNext(b, b.Direction);
            }
            else if (c is '/' or '\\')
            {
                var nextDir = b.Direction switch {
                    Direction.Up => Direction.Right,
                    Direction.Right => Direction.Up,
                    Direction.Down => Direction.Left,
                    _ => Direction.Down
                };
                if (c == '\\') nextDir = nextDir.Opposite();

                MoveToNext(b, nextDir);
            }
            else if (c is '-')
            {
                beams.Remove(b);
                beams.Add(new Beam(b.Pos + Direction.Right, Direction.Right));
                beams.Add(new Beam(b.Pos - Direction.Right, Direction.Left));
            }
            else if (c == '|')
            {
                beams.Remove(b);
                beams.Add(new Beam(b.Pos + Direction.Up, Direction.Up));
                beams.Add(new Beam(b.Pos - Direction.Up, Direction.Down));
            }

            // PrintMap();
        }

        return dirsMap.Enumerate().Count(l => l.Any());

        void PrintMap()
        {
            for (var y = 0; y < Input.Height; y++)
            {
                for (var x = 0; x < Input.Width; x++)
                {
                    if (Input[x, y] != '.' || dirsMap[x, y].Count == 0)
                    {
                        Write(Input[x, y]);
                    }
                    else if (dirsMap[x, y].Count == 1)
                    {
                        Write(dirsMap[x, y][0].AsAscii());
                    }
                    else
                    {
                        Write(dirsMap[x, y].Count);
                    }
                }

                WriteLine("");
            }

            WriteLine("");
        }

        void MoveToNext(Beam b, Direction dir)
        {
            var next = b.Pos + dir;

            if (next.InBounds(Input.Width, Input.Height))
            {
                beams[0] = new Beam(next, dir);
            }
            else if (infinite)
            {
                beams[0] = new Beam(
                    b.Pos.Loop(Input.Width, Input.Height),
                    dir
                );
            }
            else
            {
                beams.Remove(b);
            }
        }
    }

    public record Beam(Point Pos, Direction Direction);
}