using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

[DateInfo(2023, 17, AdventParts.All)]
public class Day17 : AdventSolution
{
    private readonly Direction[] _dirs = {
        Direction.Up, Direction.Right, Direction.Down, Direction.Left
    };
    public override object SolvePartOne() => Solve(1, 3);

    public override object SolvePartTwo() => Solve(4, 10);

    private int Solve(int minDistance, int maxDistance)
    {
        var queue = new List<(int, Point, Direction)>();
        var visits = new bool[Input.Width, Input.Height, 7];
        var memory = new int[Input.Width, Input.Height, 7];

        var map = Input.SquareMap(c => c - '0');
        
        for (int i = 0; i < Input.Width; i++)
        for (int j = 0; j < Input.Height; j++)
        for (int k = 0; k < 7; k++)
        {
            visits[i, j, k] = false;
            memory[i, j, k] = int.MaxValue;
        }

        queue.Add((0, Point.Zero, Direction.None));
        
        while (queue.Count > 0)
        {
            var v = queue.MinBy(i => i.Item1);
            var (cost, p, noDir) = v;
            queue.Remove(v);

            if (p.X >= Input.Width - 1 && p.Y >= Input.Height - 1) return cost;
            if (visits[p.X, p.Y, (int)noDir]) continue;

            visits[p.X, p.Y, (int)noDir] = true;
            
            foreach (var dir in _dirs)
            {
                if (dir == noDir || dir.Opposite() == noDir) continue;
                
                var increase = 0;

                for (var dist = 1; dist <= maxDistance; dist++)
                {
                    var next = p + ((Point)dir) * dist;

                    if (!next.InBounds(Input.Width, Input.Height)) continue;

                    increase += map[next.X, next.Y];
                    if (dist < minDistance) continue;

                    var nc = cost + increase;
                        
                    if (memory[next.X, next.Y, (int)dir] <= nc) continue;

                    memory[next.X, next.Y, (int)dir] = nc;
                    queue.Add((nc, next, dir));
                }
            }
        }

        return -1;
    }
}