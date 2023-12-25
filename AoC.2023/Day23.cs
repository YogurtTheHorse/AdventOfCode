using System.Runtime.CompilerServices;
using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

using Point = PointBase<int>;

[DateInfo(2023, 23, AdventParts.All)]
public class Day23 : AdventSolution
{
    private static readonly Direction[] Dirs = {
        Direction.Right, Direction.Down, Direction.Up, Direction.Left
    };

    private int _currMax = 0;
    private bool[,] _visits = null!;

    private readonly Stack<Point> _path = new();

    private int Solve(int startX, bool isPartOne)
    {
        _visits = Input.SquareMap(_ => false);

        return InnerSolve(new Point(startX, 0), isPartOne) - 1;
    }

    private int InnerSolve(Point start, bool isPartOne)
    {
        var pushed = 1;
        _path.Push(start);

        while (true)
        {
            var curr = _path.Peek();

            if (curr.Y == Input.Height - 1)
            {
                var res = _path.Count;

                if (res > _currMax)
                {
                    Console.WriteLine(_currMax = res);
                }

                Clean(pushed);

                return res;
            }

            _visits[curr.X, curr.Y] = true;

            var neig = (Input[curr] switch {
                    '#' => Array.Empty<Direction>(),
                    '>' when isPartOne => Direction.Right.Single(),
                    'v' when isPartOne => Direction.Down.Single(),
                    '<' when isPartOne => Direction.Left.Single(),
                    _ => Dirs
                })
                .Select(d => d + curr)
                .Where(p => p.InBounds(Input.Width, Input.Height))
                .Where(p => Input[p] != '#')
                .Where(p => !_visits[p.X, p.Y])
                .ToArray();

            switch (neig.Length)
            {
                case 0:
                    Clean(pushed);

                    return 0;

                case 1:
                    _path.Push(neig[0]);
                    pushed++;

                    break;

                default:
                    var currMax = 0;

                    foreach (var n in neig)
                    {
                        currMax = Math.Max(InnerSolve(n, isPartOne), currMax);
                    }

                    Clean(pushed);

                    return currMax;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Clean(int pushed)
    {
        for (var i = 0; i < pushed; i++)
        {
            var curr = _path.Pop();
            _visits[curr.X, curr.Y] = false;
        }
    }

    public override object SolvePartOne()
    {
        var startX = Input[0].IndexOf('.');


        return Solve(startX, true);
    }

    public override object SolvePartTwo()
    {
        var startX = Input[0].IndexOf('.');

        return Solve(startX, false);
        // launched at 1:08
        //6235 01:10 1:41
        //6491 01:12
    }
}